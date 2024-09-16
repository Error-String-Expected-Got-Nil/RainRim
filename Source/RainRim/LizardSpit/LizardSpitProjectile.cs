using UnityEngine;
using Verse;

namespace RainRim.LizardSpit;

public class LizardSpitProjectile : Projectile
{
    private ModExtension_LizardSpitProps Props => def.GetModExtension<ModExtension_LizardSpitProps>();

    protected override void Impact(Thing hitThing, bool blockedByShield = false)
    {
        // TODO: Combat log entry
        
        base.Impact(hitThing, blockedByShield);

        if (hitThing is not Pawn { health.hediffSet: not null } target || Props is not { } props) return;
        if (blockedByShield && Random.value < props.shieldBlockChance) return;

        var spitHediff = target.health.hediffSet.GetFirstHediffOfDef(props.hediff)
                         ?? target.health.AddHediff(props.hediff);
        spitHediff.Severity += props.spitSeverity;
    }
}