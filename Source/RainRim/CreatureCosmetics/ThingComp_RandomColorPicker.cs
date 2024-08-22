using System.Collections.Generic;
using RainRim.Utils;
using UnityEngine;
using Verse;

namespace RainRim.CreatureCosmetics;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnassignedField.Global

public class ThingComp_RandomColorPicker : ThingComp
{
    public CompProperties_RandomColorPicker Props => (CompProperties_RandomColorPicker)props;

    public Color Color;
    public float HueFactor;
    public float SatFactor;
    public float LitFactor;
    
    public override void Initialize(CompProperties properties)
    {
        props = properties;

        if (Scribe.mode == LoadSaveMode.LoadingVars) return;

        HueFactor = Random.value;
        SatFactor = Random.value;
        LitFactor = Random.value;

        Color = Props.gradientPalette.PickColor(HueFactor, SatFactor, LitFactor);
    }

    public override void PostExposeData()
    {
        base.PostExposeData();
        
        Scribe_Values.Look(ref HueFactor, nameof(HueFactor));
        Scribe_Values.Look(ref SatFactor, nameof(SatFactor));
        Scribe_Values.Look(ref LitFactor, nameof(LitFactor));

        if (Scribe.mode == LoadSaveMode.LoadingVars)
            Color = Props.gradientPalette.PickColor(HueFactor, SatFactor, LitFactor);
    }
}

public class CompProperties_RandomColorPicker : CompProperties
{
    public GradientPalette gradientPalette;

    public CompProperties_RandomColorPicker()
    {
        compClass = typeof(ThingComp_RandomColorPicker);
    }

    public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
    {
        foreach (var error in base.ConfigErrors(parentDef)) yield return error;

        if (gradientPalette == null) yield return parentDef.defName + " has RandomColorPicker with no defined palette";
    }
}