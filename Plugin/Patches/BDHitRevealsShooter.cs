using System;
using System.Collections.Generic;
using System.Reflection;
using EFT;
using EFT.Ballistics;
using SAIN.Models.Structs;
using SAIN.Preset.Shared.Enums;
using SAIN.SAINComponent.Classes;
using SAIN.SAINComponent.Classes.EnemyClasses;
using SPT.Reflection.Patching;
using UnityEngine;

namespace BlackDiv.Patches;

// Root-cause fix for "BD bot gets shot and never reacts" (2026-09-24). BDUnderFireSteeringFallback
// only turns the body toward the shooter; this puts SAIN's own combat AI back in charge instead.
//
// SAIN only treats an enemy as known (EnemyKnownChecker) while its KnownPlaces were updated
// within ForgetEnemyTime, and only vision or hearing ever update them. Being hit does not:
// BotHitByEnemyClass.GetHit only records the hit on an enemy that is already active, the
// OnBeingShotByEnemy event it raises has no subscribers anywhere in SAIN, and
// EEnemyPlaceType.Injury is declared but never used. So a bot hit by a shooter it neither sees
// nor hears keeps GoalEnemy == null, EnemyDecisionClass returns ECombatDecision.None, every SAIN
// combat layer stays inactive, and BigBrain falls through to HuntTargetLayer.
//
// After SAIN's own GetHit, register the shooter as a SAIN enemy (no-op if it isn't a valid enemy
// for this bot, e.g. a squadmate) and record its position as an Injury place. That makes it a
// known enemy, ChooseEnemy picks it up on the next decision tick, and SAIN's combat layers
// (return fire, cover, retreat logic) take the bot back.
internal class BDHitRevealsShooter : ModulePatch
{
    private static readonly HashSet<int> BdRoles = new HashSet<int>
    {
        848420, 848421, 848422, 848423, 848424, 848426,
    };

    private static readonly HashSet<string> _reported = new HashSet<string>();

    protected override MethodBase GetTargetMethod()
    {
        return typeof(BotHitByEnemyClass).GetMethod(nameof(BotHitByEnemyClass.GetHit), BindingFlags.Public | BindingFlags.Instance);
    }

    [PatchPostfix]
    protected static void PatchPostfix(BotHitByEnemyClass __instance, DamageInfo DamageInfo)
    {
        try
        {
            BotOwner botOwner = __instance?.BotOwner;
            if (botOwner?.Profile?.Info?.Settings == null || !BdRoles.Contains((int)botOwner.Profile.Info.Settings.Role))
            {
                return;
            }

            IPlayer shooter = DamageInfo.Player?.iPlayer;
            if (shooter == null || shooter.HealthController?.IsAlive != true)
            {
                return;
            }

            var bot = __instance.Bot;
            Enemy enemy = bot?.EnemyController?.CheckAddEnemy(shooter);
            if (enemy == null)
            {
                return;
            }

            bool wasKnown = enemy.EnemyKnown;
            var report = new SAINHearingReport
            {
                position = shooter.Position,
                soundType = SAINSoundType.Shot,
                placeType = EEnemyPlaceType.Injury,
                isDanger = true,
                shallReportToSquad = false,
            };
            enemy.KnownPlaces.UpdatePersonalHeardPosition(report, Time.time);
            bot.Memory.SetUnderFire(enemy, shooter.Position);

            // Once per bot, and only when this actually changed something: an enemy SAIN had
            // already forgotten (or never registered) became known again because of the hit.
            if (!wasKnown && _reported.Add(botOwner.ProfileId))
            {
                Plugin.LogSource.LogWarning(
                    $"[BDHitRevealsShooter] {botOwner.name} was hit by an enemy SAIN did not know about ({shooter.ProfileId}) - "
                    + "marked it known so SAIN's combat layers take over.");
            }
        }
        catch (Exception e)
        {
            Plugin.LogSource.LogWarning($"[BDHitRevealsShooter] failed: {e.Message}");
        }
    }
}
