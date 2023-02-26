using UnityEngine;
using Verse;
using RimWorld;

namespace RainRim
{
    public class LizardTongueProjectile : Projectile
    {
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            base.Impact(hitThing);

            BattleLogEntry_RangedImpact logEntry = new BattleLogEntry_RangedImpact(launcher, hitThing, intendedTarget.Thing, ThingDef.Named("Gun_Autopistol"), def, targetCoverDef);
            Find.BattleLog.Add(logEntry);

            Pawn lizard = launcher as Pawn;

            if (hitThing != null)
            {
                Pawn target = hitThing as Pawn;
                if (target != null)
                {
                    if (!ShouldGrabTarget(lizard, target))
                        return;

                    Map map = target.Map;
                    bool targetWasSelected = Find.Selector.IsSelected(target);

                    IntVec3 destinationPosition = GetDesitinationPosition(lizard.Position, target.Position, map);

                    PawnFlyer pawnFlyer = PawnFlyer.MakeFlyer(RW_ThingDefOf.RW_LizardTongueGrabFlyer, target, destinationPosition, null, null, false);
                    if (pawnFlyer == null)
                        return;
                    GenSpawn.Spawn(pawnFlyer, destinationPosition, map);
                    if (targetWasSelected)
                        Find.Selector.Select(target, false, false);
                }
            }
            else
            {

            }
        }

        // possibly excessive, but I may want to add extra conditions for if the grab should succeed in the future, so I'm putting it in a method
        private bool ShouldGrabTarget(Pawn lizard, Pawn target)
        {
            if (lizard.BodySize < target.BodySize)
                return false;
            return true;
        }

        // this gets the space adjacent to the lizard that's between it and the target
        // pulling the target to the lizard's space is fine, but this looks much nicer
        private IntVec3 GetDesitinationPosition(IntVec3 origin, IntVec3 target, Map map)
        {
            Vector3 relativePosition = (target - origin).ToVector3();
            relativePosition.Normalize();
            IntVec3 roundedRelativePosition = new IntVec3((int)Mathf.Round(relativePosition.x), 0, (int)Mathf.Round(relativePosition.z));
            IntVec3 candidatePosition = origin + roundedRelativePosition;

            // while an adjacent space between the lizard and target is PROBABLY fine, there's no guarantee, so we need to check here
            if (!IsValidPositionTarget(map, candidatePosition))
                // given the lizard is already standing there, its spot will always be safe, so we can use it as a fallback
                return origin;
            return candidatePosition;
        }

        private bool IsValidPositionTarget(Map map, IntVec3 cell)
        {
            if (!cell.IsValid || !cell.InBounds(map) || cell.Impassable(map) || !cell.Walkable(map))
                return false;
            Building edifice = cell.GetEdifice(map);
            return edifice == null || !(edifice is Building_Door buildingDoor) || buildingDoor.Open;
        }
    }
}
