using System;
using System.Collections.Generic;
using System.Reflection;
using SAIN;
using SAIN.Preset.Shared.Models.Preset.Personalities;
using SAIN.Preset.Shared.Personalities.BasePersonality;
using SAIN.SAINComponent.Classes.Info;
using SPT.Reflection.Patching;

namespace BlackDiv.Patches;

// Black Division is meant to hit like a monster faction even against vanilla AI - it's
// the faction's own identity, not something to leave to chance. Left alone, SAIN's own
// personality assignment gives BD/Wedge bots nothing of the sort: BlackDivSainRegistrations.cs
// never sets a personality, none of BD's six custom WildSpawnTypes (848420-848426) appear
// in any personality's AllowedTypes or in SAIN's PERS_BOSSES dictionary (both keyed to
// vanilla WildSpawnType values), and BD isn't IsPMC either (only pmcBEAR/pmcUSEC qualify).
// So PersonalityDictionary.GetPersonality falls through to its last branch and BD bots
// default to Normal, with only a stray 3% chance per spawn of randomly rolling GigaChad.
//
// 2026-09-19: split BD's five non-Wedge roles from Wedge itself, each independently
// configurable via BepInEx (see BDPersonalityConfig.cs) rather than hardcoded, after
// checking whether Wedge already has some distinct AI worth preserving - it doesn't.
// docs/BOSSWEDGE-AI-REPORT.md in the Icebreaker repo describes a "BossWedge" class, but
// that's vanilla retail's own boss brain (ABossLogic-derived) used there purely as
// blueprint material for an unrelated custom Icebreaker boss - grepping this repo
// confirms nothing here references ABossLogic or that class at all. BD's Wedge is
// currently just WildSpawnType 848424 running the exact same SAIN pipeline as the other
// five roles, so there's nothing distinct to preserve; the split exists purely so the two
// can be tuned differently later if BD ever grows Wedge-specific logic.
//
// Postfixing SAINBotInfoClass.GetPersonality applies the configured personality for
// whichever group a bot belongs to, but only when that personality's own
// Assignment.Enabled is still true in the user's SAIN preset (same flag SAIN's own
// PersonalityDictionary.canBotBePersonality checks before assigning it to anyone). If the
// user disabled it, we back off and leave GetPersonality's own result alone, so the bot
// falls back to whatever personality the user actually left available rather than one
// they explicitly turned off.
//
// GetPersonality is the single method both the constructor and any later preset-reload
// (F12 live tuning triggers SAINBotInfoClass.UpdatePresetSettings -> ConfigureBot ->
// GetPersonality) funnel through, so the override (or the deliberate non-override)
// survives a mid-raid preset change rather than only applying at spawn.
internal class BDPersonalityOverride : ModulePatch
{
    private const int WedgeWildSpawnType = 848424;

    private static readonly HashSet<int> NonWedgeBdRoles = new HashSet<int>
    {
        848420, 848421, 848422, 848423, 848426,
    };

    protected override MethodBase GetTargetMethod()
    {
        return typeof(SAINBotInfoClass).GetMethod(nameof(SAINBotInfoClass.GetPersonality), BindingFlags.Public | BindingFlags.Instance);
    }

    [PatchPostfix]
    protected static void PatchPostfix(SAINBotInfoClass __instance, ref EPersonality __result, ref PersonalitySettingsClass settings)
    {
        try
        {
            if (__instance?.Profile == null)
            {
                return;
            }

            int wildSpawnType = (int)__instance.Profile.WildSpawnType;

            EPersonality? wanted;
            if (wildSpawnType == WedgeWildSpawnType)
            {
                wanted = BDPersonalityConfig.WedgePersonality?.Value;
            }
            else if (NonWedgeBdRoles.Contains(wildSpawnType))
            {
                wanted = BDPersonalityConfig.BdPersonality?.Value;
            }
            else
            {
                return;
            }

            if (
                wanted.HasValue
                && SAINPlugin.LoadedPreset?.PersonalityManager?.PersonalityDictionary.TryGetValue(wanted.Value, out var wantedSettings) == true
                && wantedSettings.Assignment.Enabled
            )
            {
                __result = wanted.Value;
                settings = wantedSettings;
            }
        }
        catch (Exception e)
        {
            Plugin.LogSource.LogWarning($"[BDPersonalityOverride] failed: {e.Message}");
        }
    }
}
