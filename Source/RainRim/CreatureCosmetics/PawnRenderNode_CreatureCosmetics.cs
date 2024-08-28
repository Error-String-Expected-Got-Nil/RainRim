using UnityEngine;
using Verse;

namespace RainRim.CreatureCosmetics;

// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnassignedField.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable ConvertToConstant.Global
// ReSharper disable MemberCanBePrivate.Global

public class PawnRenderNode_CreatureCosmetics : PawnRenderNode
{
    public PawnRenderNodeProperties_CreatureCosmetics CastProps => (PawnRenderNodeProperties_CreatureCosmetics)props;
    
    public Graphic CosmeticGraphic;
    public bool Colorize;
    public float OpacityFactor = 1f;

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
        }
        else
            Log.Error("[RainRim] - Pawn " + pawn.ToStringSafe() + " did not have CreatureCosmeticsGraphics def " +
                      "extension in its PawnKindDef, failed to initialize render node");
    }

    public override Graphic GraphicFor(Pawn pawn) 
        => GraphicDatabase.Get<Graphic_Multi>(CosmeticGraphic.path, ShaderDatabase.CutoutComplex,
            pawn.ageTracker.CurKindLifeStage.bodyGraphicData.drawSize, ColorFor(pawn), Color.white,
            CosmeticGraphic.data, CosmeticGraphic.maskPath);
    
    public override Color ColorFor(Pawn pawn) 
        => !Colorize || pawn.GetComp<ThingComp_RandomColorPicker>() is not { } colorComp 
            ? Color.white 
            : colorComp.Color;
}

public class PawnRenderNodeProperties_CreatureCosmetics : PawnRenderNodeProperties
{
    public string graphicKey;
    public bool colorize = false;
    
    public PawnRenderNodeProperties_CreatureCosmetics()
    {
        nodeClass = typeof(PawnRenderNode_CreatureCosmetics);
    }
}