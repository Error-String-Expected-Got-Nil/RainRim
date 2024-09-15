using UnityEngine;
using Verse;

namespace RainRim.LizardSpit;

public class LizardSpitProjectile : Projectile
{
    private ModExtension_LizardSpitProps Props => def.GetModExtension<ModExtension_LizardSpitProps>();

    protected override void Impact(Thing hitThing, bool blockedByShield = false)
    {
        base.Impact(hitThing, blockedByShield);

        if (Props == null || hitThing is not Pawn { health.hediffSet: not null } target) return;
        if (blockedByShield && Random.value < Props.shieldBlockChance) return;

        var spitHediff = target.health.hediffSet.GetFirstHediffOfDef(Props.hediff)
                         ?? target.health.AddHediff(Props.hediff);
        spitHediff.Severity += Props.spitSeverity;
    }
}