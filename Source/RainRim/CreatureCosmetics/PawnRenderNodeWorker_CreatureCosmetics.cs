using UnityEngine;
using Verse;

namespace RainRim.CreatureCosmetics;

// ReSharper disable InconsistentNaming

public class PawnRenderNodeWorker_CreatureCosmetics : PawnRenderNodeWorker
{
    public override MaterialPropertyBlock GetMaterialPropertyBlock(PawnRenderNode node, Material material,
        PawnDrawParms parms)
    {
        if (GetGraphic(node, parms) == null) return null;

        var matPropBlock = node.MatPropBlock;
        var color = material.color;
        color.a *= ((PawnRenderNode_CreatureCosmetics)node).OpacityFactor;
        matPropBlock.SetColor(ShaderPropertyIDs.Color, color);
        
        return matPropBlock;
    }
}