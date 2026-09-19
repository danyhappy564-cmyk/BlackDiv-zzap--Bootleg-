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
// Postfixing SAINBotInfoClass.GetPersonality forces GigaChad unconditionally for all six
// BD/Wedge roles. This is the single method both the constructor and any later
// preset-reload (F12 live tuning triggers SAINBotInfoClass.UpdatePresetSettings ->
// ConfigureBot -> GetPersonality) funnel through, so the override survives a mid-raid
// preset change rather than only applying at spawn and getting rolled away later.
internal class BDForceGigaChadPersonality : ModulePatch
{
    private static readonly HashSet<int> BdRoles = new HashSet<int>
    {
        848420, 848421, 848422, 848423, 848424, 848426,
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
            if (__instance?.Profile == null || !BdRoles.Contains((int)__instance.Profile.WildSpawnType))
            {
                return;
            }

            if (SAINPlugin.LoadedPreset?.PersonalityManager?.PersonalityDictionary.TryGetValue(EPersonality.GigaChad, out var gigaChadSettings) == true)
            {
                __result = EPersonality.GigaChad;
                settings = gigaChadSettings;
            }
        }
        catch (Exception e)
        {
            Plugin.LogSource.LogWarning($"[BDForceGigaChadPersonality] failed: {e.Message}");
        }
    }
}
