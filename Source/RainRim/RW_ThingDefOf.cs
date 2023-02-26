using Verse;
using RimWorld;

namespace RainRim
{
    [DefOf]
    public static class RW_ThingDefOf
    {
        static RW_ThingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
        }

        public static ThingDef RW_LizardTongueGrabFlyer;
    }
}
