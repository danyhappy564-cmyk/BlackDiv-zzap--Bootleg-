using BepInEx.Configuration;
using SAIN.Preset.Shared.Models.Preset.Personalities;

namespace BlackDiv;

// Kept out of Plugin.cs on purpose. Plugin.Awake() must never reference a SAIN type
// directly in its own method body: under Mono (what this game actually runs on), a
// method's IL type tokens get resolved when that method is first JIT-compiled, not
// lazily per-branch. A Config.Bind<EPersonality>(...) call sitting directly inside
// Awake() would try to resolve SAIN's assembly the moment Awake() itself runs,
// regardless of any "if SAIN is loaded" check wrapped around it, and break BD when
// SAIN - a soft dependency - isn't installed. Every other SAIN-touching class in this
// mod already lives in its own file for the same reason (see
// BDSteeringHandoffDiagnostic.cs, BDUnderFireSteeringFallback.cs,
// BDPersonalityOverride.cs); this one just holds the config entries those patches read.
internal static class BDPersonalityConfig
{
    public static ConfigEntry<EPersonality> BdPersonality;
    public static ConfigEntry<EPersonality> WedgePersonality;

    // Call only from behind a "SAIN is loaded" check - see the note above.
    public static void Bind(ConfigFile config)
    {
        BdPersonality = config.Bind(
            "SAIN Personality",
            "Black Division Personality",
            EPersonality.GigaChad,
            "SAIN personality forced on Black Division's five non-Wedge roles (Lead, Assault, "
                + "Breacher, Support, Raider). Ignored if this personality's own 'Personality Enabled' "
                + "is off in your SAIN preset - BD falls back to whatever SAIN would have assigned instead."
        );

        WedgePersonality = config.Bind(
            "SAIN Personality",
            "Wedge Personality",
            EPersonality.GigaChad,
            "SAIN personality forced on Black Division's Wedge boss specifically. Same "
                + "'Personality Enabled' opt-out as the setting above."
        );
    }
}
