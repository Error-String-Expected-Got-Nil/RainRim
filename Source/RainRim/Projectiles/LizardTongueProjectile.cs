using Verse;
using RimWorld;
using static RainRim.Projectiles.LizardTongueGrabUtilities;

namespace RainRim.Projectiles
{
    public class LizardTongueProjectile : Projectile
    {
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            base.Impact(hitThing, blockedByShield);

            var logEntry = new BattleLogEntry_RangedImpact(launcher, hitThing, intendedTarget.Thing,
                ThingDef.Named("Gun_Autopistol"), def, targetCoverDef);
            Find.BattleLog.Add(logEntry);

            var lizard = launcher as Pawn;

            if (hitThing != null)
            {
                var target = hitThing as Pawn;
                if (target != null)
                {
                    if (!ShouldGrabTarget(lizard, target))
                        return;

                    var map = target.Map;
                    var targetWasSelected = Find.Selector.IsSelected(target);

                    var destinationPosition = GetDestinationPosition(lizard.Position, target.Position, map);

                    var pawnFlyer = PawnFlyer.MakeFlyer(RW_ThingDefOf.RW_LizardTongueGrabFlyer, target, destinationPosition, null, null, false);
                    if (pawnFlyer == null)
                        return;
                    GenSpawn.Spawn(pawnFlyer, destinationPosition, map);
                    if (targetWasSelected)
                        Find.Selector.Select(target, false, false);
                }
            }
        }
    }
}
