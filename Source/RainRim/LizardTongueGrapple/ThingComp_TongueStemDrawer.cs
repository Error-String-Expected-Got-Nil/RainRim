using System.Collections.Generic;
using RainRim.Utils;
using UnityEngine;
using Verse;

// ReSharper disable InconsistentNaming

namespace RainRim.LizardTongueGrapple;

// This ThingComp is attached to a pawn that uses a tongue grapple attack. Setting the StemAnchor will make it draw the
// stem between the parent of this ThingComp and the StemAnchor, if both are present.
public class ThingComp_TongueStemDrawer : ThingComp
{
    public Graphic StemGraphic;
    public Thing StemAnchor;

    public Vector3? RootPosition
        => parent.DrawPosHeld is { } drawPos
            ? drawPos + (
                parent is Pawn parentPawn 
                    ? EffectUtils.GetHeadOffset(parentPawn)
                    : new Vector3(0f, 0f, 0f)
                )
            : null;

    public override void Initialize(CompProperties properties)
    {
        props = properties;

        StemGraphic = ((CompProperties_TongueStemDrawer)props).graphicData.Graphic;
    }

    // Note: PostDraw() is only called for things with realtime draw mode. As far as that's relevant for my purposes,
    // that means this has to be attached to a pawn to work properly.
    public override void PostDraw()
    {
        if (StemAnchor == null) return;
        
        var rootPosMaybe = RootPosition;
        var anchorPosMaybe = StemAnchor.DrawPosHeld;

        if (rootPosMaybe is null || anchorPosMaybe is null) return;

        var rootPos = (Vector3)rootPosMaybe;
        var anchorPos = (Vector3)anchorPosMaybe;
        
        var relativePos = (anchorPos - rootPos).Yto0();

        StemGraphic.drawSize = new Vector2 { x = StemGraphic.drawSize.x, y = relativePos.magnitude };

        var drawPos = rootPos + relativePos * 0.5f;
        drawPos.y = Mathf.Min(parent.def.Altitude, StemAnchor.def.Altitude) - Altitudes.AltInc;
        
        StemGraphic.DrawWorker(drawPos, Rot4.North, null, null, relativePos.AngleFlat());
    }

    public override void PostExposeData()
    {
        Scribe_References.Look(ref StemAnchor, nameof(StemAnchor));
    }
}

public class CompProperties_TongueStemDrawer : CompProperties
{
    public GraphicData graphicData;
    
    public CompProperties_TongueStemDrawer()
    {
        compClass = typeof(ThingComp_TongueStemDrawer);
    }

    public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
    {
        foreach(var error in base.ConfigErrors(parentDef)) yield return error;

        if (graphicData == null) yield return parentDef.defName + " has TongueStemDrawer comp with no GraphicData";
    }
}