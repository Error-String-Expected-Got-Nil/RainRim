using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RainUtils.LocalArmor;
using Verse;

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
    
    public override void Initialize(CompProperties properties)
    {
        props = properties;
        
        _nodesByTag = (Dictionary<PawnRenderNodeTagDef, PawnRenderNode>)PawnRenderTree_NodesByTag_Info
            .GetValue(ParentPawn.Drawer.renderer.renderTree);
    }

    public override void CompTick()
    {
        if (WhiteFlashAnimator == null) { }
        else if (WhiteFlashAnimator.Finished)
            WhiteFlashAnimator = null;
        else
        {
            WhiteFlashAnimator.Tick();
            _graphicsUpToDate = false;
        }
    }

    public override void PostDraw()
    {
        if (_graphicsUpToDate) return;
        
        var headNode = HeadNode;
        if (headNode == null) return;
        
        _graphicsUpToDate = true;

        if (WhiteFlashAnimator != null) headNode.WhiteFlashFactor = WhiteFlashAnimator.Peek();
    }
    
    public void LocalArmorCallback(float preArmorDamage, float postArmorDamage, float armorPen, DamageDef preArmorDef,
        DamageDef postArmorDef, Pawn pawn, bool metalArmor, BodyPartRecord part)
    {
        
    }

    public override void PostExposeData()
    {
        base.PostExposeData();

        Scribe_Deep.Look(ref WhiteFlashAnimator, nameof(WhiteFlashAnimator));
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