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

// v1 -> v2 -> v3. v2 was corrected after cloning SAIN's own dependencies (BigBrain, MoreBotsAPI) for reference
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
//   So the rotation has to go through vanilla BotSteering.Steering(). v2 assumed that
//   meant writing BotSteering._lookDirection; decompiling Steering() showed it recomputes
//   that field from SteeringMode first, so v3 sets the mode instead - see the call site.
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

            // v3 (2026-09-24): v2 wrote BotSteering._lookDirection directly, which never
            // turned the bot either. Decompiled BotSteering.Steering() recomputes that field
            // from SteeringMode before applying any rotation, and HuntTargetAction.Update
            // sets SteeringMode = ToMovingDirection every tick. In that mode Steering()
            // overwrites _lookDirection with Mover.DirCurPoint while moving, and returns
            // early without rotating at all while standing still. LookToPoint switches the
            // mode to ToCustomPoint, where Steering() derives _lookDirection from our point
            // and actually applies it. This postfix runs after HuntTargetAction.Update's
            // LookToMovingDirection() call, so it wins for this tick.
            botOwner.Steering.LookToPoint(point);

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
