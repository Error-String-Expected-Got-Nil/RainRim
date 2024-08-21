using UnityEngine;

namespace RainRim.Utils;

// ReSharper disable InconsistentNaming

public static class ColorUtils
{
    // Assumes inputs are all on range [0, 1]
    public static Color HSL2RGB(float hue, float saturation, float lightness)
    {
        var chroma = (1f - Mathf.Abs(2f * lightness - 1f)) * saturation;

        if (chroma == 0f) return new Color(lightness, lightness, lightness);
        
        var secondary = chroma * (1f - Mathf.Abs(hue * 6f % 2f - 1f));
        var baseline = lightness - chroma * 0.5f;

        float red = 0f, green = 0f, blue = 0f;
        switch ((int)(hue * 6f) % 6)
        {
            case 0:
                red = chroma;
                green = secondary;
                break;
            case 1:
                red = secondary;
                green = chroma;
                break;
            case 2:
                green = chroma;
                blue = secondary;
                break;
            case 3:
                green = secondary;
                blue = chroma;
                break;
            case 4:
                red = secondary;
                blue = chroma;
                break;
            case 5:
                red = chroma;
                blue = secondary;
                break;
        }

        return new Color(red + baseline, green + baseline, blue + baseline);
    }
}