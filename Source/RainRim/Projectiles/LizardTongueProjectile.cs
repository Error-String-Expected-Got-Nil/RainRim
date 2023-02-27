using Verse;
using RimWorld;
using static RainRim.LizardTongueGrabUtilities;

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
    }
}
