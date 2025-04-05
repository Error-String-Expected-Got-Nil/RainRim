using System.Collections.Generic;
using RainRim.Utils;
using Verse;

namespace RainRim.Debug;

// ReSharper disable InconsistentNaming

// Debug comp to test that the TransformVectorByPawn and GetBaseHeadOffset functions worked correctly
public class ThingComp_RenderOnHead : ThingComp
{
    public Graphic RenderGraphic;

    public override void Initialize(CompProperties properties)
    {
        props = properties;

        RenderGraphic = ((CompProperties_RenderOnHead)props).graphicData.Graphic;
    }

    public override void PostDraw()
    {
        if (parent is not Pawn pawn || parent.DrawPosHeld is not { } drawPos) return;
        var headOffset = EffectUtils.GetHeadOffset(pawn);
        
        RenderGraphic.DrawWorker((drawPos + headOffset).WithY(pawn.def.Altitude + Altitudes.AltInc), Rot4.North,
            null, null, 0f);
    }
}

public class CompProperties_RenderOnHead : CompProperties
{
    public GraphicData graphicData;

    public CompProperties_RenderOnHead()
    {
        compClass = typeof(ThingComp_RenderOnHead);
    }
    
    public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
    {
        foreach(var error in base.ConfigErrors(parentDef)) yield return error;

        if (graphicData == null) yield return parentDef.defName + " has RenderOnHead comp with no GraphicData";
    }
}