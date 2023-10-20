using UnityEngine;
using Verse;
using RimWorld;

namespace RainRim.Projectiles
{
    public static class LizardTongueGrabUtilities
    {
        // Possibly excessive, but I may want to add extra conditions for if the grab should succeed in the future,
        // so I'm putting it in a method
        public static bool ShouldGrabTarget(Pawn lizard, Pawn target)
        {
            if (lizard.BodySize < target.BodySize) return false;
            return true;
        }

        /// <summary>
        /// Gets the space adjacent to the origin that's between the origin and the target.
        /// </summary>
        public static IntVec3 GetDestinationPosition(IntVec3 origin, IntVec3 target, Map map)
        {
            var relativePosition = (target - origin).ToVector3();
            relativePosition.Normalize();
            var roundedRelativePosition = new IntVec3((int)Mathf.Round(relativePosition.x), 0, (int)Mathf.Round(relativePosition.z));
            var candidatePosition = origin + roundedRelativePosition;

            // While an adjacent space between the lizard and target is probably open, there's no guarantee, so we
            // need to check here.
            if (!IsValidPositionTarget(map, candidatePosition))
                // Given the lizard is already standing there, its spot will always be safe, so we can use it as a
                // fallback.
                return origin;
            return candidatePosition;
        }

        public static bool IsValidPositionTarget(Map map, IntVec3 cell)
        {
            if (!cell.IsValid || !cell.InBounds(map) || cell.Impassable(map) || !cell.Walkable(map))
                return false;
            var edifice = cell.GetEdifice(map);
            return edifice == null || !(edifice is Building_Door buildingDoor) || buildingDoor.Open;
        }
    }
}
