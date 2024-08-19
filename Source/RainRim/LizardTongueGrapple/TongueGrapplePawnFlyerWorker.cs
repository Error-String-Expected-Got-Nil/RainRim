using Verse;

namespace RainRim.LizardTongueGrapple;

public class TongueGrapplePawnFlyerWorker : PawnFlyerWorker
{
    public TongueGrapplePawnFlyerWorker(PawnFlyerProperties properties) : base(properties) {}

    // y = 0 at x = 0; y = 0.25 at x = 0.5; y = 1 at x = 1
    public override float AdjustedProgress(float t) => 1.0f - GenMath.InverseParabola((t + 1.0f) * 0.5f);
        
    public override float GetHeight(float t) => 0f;
}