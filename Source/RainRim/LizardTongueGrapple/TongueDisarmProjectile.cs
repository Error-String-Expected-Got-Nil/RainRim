using RainUtils.Utils;
using RimWorld;
using UnityEngine;
using Verse;

namespace RainRim.LizardTongueGrapple;

public class TongueDisarmProjectile : TongueGrappleProjectile
{
    protected override void Impact(Thing hitThing, bool blockedByShield = false)
    {
        if (StemRoot?.GetComp<ThingComp_TongueStemDrawer>() is { } drawerComp1 && drawerComp1.StemAnchor == this)
            drawerComp1.StemAnchor = null;

        if (launcher is not Pawn lizard)
        {
            BaseImpact(hitThing, blockedByShield);
            return;
        }

        var thingDestinationPosition = lizard.Position;
        var thingInitialPosition = destination;
        
        if (hitThing is Pawn target && CanDisarm(target) && RollDisarm(target) && target.equipment.TryDropEquipment(
                target.equipment.Primary, out var dropped, target.Position))
        {
            thingDestinationPosition = GetDestinationPosition(lizard.Position, target.Position, Map);
            thingInitialPosition = dropped.TrueCenter();

            var equipmentFlyer = (TongueRetractPawnFlyer)ThingFlyer.MakeThingFlyer(
                RW_Common.RW_ThingDefOf.RW_LizardTongueRetractFlyer, dropped, thingDestinationPosition, 
                thingInitialPosition);
            
            if (equipmentFlyer != null)
            {
                equipmentFlyer.DestroyOnLand = false;
                GenSpawn.Spawn(equipmentFlyer, lizard.Position, lizard.Map);
            }
        }
            
        // Always spawn tongue tip retract dummy, needed whether target was disarmed or not.
        var tongueTipDummy = (TongueTipDummy) ThingMaker.MakeThing(RW_Common.RW_ThingDefOf.RW_TongueTipDummy);
        GenSpawn.Spawn(tongueTipDummy, DestinationCell, Map);
        tongueTipDummy.RotationAnchor = StemRoot;

        var thingFlyer = (TongueRetractPawnFlyer)ThingFlyer.MakeThingFlyer(
            RW_Common.RW_ThingDefOf.RW_LizardTongueRetractFlyer, tongueTipDummy, thingDestinationPosition, 
            thingInitialPosition);
        
        BaseImpact(hitThing, blockedByShield);
            
        if (thingFlyer == null) return;

        if (StemRoot?.GetComp<ThingComp_TongueStemDrawer>() is { StemAnchor: null } drawerComp2)
        {
            thingFlyer.StemRoot = StemRoot;
            drawerComp2.StemAnchor = tongueTipDummy;
        }

        GenSpawn.Spawn(thingFlyer, lizard.Position, lizard.Map);
    }

    private static bool CanDisarm(Pawn target)
    {
        var equipment = target.equipment.Primary;
        if (equipment == null) return false;

        return !equipment.def.destroyOnDrop;
    }
    
    private static bool RollDisarm(Pawn target)
    {
        // Chance = 100% - (melee level * 5% * 0.8) * Manipulation
        // So a max melee pawn has only a 20% chance of being disarmed, more or less depending on their manipulation.
        var chance = 1f - (float)target.skills.GetSkill(SkillDefOf.Melee).Level / 20 * 0.8f
            * target.health.capacities.GetLevel(PawnCapacityDefOf.Manipulation);

        return Random.value < chance;
    }

    // Don't want to use base.Impact() because that would call TongueGrappleProjectile's version
    // This is just a copy of the base Projectile.Impact() method
    private void BaseImpact(Thing hitThing, bool blockedByShield = false)
    {
        GenClamor.DoClamor(this, 12f, ClamorDefOf.Impact);
        if (!blockedByShield && def.projectile.landedEffecter != null)
            def.projectile.landedEffecter.Spawn(Position, Map).Cleanup();
        Destroy();
    }
}