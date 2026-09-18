using System;
using System.Collections.Generic;
using System.Reflection;
using EFT;
using MoreBotsAPI.Behavior.Actions;
using SAIN;
using SAIN.Components;
using SPT.Reflection.Patching;
using UnityEngine;

namespace BlackDiv.Patches;

// Confirmed by reading the actual MoreBotsAPI source (danyhappy564-cmyk/MoreBotsAPI_Check)
// after BDSteeringHandoffDiagnostic.cs was written to test this as a theory. It is no longer
// a theory:
//
//   HuntTargetLayer.IsActive() (MoreBotsAPI.Behavior.Layers.HuntTargetLayer) becomes SAIN's
//   BigBrain competitor the moment BotHuntManager.HasHuntTarget() is true, independently of
//   SAIN's own GoalEnemy tracking. Priority 10 loses to every SAIN combat/avoid-threat layer
//   (62-104), so it only actually wins BigBrain's layer pick when SAIN has none active - which
//   happens whenever SAIN has no GoalEnemy (SAIN's EnemyDecisionClass.GetDecision returns
//   None the instant enemy == null). SelfActionDecisionClass.TryReload already documents a
//   permanent reload failure for these bots' inventories that can drive a bot to lose its
//   GoalEnemy while mid-retreat.
//
//   HuntTargetAction.Update (the action HuntTargetLayer runs) does
//   "BotOwner.Steering.LookToMovingDirection(); baseSteeringLogic.Update(BotOwner);" and
//   nothing else - no "look at whoever is shooting me" logic at all. And because BigBrain's
//   active layer for that bot isn't a SAIN layer, SAIN.Classes.Bot.Mover.SAINMoverClass never
//   calls its own TickPlayerSteering() for it either (gated behind
//   "if (Bot.SAINLayersActive)"). Net effect: a BD/Wedge bot that loses its SAIN GoalEnemy
//   while retreating freezes facing whatever direction it had, even while still taking fire.
//   This is a direct consequence of SainBrainLayerPatch removing BD/Wedge's vanilla fallback
//   layers (mandatory for SAIN to run on them at all, see that file's own notes) - vanilla
//   bots would have had their own native reaction to being shot to fall back on; these don't.
//
// Fix, scoped to BD/Wedge roles only: postfix HuntTargetAction.Update. When the vanilla
// "under fire" flag is set (BotOwner.Memory.IsUnderFire - the same one SAIN itself sets via
// SAINMemoryClass.SetUnderFire) and SAIN isn't currently steering this bot, turn it to face
// where the fire is coming from. The point and the call used here are copied from SAIN's own
// SAINSteeringClass.LookToUnderFirePos / TickPlayerSteering - not guessed - because both are
// public on the SAIN source in this session (SAIN-zzap--Bootleg-) and SAIN is a hard
// dependency of this exact call path already.
internal class BDUnderFireSteeringFallback : ModulePatch
{
    private static readonly HashSet<int> BdRoles = new HashSet<int>
    {
        848420, 848421, 848422, 848423, 848424, 848426,
    };

    protected override MethodBase GetTargetMethod()
    {
        return typeof(HuntTargetAction).GetMethod(nameof(HuntTargetAction.Update), BindingFlags.Public | BindingFlags.Instance);
    }

    [PatchPostfix]
    protected static void PatchPostfix(HuntTargetAction __instance)
    {
        try
        {
            BotOwner botOwner = __instance.BotOwner;
            if (botOwner?.Profile?.Info?.Settings == null || !BdRoles.Contains((int)botOwner.Profile.Info.Settings.Role))
            {
                return;
            }

            if (botOwner.Memory?.IsUnderFire != true)
            {
                return;
            }

            if (!SAINEnableClass.GetSAIN(botOwner.ProfileId, out BotComponent bot) || bot == null)
            {
                return;
            }

            if (bot.SAINLayersActive)
            {
                // SAIN has this bot back this tick - let it steer, don't fight it.
                return;
            }

            Vector3 point = bot.Memory.UnderFireFromPosition + bot.Steering.WeaponRootOffset;
            Vector3 direction = (point - bot.Transform.WeaponRoot).normalized;
            bot.PlayerComponent.CharacterController.SetTargetLookDirection(direction, botOwner, bot);
        }
        catch (Exception e)
        {
            Plugin.LogSource.LogWarning($"[BDUnderFireSteeringFallback] failed: {e.Message}");
        }
    }
}
