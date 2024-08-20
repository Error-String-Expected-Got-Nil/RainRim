using UnityEngine;
using Verse;

namespace RainRim.Utils;

public static class MathUtils
{
    // Takes a vector, returning it scaled and rotated based on a pawn's graphic data and body. If the initial vector
    // represents an offset from the center of the pawn's base sprite, the returned vector will be the same point on
    // the pawn's drawn sprite.
    public static Vector3 TransformVectorByPawn(Vector3 vec, Pawn pawn)
    {
        var scaling = pawn.ageTracker.CurKindLifeStage.bodyGraphicData.drawSize 
                      * pawn.ageTracker.CurLifeStage.bodySizeFactor;
        return new Vector3(vec.x * scaling.x, vec.y, vec.z * scaling.y)
            .RotatedBy(pawn.Drawer.renderer.BodyAngle(PawnRenderFlags.None));
    }
}