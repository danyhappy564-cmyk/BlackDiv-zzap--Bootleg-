using System;
using System.Collections.Generic;
using System.Reflection;
using EFT;
using SAIN.Components;
using SPT.Reflection.Patching;

namespace BlackDiv.Patches;

// 2026-09-18 report: BD/Wedge bots seen fleeing under fire, dropping prone, then never
// turning to face the shooter again - frozen, while clearly still alive and still taking
// hits.
//
// WORKING THEORY, not yet confirmed by a raid log:
//
//   SAIN's SAINMoverClass.ManualUpdate only calls Bot.CurrentAction?.OnSteeringTicked() and
//   Bot.Steering.TickPlayerSteering() - the two calls that actually turn the bot to face a
//   threat - inside "if (Bot.SAINLayersActive)". SAINLayersActive is false whenever
//   BigBrain's active layer for that bot isn't one of SAIN's own layers.
//
//   SAIN's own combat/avoid-threat layers only activate when the bot has a live GoalEnemy
//   (SAIN's EnemyDecisionClass.GetDecision returns None the instant enemy == null). If
//   GoalEnemy drops while a BD/Wedge bot is mid-retreat - plausible, since
//   SelfActionDecisionClass.TryReload already has a documented, permanent failure mode for
//   these bots' inventories (see that class's own 2026-09-15 comment: BotReload.CanReload
//   throws walking their equipment, so they can never actually finish a reload once dry) -
//   SAIN's combat layers go inactive and BigBrain falls through to whatever else is
//   registered for the BD/Wedge roles. SainBrainLayerPatch removes the vanilla fallback
//   layers on purpose for these roles (that removal is what lets SAIN run for them at all),
//   so the only thing left standing is our own HuntTargetLayer (priority 10, see
//   Plugin.cs), whose steering has no "look at whoever is shooting me" logic.
//
//   Net effect: SAINLayersActive goes false, SAIN's steering stops being pushed into the
//   character controller, and the bot's facing direction freezes at whatever it last was -
//   including while still being shot.
//
// This patch does not change behavior. It logs the exact moment the theory predicts, once
// per bot per active-layer change, so the next raid log either confirms or kills the theory
// before any fix gets written against APIs nobody has verified live yet.
internal class BDSteeringHandoffDiagnostic : ModulePatch
{
    private static readonly HashSet<int> BdRoles = new HashSet<int>
    {
        848420, 848421, 848422, 848423, 848424, 848426,
    };

    private static readonly HashSet<string> _reported = new HashSet<string>();

    protected override MethodBase GetTargetMethod()
    {
        return typeof(BotComponent).GetMethod(nameof(BotComponent.ManualUpdate), BindingFlags.Public | BindingFlags.Instance);
    }

    [PatchPostfix]
    protected static void PatchPostfix(BotComponent __instance)
    {
        try
        {
            BotOwner botOwner = __instance.BotOwner;
            if (botOwner?.Profile?.Info?.Settings == null || !BdRoles.Contains((int)botOwner.Profile.Info.Settings.Role))
            {
                return;
            }

            if (__instance.SAINLayersActive)
            {
                return;
            }

            string key = $"{botOwner.ProfileId}:{__instance.ActiveLayer}";
            if (!_reported.Add(key))
            {
                return;
            }

            var mem = __instance.Memory;
            Plugin.LogSource.LogWarning(
                $"[BDSteeringDiag] {botOwner.name} SAINLayersActive=false, ActiveLayer={__instance.ActiveLayer}, "
                + $"GoalEnemy={(__instance.GoalEnemy != null ? "set" : "null")}, "
                + $"HaveBullets={botOwner.WeaponManager?.HaveBullets}, "
                + $"Reloading={botOwner.WeaponManager?.Reload?.Reloading}, "
                + $"UnderFire={mem?.LastUnderFireEnemy != null}, "
                + $"IsInPronePose={__instance.Player?.IsInPronePose}");
        }
        catch (Exception e)
        {
            Plugin.LogSource.LogWarning($"[BDSteeringDiag] failed: {e.Message}");
        }
    }
}
