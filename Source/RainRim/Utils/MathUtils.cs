using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RainRim.Utils;

public static class MathUtils
{
    // Default head offsets for animals if their RaceProps don't define them explicitly
    private static readonly List<Vector3> GenericAnimalHeadOffsets = new()
    {
        new Vector3(0f, 0f, 0.4f),
        new Vector3(0.4f, 0f, 0.25f),
        new Vector3(0f, 0f, 0.1f),
        new Vector3(-0.4f, 0f, 0.25f)
    };
    
    // Takes a vector, returning it scaled and rotated based on a pawn's graphic data and body. If the initial vector
    // represents an offset from the center of the pawn's base sprite, the returned vector will be the same point on
    // the pawn's drawn sprite.
    // May not be entirely accurate for humanlike pawns.
    public static Vector3 TransformVectorByPawn(Vector3 vec, Pawn pawn)
    {
        if (pawn.RaceProps.Humanlike)
            return vec.RotatedBy(pawn.Drawer.renderer.BodyAngle(PawnRenderFlags.None));
        
        var scaling = pawn.ageTracker.CurKindLifeStage.bodyGraphicData.drawSize
                      * pawn.ageTracker.CurLifeStage.bodySizeFactor;
        return new Vector3(vec.x * scaling.x, vec.y, vec.z * scaling.y)
            .RotatedBy(pawn.Drawer.renderer.BodyAngle(PawnRenderFlags.None));
    }

    // Gets the base offset relative to the center of a pawn's sprite that represents the location of their head.
    public static Vector3 GetBaseHeadOffset(Pawn pawn) 
        => pawn.RaceProps.Humanlike 
            ? pawn.Drawer.renderer.BaseHeadOffsetAt(pawn.Rotation) 
            : pawn.RaceProps.headPosPerRotation.NullOrEmpty() 
                ? GenericAnimalHeadOffsets[pawn.Rotation.AsInt] 
                : pawn.RaceProps.headPosPerRotation[pawn.Rotation.AsInt];
}