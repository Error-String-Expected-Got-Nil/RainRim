using RimWorld;
using Verse;

// ReSharper disable InconsistentNaming
// ReSharper disable UnassignedField.Global

namespace RainRim;

public static class RW_Common
{
    [DefOf]
    public static class RW_ThingDefOf
    {
        static RW_ThingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RW_ThingDefOf));
        }

        public static ThingDef RW_TongueTipDummy;
        public static ThingDef RW_LizardTongueGrappleFlyer;
        public static ThingDef RW_LizardTongueRetractFlyer;
    }

    [DefOf]
    public static class RW_HediffDefOf
    {
        static RW_HediffDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RW_HediffDefOf));
        }

        public static HediffDef RW_LizardSpitHediff;
    }
}