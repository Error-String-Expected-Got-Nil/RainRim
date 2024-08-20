using HarmonyLib;
using RimWorld;

namespace RainRim.Patches;

// ReSharper disable InconsistentNaming

[HarmonyPatch(typeof(PawnFlyer))]
public class Patch_PawnFlyer
{
    public static bool DisableLandingEffects;
    
    [HarmonyPatch("LandingEffects")]
    [HarmonyPrefix]
    public static bool Patch_LandingEffects()
    {
        return !DisableLandingEffects;
    }
}