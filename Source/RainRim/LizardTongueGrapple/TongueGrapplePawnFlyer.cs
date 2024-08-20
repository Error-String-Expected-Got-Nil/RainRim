using RimWorld;
using Verse;

namespace RainRim.LizardTongueGrapple;

public class TongueGrapplePawnFlyer : PawnFlyer
{
    public ThingWithComps StemRoot;
    
    protected override void RespawnPawn()
    {
        base.RespawnPawn();

        if (StemRoot?.GetComp<ThingComp_TongueStemDrawer>() is { } comp)
            comp.StemAnchor = null;
    }

    public override void ExposeData()
    {
        base.ExposeData();
        
        Scribe_References.Look(ref StemRoot, nameof(StemRoot));
    }
}