using RainRim.Utils;
using UnityEngine;
using Verse;

namespace RainRim.CreatureCosmetics;

// ReSharper disable InconsistentNaming

public class PawnRenderNodeWorker_CreatureCosmetics : PawnRenderNodeWorker
{
    protected override Graphic GetGraphic(PawnRenderNode node, PawnDrawParms parms) => node.GraphicFor(parms.pawn);
    
    public override MaterialPropertyBlock GetMaterialPropertyBlock(PawnRenderNode node, Material material,
        PawnDrawParms parms)
    {
        if (GetGraphic(node, parms) == null) return null;

        if (node is not PawnRenderNode_CreatureCosmetics ccnode) return null;
        
        var matPropBlock = ccnode.MatPropBlock;

        Color color;

        if (RW_Mod.Settings.RainbowMode && parms.pawn.GetComp<ThingComp_RandomColorPicker>() is { } colorComp)
        {
            color = colorComp.RainbowColor;
        }
        else
        {
            color = ccnode.ColorFor(parms.pawn);
            color.a *= ccnode.OpacityFactor;
            color = Color.Lerp(color, Color.white, ccnode.WhiteFlashFactor);   
        }
        
        matPropBlock.SetColor(ShaderPropertyIDs.Color, color);
        
        return matPropBlock;
    }
}