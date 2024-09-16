using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RainUtils.LocalArmor;
using Verse;

namespace RainRim.CreatureCosmetics;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnassignedField.Global

public class ThingComp_LizardMoodHandler : ThingComp, ILocalArmorCallback
{
    private static readonly FieldInfo PawnRenderTree_NodesByTag_Info 
        = AccessTools.Field(typeof(PawnRenderTree), "nodesByTag");

    private PawnRenderNode _headNode;
    
    public CompProperties_LizardMoodHandler Props => (CompProperties_LizardMoodHandler)props;
    public Pawn ParentPawn => (Pawn)parent;
    
    public override void Initialize(CompProperties properties)
    {
        props = properties;

        var nodesByTag = (Dictionary<PawnRenderNodeTagDef, PawnRenderNode>)PawnRenderTree_NodesByTag_Info
            .GetValue(ParentPawn.Drawer.renderer.renderTree);
        if (!nodesByTag.TryGetValue(RW_Common.RW_PawnRenderNodeTagDefOf.RW_LizardHead, out _headNode))
            Log.Error("[RainRim] - Error while initializing LizardMoodHandler for pawn " 
                      + ParentPawn.ToStringSafe() + ", could not find PawnRenderNode tagged with RW_LizardHead");
    }

    public override void CompTick()
    {
        
    }
    
    public void LocalArmorCallback(float preArmorDamage, float postArmorDamage, float armorPen, DamageDef preArmorDef,
        DamageDef postArmorDef, Pawn pawn, bool metalArmor, BodyPartRecord part)
    {
        
    }
}

public class CompProperties_LizardMoodHandler : CompProperties
{
    public CompProperties_LizardMoodHandler()
    {
        compClass = typeof(ThingComp_LizardMoodHandler);
    }

    public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
    {
        foreach (var error in base.ConfigErrors(parentDef)) yield return error;

        if (parentDef.thingClass != typeof(Pawn)) 
            yield return parentDef.defName + " has LizardMoodHandler but is not a Pawn";
    }
}