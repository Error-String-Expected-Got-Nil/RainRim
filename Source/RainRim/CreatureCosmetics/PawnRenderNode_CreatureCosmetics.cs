﻿using UnityEngine;
using Verse;

namespace RainRim.CreatureCosmetics;

// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnassignedField.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable ConvertToConstant.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable VirtualMemberCallInConstructor

public class PawnRenderNode_CreatureCosmetics : PawnRenderNode
{
    public PawnRenderNodeProperties_CreatureCosmetics CastProps => (PawnRenderNodeProperties_CreatureCosmetics)props;
    
    public Graphic CosmeticGraphic;
    public bool Colorize;
    public float OpacityFactor = 1f;
    public float WhiteFlashFactor = 0f;

    public PawnRenderNode_CreatureCosmetics(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
        : base(pawn, props, tree)
    {
        Colorize = CastProps.colorize;

        if (pawn.kindDef.GetModExtension<ModExtension_CreatureCosmeticsGraphics>() is { } cosmetics)
        {
            if (!cosmetics.cosmeticGraphics.TryGetValue(CastProps.graphicKey, out var graphicData))
            {
                Log.Error("[RainRim] - Failed to get CreatureCosmetic graphic data for pawn "
                          + pawn.ToStringSafe());
                return;
            }

            CosmeticGraphic = graphicData.Graphic;
            // Mesh relies on the cosmetic graphic which is not set until this point, so we cannot use how it is set
            // in the base constructor, and must set it again here after the cosmetic graphic has been established.
            meshSet = MeshSetFor(pawn);
        }
        else
            Log.Error("[RainRim] - Pawn " + pawn.ToStringSafe() + " did not have CreatureCosmeticsGraphics def " +
                      "extension in its PawnKindDef, failed to initialize render node");
    }

    public override GraphicMeshSet MeshSetFor(Pawn pawn)
    {
        if (GraphicFor(pawn) is not { } pawnGraphic) return null;
        
        if (!CastProps.rectangular)
            return MeshPool.GetMeshSetForSize(pawnGraphic.drawSize.x, pawnGraphic.drawSize.y);
            
        var horizMeshSet = MeshPool.GetMeshSetForSize(pawnGraphic.drawSize.x, pawnGraphic.drawSize.y);
        var vertMeshSet = MeshPool.GetMeshSetForSize(pawnGraphic.drawSize.y, pawnGraphic.drawSize.x);

        return new GraphicMeshSet(vertMeshSet.MeshAt(Rot4.North), horizMeshSet.MeshAt(Rot4.East), 
            vertMeshSet.MeshAt(Rot4.South), horizMeshSet.MeshAt(Rot4.West));
    }

    public override Graphic GraphicFor(Pawn pawn) 
        => CosmeticGraphic != null 
            ? GraphicDatabase.Get<Graphic_Multi>(CosmeticGraphic.path, CosmeticGraphic.Shader,
            pawn.ageTracker.CurKindLifeStage.bodyGraphicData.drawSize, Color.white, Color.white,
            CosmeticGraphic.data, CosmeticGraphic.maskPath) 
            : null;
    
    public override Color ColorFor(Pawn pawn) 
        => !Colorize || pawn.GetComp<ThingComp_RandomColorPicker>() is not { } colorComp 
            ? Color.white 
            : colorComp.Color;
}

public class PawnRenderNodeProperties_CreatureCosmetics : PawnRenderNodeProperties
{
    public string graphicKey;
    public bool colorize = false;
    public bool rectangular = false;
    
    public PawnRenderNodeProperties_CreatureCosmetics()
    {
        nodeClass = typeof(PawnRenderNode_CreatureCosmetics);
    }
}