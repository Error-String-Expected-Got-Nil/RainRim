using UnityEngine;
using Verse;
using RimWorld;

namespace RainRim
{
    public class LizardTongueGrabFlyer : PawnFlyer
    {
        private Vector3 currentPosition;
        private int lastCalculatedPositionTick = -1;

        private void CalculatePosition() 
        {
            if (lastCalculatedPositionTick == ticksFlying)
                return;
            lastCalculatedPositionTick = ticksFlying;
            float timeFraction = (float) ticksFlying / ticksFlightTime;
            currentPosition = Vector3.Lerp(startVec, DestinationPos, timeFraction);
        }

        public override Vector3 DrawPos
        {
            get
            {
                CalculatePosition();
                return currentPosition;
            }
        }

        public override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            CalculatePosition();
            FlyingPawn.DrawAt(currentPosition, flip);
            if (CarriedThing == null)
                return;
            PawnRenderer.DrawCarriedThing(FlyingPawn, currentPosition, CarriedThing);
        }
    }
}
