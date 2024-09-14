using RainRim.Patches;
using RimWorld;
using UnityEngine;
using Verse;

namespace RainRim.LizardTongueGrapple;

// Only for the case that the tongue missed and retracted empty. Uses a dummy Thing to show the tongue tip graphic, and
// destroys it when the flyer lands.
public class TongueRetractPawnFlyer : PawnFlyer
{
    public ThingWithComps StemRoot;
    public bool DestroyOnLand = true;

    protected override void RespawnPawn()
    {
        var flyingThing = FlyingThing;

        Patch_PawnFlyer.DisableLandingEffects = true;
        base.RespawnPawn();
        Patch_PawnFlyer.DisableLandingEffects = false;
        
        if (StemRoot?.GetComp<ThingComp_TongueStemDrawer>() is { } comp)
            comp.StemAnchor = null;
        
        if (DestroyOnLand) flyingThing.Destroy();
    }

    // Empty override to disable shadow drawing
    protected override void DrawAt(Vector3 drawLoc, bool flip = false) {}
    
    public override void ExposeData()
    {
        base.ExposeData();
        
        Scribe_References.Look(ref StemRoot, nameof(StemRoot));
        Scribe_Values.Look(ref DestroyOnLand, nameof(DestroyOnLand));
    }
}