using System.Collections.Generic;
using RainRim.LizardTongueGrapple;
using RainUtils.Utils;
using Verse;

namespace RainRim.Debug;

// ReSharper disable InconsistentNaming

public class ThingComp_DebugStemDrawerCompGiver : ThingComp
{
    private ThingWithComps _owner;
    
    public override void Initialize(CompProperties properties)
    {
        props = properties;
    }

    // Called when a pawn equips a piece of equipment with this comp
    public override void Notify_Equipped(Pawn pawn)
    {
        pawn.AddThingComp<ThingComp_TongueStemDrawer>(new CompProperties_TongueStemDrawer { graphicData =
            ((CompProperties_DebugStemDrawerCompGiver)props).graphicData });
        _owner = pawn;
    }

    // Called when a pawn unequips a piece of equipment with this comp
    public override void Notify_Unequipped(Pawn pawn)
    {
        pawn.RemoveThingComp<ThingComp_TongueStemDrawer>();
        _owner = null;
    }

    public override void PostExposeData()
    {
        base.PostExposeData();
        
        Scribe_References.Look(ref _owner, nameof(_owner));

        if (Scribe.mode != LoadSaveMode.PostLoadInit) return;
        if (_owner != null && _owner.GetComp<ThingComp_TongueStemDrawer>() == null)
            _owner.AddThingComp<ThingComp_TongueStemDrawer>(new CompProperties_TongueStemDrawer { graphicData =
                ((CompProperties_DebugStemDrawerCompGiver)props).graphicData });
    }
}

public class CompProperties_DebugStemDrawerCompGiver : CompProperties
{
    public GraphicData graphicData;
    
    public CompProperties_DebugStemDrawerCompGiver()
    {
        compClass = typeof(ThingComp_DebugStemDrawerCompGiver);
    }
    
    public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
    {
        foreach(var error in base.ConfigErrors(parentDef)) yield return error;

        if (graphicData == null) yield return parentDef.defName + " has DebugStemDrawerCompGiver comp with no " +
                                              "GraphicData";
    }
}