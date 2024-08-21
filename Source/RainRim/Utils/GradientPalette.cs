using UnityEngine;

namespace RainRim.Utils;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable ConvertToConstant.Global

public class GradientPalette
{
    // Base is the baseline value. Variation is the maximum deviation from that value, either positively or negatively.
    // Curve is the value of k for the WeightedCurve function.
    public float hueBase = 0f;
    public float hueVariation = 0f;
    public float hueCurve = 1f;
    
    public float satBase = 1f;
    public float satVariation = 0f;
    public float satCurve = 1f;
    
    public float litBase = 0.5f;
    public float litVariation = 0f;
    public float litCurve = 1f;

    // Hue is wrapped around to fit into range [0, 1), sat and lit are clamped to [0, 1]
    public Color PickColor(float hueFactor, float satFactor, float litFactor) 
        => ColorUtils.HSL2RGB(
            (hueBase + hueVariation * WeightedCurve(hueFactor, hueCurve)) % 1, 
            Mathf.Clamp01(satBase + satVariation * WeightedCurve(satFactor, satCurve)), 
            Mathf.Clamp01(litBase + litVariation * WeightedCurve(litFactor, litCurve)));

    // Maps domain [0, 1] to range [-1, 1] on a weighted curve. This is the simplified form of a set of functions
    // that Rain World actually uses when randomly selecting colors for things. Not using the original system since
    // that'd be excessive (and it feels wrong to just verbatim copy decompiled code).
    //
    // The k value is on range (0, inf). The shape of the curve asymptotically approaches a linear function as k 
    // increases, and is more heavily weighted towards the ends at lower values of k. Don't use k = 0, it is a flat
    // line at y = 0.
    public static float WeightedCurve(float x, float k)
    {
        x = x * 2f - 1f;
        return k * Mathf.Abs(x) / (k - Mathf.Abs(x) + 1f) * Mathf.Sign(x);
    }
}