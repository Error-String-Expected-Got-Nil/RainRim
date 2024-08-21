using UnityEngine;
using Verse;

namespace RainRim.LizardTongueGrapple;

// This class is just so the dummy Thing can be drawn rotation relative to a certain anchor-point Thing
public class TongueTipDummy : Thing
{
    public Thing RotationAnchor;

    protected override void DrawAt(Vector3 drawLoc, bool flipped = false)
    {
        // This is unnecessarily complex in terms of handling edge-cases. But I like writing absurd single statements
        // so it's staying like this.
        Graphic.Draw(drawLoc, Rotation, this, 
            DrawPosHeld is { } thisDrawPos 
                ? RotationAnchor?.TryGetComp<ThingComp_TongueStemDrawer>()?.RootPosition is { } rootDrawPos 
                    ? (thisDrawPos - rootDrawPos).AngleFlat()
                    : RotationAnchor?.DrawPosHeld is { } anchorDrawPos 
                        ? (thisDrawPos - anchorDrawPos).AngleFlat()
                        : 0f
                : 0f
            );
    }

    public override void ExposeData()
    {
        base.ExposeData();
        
        Scribe_References.Look(ref RotationAnchor, nameof(RotationAnchor));
    }
}