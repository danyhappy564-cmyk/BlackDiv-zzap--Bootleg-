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

// v2 - corrected after cloning SAIN's own dependencies (BigBrain, MoreBotsAPI) for reference
// and reading SAIN's SmoothTurnPatch properly (SAIN/Patches/Shoot/AimDataPatches.cs).
//
// v1 of this fix called PlayerComponent.CharacterController.SetTargetLookDirection(...),
// copying what SAIN's own SAINSteeringClass.TickPlayerSteering does. That call does nothing
// useful while SAINLayersActive is false - which is exactly the state this whole bug lives
// in. Here is why:
//
//   SAIN's SmoothTurnPatch prefixes vanilla BotSteering.Steering() - the method that
//   actually turns the character every frame. When SAINLayersActive is true, it applies
//   PlayerComponent.CharacterController's smoothed TurnData and blocks the original method.
//   When SAINLayersActive is false (our case), it does the opposite: it copies vanilla's OWN
//   BotSteering._lookDirection field INTO TurnData (bookkeeping, so SAIN doesn't jump if it
//   regains control later) and then lets the ORIGINAL vanilla method run. So anything written
//   to CharacterController's TurnData while SAINLayersActive is false gets silently
//   overwritten by that same sync on the very next frame and never reaches the character -
//   v1 compiled, ran, and did nothing visible. No raid log was needed to catch this; reading
//   AimDataPatches.cs was enough.
//
//   The field that actually drives rotation in that branch is vanilla's own
//   BotSteering._lookDirection. SAIN's own SmoothTurnPatch itself writes to it directly
//   ("__instance._lookDirection = ...") from SAIN's separate assembly, which is the proof
//   that writing to it from another assembly (ours) is fine - it's a real field this
//   published mod already touches this way, not a guess at BSG's obfuscated API.
//
// Root cause and scope are unchanged from BDSteeringHandoffDiagnostic.cs and are now
// confirmed (not theorized) by reading MoreBotsAPI's real source
// (danyhappy564-cmyk/MoreBotsAPI_Check): a BD/Wedge bot that loses its SAIN GoalEnemy while
// retreating falls to our own HuntTargetLayer, whose HuntTargetAction has no "look at
// whoever is shooting me" logic - it only ever calls BotOwner.Steering.LookToMovingDirection().
internal class BDUnderFireSteeringFallback : ModulePatch
{
    private static readonly HashSet<int> BdRoles = new HashSet<int>
    {
        848420, 848421, 848422, 848423, 848424, 848426,
    };

    private static readonly HashSet<string> _reportedEngaged = new HashSet<string>();

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
                // SAIN has this bot back this tick - let it steer through its own pipeline,
                // don't fight it.
                return;
            }

            Vector3 point = bot.Memory.UnderFireFromPosition + bot.Steering.WeaponRootOffset;
            Vector3 direction = (point - bot.Transform.WeaponRoot).normalized;

            // Runs after HuntTargetAction.Update's own
            // "BotOwner.Steering.LookToMovingDirection()" call, so this intentionally
            // overrides it for this tick - the vanilla field vanilla's own
            // BotSteering.Steering() reads when SAIN has yielded control, not SAIN's.
            botOwner.Steering._lookDirection = direction;

            // Once per bot: proof the fallback actually fired, not just that it compiled.
            if (_reportedEngaged.Add(botOwner.ProfileId))
            {
                Plugin.LogSource.LogWarning(
                    $"[BDUnderFireSteeringFallback] engaged for {botOwner.name} - forcing look direction "
                    + "toward under-fire source while SAINLayersActive is false.");
            }
        }
        catch (Exception e)
        {
            Plugin.LogSource.LogWarning($"[BDUnderFireSteeringFallback] failed: {e.Message}");
        }
    }
}
