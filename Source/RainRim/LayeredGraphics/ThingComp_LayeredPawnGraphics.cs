using System.Collections.Generic;
using System.Linq;
using RainRim.Utils;
using UnityEngine;
using Verse;

namespace RainRim.LayeredGraphics;

// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnassignedField.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable ConvertToConstant.Global
// ReSharper disable MemberCanBePrivate.Global

public class ThingComp_LayeredPawnGraphics : ThingComp
{
    public CompProperties_LayeredPawnGraphics Props => (CompProperties_LayeredPawnGraphics)props;

    public readonly Dictionary<string, GraphicLayer> LayersByName = new();
    public Color ColorizeColor;
    public float HueFactor;
    public float SatFactor;
    public float LitFactor;

    private float _layerAltInc;
    
    public override void Initialize(CompProperties properties)
    {
        props = properties;

        _layerAltInc = Altitudes.AltInc / (Props.layers.Count + 1);
        
        foreach (var layer in Props.layers)
        {
            layer.Graphic = layer.graphicData.Graphic;
            
            if (layer.name == null) continue;
            LayersByName.Add(layer.name, layer);
        }

        if (Scribe.mode == LoadSaveMode.LoadingVars || Props.colorizePalette == null) return;
        
        HueFactor = Random.value;
        SatFactor = Random.value;
        LitFactor = Random.value;

        ColorizeColor = Props.colorizePalette.PickColor(HueFactor, SatFactor, LitFactor);
    }

    public override void PostExposeData()
    {
        base.PostExposeData();

        if (Props.colorizePalette == null) return;
        
        Scribe_Values.Look(ref HueFactor, nameof(HueFactor));
        Scribe_Values.Look(ref SatFactor, nameof(SatFactor));
        Scribe_Values.Look(ref LitFactor, nameof(LitFactor));

        if (Scribe.mode == LoadSaveMode.LoadingVars)
            ColorizeColor = Props.colorizePalette.PickColor(HueFactor, SatFactor, LitFactor);
    }
}

public class CompProperties_LayeredPawnGraphics : CompProperties
{
    public GradientPalette colorizePalette;
    public List<GraphicLayer> layers;
    
    public CompProperties_LayeredPawnGraphics()
    {
        compClass = typeof(ThingComp_LayeredPawnGraphics);
    }

    public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
    {
        foreach (var error in base.ConfigErrors(parentDef)) yield return error;

        if (parentDef.thingClass != typeof(Pawn))
        {
            yield return parentDef.defName + " has LayeredPawnGraphics but is not a Pawn.";
            yield break;
        }
        
        if (layers.NullOrEmpty())
        {
            yield return parentDef.defName + " has LayeredPawnGraphics with no defined layers";
            yield break;
        }

        var seenNames = new List<string>();
        for (var i = 0; i < layers.Count; i++)
        {
            var layer = layers[i];
            if (layer.name == null)
                yield return parentDef.defName + " has LayeredPawnGraphics, layer with index " + i + " has no name.";
            else
            {
                if (seenNames.Contains(layer.name))
                    yield return parentDef.defName + " has LayeredPawnGraphics, layer with index " + i + " has " +
                                 "duplicate name with previously seen layer.";
                seenNames.Add(layer.name);
            }
            if (colorizePalette == null && layer.colorize)
                yield return parentDef.defName + " has LayeredPawnGraphics, layer with index " + i
                             + (layer.name != null ? " (name: " + layer.name + ") " : " ") + "is set to colorize = " +
                             "true, but there is no defined palette.";
            if (layer.graphicData == null)
                yield return parentDef.defName + " has LayeredPawnGraphics, layer with index " + i
                             + (layer.name != null ? " (name: " + layer.name + ") " : " ") + "has no graphicData.";
            else if (layer.graphicData.graphicClass != typeof(Graphic_Multi))
                yield return parentDef.defName + " has LayeredPawnGraphics, layer with index " + i
                             + (layer.name != null ? " (name: " + layer.name + ") " : " ") + "has graphicData, " +
                             "but its graphicClass is not Graphic_Multi.";
        }
    }
}

public class GraphicLayer
{
    // Should not be set in def, but it's fine if it is since it will always be overwritten anyways.
    public Graphic Graphic;
    
    public string name = null;
    public bool colorize = true;
    public GraphicData graphicData;
}