using UnityEngine;
using Verse;

namespace RainRim.LizardTongueGrapple;

// This class is just so the dummy Thing can be drawn with arbitrary rotation.
public class TongueTipDummy : Thing
{
    public float RotationAngle;

    protected override void DrawAt(Vector3 drawLoc, bool flipped = false)
    {
        Graphic.Draw(drawLoc, Rotation, this, RotationAngle);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        
        Scribe_Values.Look(ref RotationAngle, nameof(RotationAngle));
    }
}