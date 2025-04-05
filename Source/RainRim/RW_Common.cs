using RainUtils.Cosmetics;
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

    [DefOf]
    public static class RW_SoundDefOf
    {
        static RW_SoundDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RW_SoundDefOf));
        }

        public static SoundDef RW_LizardSpit;
        public static SoundDef RW_LizardSpitSplatter;
        public static SoundDef RW_LizardTongueAttach;
        public static SoundDef RW_LizardTongueDetach;

        public static SoundDef RW_LizardHeadDeflectAttack;
    }

    [DefOf]
    public static class RW_PawnRenderNodeTagDefOf
    {
        static RW_PawnRenderNodeTagDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RW_PawnRenderNodeTagDefOf));
        }

        public static PawnRenderNodeTagDef RW_LizardHead;
    }

    [DefOf]
    public static class RW_FlashAnimationDefOf
    {
        static RW_FlashAnimationDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RW_FlashAnimationDefOf));
        }

        public static FlashAnimationDef RW_Flash_Lizard_HeadArmorAbsorb_White;
        public static FlashAnimationDef RW_Flash_Lizard_HeadArmorAbsorb_Color;
    }

    [DefOf]
    public static class RW_EffecterDefOf
    {
        static RW_EffecterDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RW_EffecterDefOf));
        }

        public static EffecterDef RW_LizardBubblesNorth;
        public static EffecterDef RW_LizardBubblesEast;
        public static EffecterDef RW_LizardBubblesSouth;
        public static EffecterDef RW_LizardBubblesWest;
    }
}