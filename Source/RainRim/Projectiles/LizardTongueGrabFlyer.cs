﻿using UnityEngine;
using Verse;
using RimWorld;

// TODO: Still valid for 1.4, but 1.5 seems to use a new system for PawnFlyers. Might need a wholly new class for that.

namespace RainRim.Projectiles
{
    public class LizardTongueGrabFlyer : PawnFlyer
    {
        private Vector3 _currentPosition;
        private int _lastCalculatedPositionTick = -1;

        private void CalculatePosition() 
        {
            if (_lastCalculatedPositionTick == ticksFlying)
                return;
            _lastCalculatedPositionTick = ticksFlying;
            var timeFraction = (float) ticksFlying / ticksFlightTime;
            _currentPosition = Vector3.Lerp(startVec, DestinationPos, timeFraction);
        }

        public override Vector3 DrawPos
        {
            get
            {
                CalculatePosition();
                return _currentPosition;
            }
        }

        public override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            CalculatePosition();
            FlyingPawn.DrawAt(_currentPosition, flip);
            if (CarriedThing == null)
                return;
            PawnRenderer.DrawCarriedThing(FlyingPawn, _currentPosition, CarriedThing);
        }
    }
}
