using Verse;

namespace RainRim.LizardTongueGrapple
{
    public class TongueGrapplePawnFlyerWorker : PawnFlyerWorker
    {
        public TongueGrapplePawnFlyerWorker(PawnFlyerProperties properties) : base(properties) {}

        public override float GetHeight(float t) => 0f;
    }
}