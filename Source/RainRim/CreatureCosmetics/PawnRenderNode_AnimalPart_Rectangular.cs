using Verse;

namespace RainRim.CreatureCosmetics;

// ReSharper disable InconsistentNaming

public class PawnRenderNode_AnimalPart_Rectangular : PawnRenderNode_AnimalPart
{
    public PawnRenderNode_AnimalPart_Rectangular(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
        : base(pawn, props, tree) { }

    public override GraphicMeshSet MeshSetFor(Pawn pawn)
    {
        var pawnGraphic = GraphicFor(pawn);
        if (pawnGraphic == null) return null;

        var horizMeshSet = MeshPool.GetMeshSetForSize(pawnGraphic.drawSize.x, pawnGraphic.drawSize.y);
        var vertMeshSet = MeshPool.GetMeshSetForSize(pawnGraphic.drawSize.y, pawnGraphic.drawSize.x);

        return new GraphicMeshSet(vertMeshSet.MeshAt(Rot4.North), horizMeshSet.MeshAt(Rot4.East),
            vertMeshSet.MeshAt(Rot4.South), horizMeshSet.MeshAt(Rot4.West));
    }
}