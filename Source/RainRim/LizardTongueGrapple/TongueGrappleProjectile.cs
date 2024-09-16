using System.Diagnostics.CodeAnalysis;
using RainUtils.Utils;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RainRim.LizardTongueGrapple;

public class TongueGrappleProjectile : Projectile
{
    protected ThingWithComps StemRoot;
    
    // Changes the position function from a simple lerp to the inverse parabola stretched so that its peak is
    // reached at x = 1.0. y = 0 at x = 0; y = 0.75 at x = 0.5; y = 1 at x = 1
    public override Vector3 ExactPosition 
        => origin.Yto0() 
           + (destination - origin).Yto0() * GenMath.InverseParabola(DistanceCoveredFraction * 0.5f)
           + Vector3.up * def.Altitude;

    public override Quaternion ExactRotation
        => StemRoot?.GetComp<ThingComp_TongueStemDrawer>()?.RootPosition is { } drawPos
            ? Quaternion.LookRotation((ExactPosition - drawPos).Yto0())
            : base.ExactRotation;

    [SuppressMessage("ReSharper", "ParameterHidesMember")]
    public override void Launch(Thing launcher, Vector3 origin, LocalTargetInfo usedTarget, 
        LocalTargetInfo intendedTarget, ProjectileHitFlags hitFlags, bool preventFriendlyFire = false, 
        Thing equipment = null, ThingDef targetCoverDef = null)
    {
        base.Launch(launcher, origin, usedTarget, intendedTarget, hitFlags, preventFriendlyFire, equipment, 
            targetCoverDef);

        if (launcher is ThingWithComps launcherWithComps
            && launcherWithComps.GetComp<ThingComp_TongueStemDrawer>() is { } drawerComp)
        {
            StemRoot = launcherWithComps;
            drawerComp.StemAnchor = this;
        }
        else
            Log.Warning("[RainRim] - TongueGrappleProjectile could not find TongueStemDrawer on launcher. " +
                        "Stem will not be rendered.");
    }
    
    protected override void Impact(Thing hitThing, bool blockedByShield = false)
    {
        // TODO: Combat log entry?
        
        if (StemRoot?.GetComp<ThingComp_TongueStemDrawer>() is { } drawerComp1 && drawerComp1.StemAnchor == this)
            drawerComp1.StemAnchor = null;

        if (hitThing is Pawn target && launcher is Pawn lizard && CanGrappleTarget(lizard, target))
        {
            RW_Common.RW_SoundDefOf.RW_LizardTongueAttach.PlayOneShot(new TargetInfo(Position, Map));
            
            base.Impact(hitThing, blockedByShield);
            
            var map = target.Map;
            var targetWasSelected = Find.Selector.IsSelected(target);
            var destinationPos = GetDestinationPosition(lizard.Position, target.Position, map);

            var pawnFlyer = (TongueGrapplePawnFlyer)PawnFlyer.MakeFlyer(
                RW_Common.RW_ThingDefOf.RW_LizardTongueGrappleFlyer,
                target, destinationPos, null, RW_Common.RW_SoundDefOf.RW_LizardTongueDetach);

            if (pawnFlyer == null) return;

            if (StemRoot?.GetComp<ThingComp_TongueStemDrawer>() is { StemAnchor: null } drawerComp2)
            {
                pawnFlyer.StemRoot = StemRoot;
                drawerComp2.StemAnchor = target;
            }

            GenSpawn.Spawn(pawnFlyer, destinationPos, map);
            if (targetWasSelected) Find.Selector.Select(target, false, false);

            return;
        }

        if (launcher is Pawn pawn)
        {
            var tongueTipDummy = (TongueTipDummy)ThingMaker.MakeThing(RW_Common.RW_ThingDefOf.RW_TongueTipDummy);
            GenSpawn.Spawn(tongueTipDummy, DestinationCell, Map);
            tongueTipDummy.RotationAnchor = StemRoot;

            var thingFlyer = (TongueRetractPawnFlyer)ThingFlyer.MakeThingFlyer(
                RW_Common.RW_ThingDefOf.RW_LizardTongueRetractFlyer, tongueTipDummy, pawn.Position, destination);

            // base.Impact() destroys the Projectile so we have to do it after getting things from it we need
            base.Impact(hitThing, blockedByShield);
            
            if (thingFlyer == null) return;

            if (StemRoot?.GetComp<ThingComp_TongueStemDrawer>() is { StemAnchor: null } drawerComp3)
            {
                thingFlyer.StemRoot = StemRoot;
                drawerComp3.StemAnchor = tongueTipDummy;
            }

            GenSpawn.Spawn(thingFlyer, pawn.Position, pawn.Map);

            return;
        }
        
        base.Impact(hitThing, blockedByShield);
    }

    private static bool CanGrappleTarget(Pawn lizard, Pawn target)
    {
        return lizard.BodySize >= target.BodySize;
    }
        
    // Gets the space adjacent to the origin that's between the origin and the target, or the origin if that space
    // cannot be stood on
    protected static IntVec3 GetDestinationPosition(IntVec3 origin, IntVec3 target, Map map)
    {
        var relativePosition = (target - origin).ToVector3();
        relativePosition.Normalize();
        var roundedRelativePosition = new IntVec3((int)Mathf.Round(relativePosition.x), 0, 
            (int)Mathf.Round(relativePosition.z));
        var candidatePosition = origin + roundedRelativePosition;
            
        return IsValidPositionTarget(map, candidatePosition) ? candidatePosition : origin;
    }

    private static bool IsValidPositionTarget(Map map, IntVec3 cell)
    {
        if (!cell.IsValid || !cell.InBounds(map) || cell.Impassable(map) || !cell.Walkable(map))
            return false;
        var edifice = cell.GetEdifice(map);
        return edifice is not Building_Door buildingDoor || buildingDoor.Open;
    }

    public override void ExposeData()
    {
        base.ExposeData();
        
        Scribe_References.Look(ref StemRoot, nameof(StemRoot));
    }
}