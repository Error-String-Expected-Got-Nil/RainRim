using System.Collections;
using System.Collections.Generic;
using Verse;

namespace RainRim.CreatureCosmetics;

// ReSharper disable InconsistentNaming

public class PawnRenderNode_CreatureCosmetics : PawnRenderNode
{
    public Graphic CosmeticGraphic;
    
    public PawnRenderNode_CreatureCosmetics(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
        : base(pawn, props, tree)
    {
        if (pawn.kindDef.GetModExtension<ModExtension_CreatureCosmeticGraphics>() is { } cosmetics)
        {
            
        }
    }
    
    
}

public class PawnRenderNodeProperties_CreatureCosmetics : PawnRenderNodeProperties
{
    public string graphicKey;
    
    public PawnRenderNodeProperties_CreatureCosmetics()
    {
        nodeClass = typeof(PawnRenderNode_CreatureCosmetics);
    }
}