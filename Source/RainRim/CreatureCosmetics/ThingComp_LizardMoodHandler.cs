using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RainUtils.Cosmetics;
using RainUtils.LocalArmor;
using Verse;
using Verse.Sound;

namespace RainRim.CreatureCosmetics;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnassignedField.Global

public class ThingComp_LizardMoodHandler : ThingComp, ILocalArmorCallback
{
    private static readonly FieldInfo PawnRenderTree_NodesByTag_Info 
        = AccessTools.Field(typeof(PawnRenderTree), "nodesByTag");

    private bool _graphicsUpToDate = false;
    private Dictionary<PawnRenderNodeTagDef, PawnRenderNode> _nodesByTag;
    
    private PawnRenderNode_CreatureCosmetics HeadNode => (PawnRenderNode_CreatureCosmetics)_nodesByTag
        .TryGetValue(RW_Common.RW_PawnRenderNodeTagDefOf.RW_LizardHead);
    
    public CompProperties_LizardMoodHandler Props => (CompProperties_LizardMoodHandler)props;
    public Pawn ParentPawn => (Pawn)parent;

    public FlashAnimator WhiteFlashAnimator;
    public FlashAnimator ColorFlashAnimator;
    
    public override void Initialize(CompProperties properties)
    {
        props = properties;
        
        _nodesByTag = (Dictionary<PawnRenderNodeTagDef, PawnRenderNode>)PawnRenderTree_NodesByTag_Info
            .GetValue(ParentPawn.Drawer.renderer.renderTree);
    }

    public override void CompTick()
    {
        if (WhiteFlashAnimator != null)
        {
            WhiteFlashAnimator.Tick();

            if (WhiteFlashAnimator.Finished)
                WhiteFlashAnimator = null;
                
            _graphicsUpToDate = false;
        }
        
        if (ColorFlashAnimator == null) { }
        else
        {
            ColorFlashAnimator.Tick();

            if (ColorFlashAnimator.Finished)
                ColorFlashAnimator = null;
            
            _graphicsUpToDate = false;
        }
    }

    public override void PostDraw()
    {
        if (_graphicsUpToDate) return;
        
        var headNode = HeadNode;
        if (headNode == null) return;
        
        _graphicsUpToDate = true;

        headNode.WhiteFlashFactor = WhiteFlashAnimator?.Peek() ?? 0f;
        headNode.OpacityFactor = ColorFlashAnimator?.Peek() ?? 1f;
    }

    private void InterruptCurrentAnimation()
    {
        WhiteFlashAnimator = null;
        ColorFlashAnimator = null;
        _graphicsUpToDate = false;
    }
    
    public void LocalArmorCallback(float preArmorDamage, float postArmorDamage, float armorPen, DamageDef preArmorDef,
        DamageDef postArmorDef, Pawn pawn, bool metalArmor, BodyPartRecord part)
    {
        if (!(postArmorDamage < preArmorDamage)) return;
        
        InterruptCurrentAnimation();
        WhiteFlashAnimator = RW_Common.RW_FlashAnimationDefOf.RW_Flash_Lizard_HeadArmorAbsorb_White.GetAnimator();
        ColorFlashAnimator = RW_Common.RW_FlashAnimationDefOf.RW_Flash_Lizard_HeadArmorAbsorb_Color.GetAnimator();
        RW_Common.RW_SoundDefOf.RW_LizardHeadDeflectAttack.PlayOneShot(ParentPawn);
    }

    public override void PostExposeData()
    {
        base.PostExposeData();

        Scribe_Deep.Look(ref WhiteFlashAnimator, nameof(WhiteFlashAnimator));
        Scribe_Deep.Look(ref ColorFlashAnimator, nameof(ColorFlashAnimator));
    }
}

public class CompProperties_LizardMoodHandler : CompProperties
{
    public CompProperties_LizardMoodHandler()
    {
        compClass = typeof(ThingComp_LizardMoodHandler);
    }

    public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
    {
        foreach (var error in base.ConfigErrors(parentDef)) yield return error;

        if (parentDef.thingClass != typeof(Pawn)) 
            yield return parentDef.defName + " has LizardMoodHandler but is not a Pawn";
    }
}

public enum LizardMood
{
    Idle,
    Aggressive,
    Scared
}