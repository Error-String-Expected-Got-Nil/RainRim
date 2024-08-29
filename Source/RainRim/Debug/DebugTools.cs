using LudeonTK;
using RainRim.CreatureCosmetics;
using Verse;
using Random = UnityEngine.Random;

namespace RainRim.Debug;

public static class DebugTools
{
    [DebugAction("RainRim", "Log random color info", actionType = DebugActionType.ToolMapForPawns, 
        allowedGameStates = AllowedGameStates.PlayingOnMap)]
    public static void LogRandomColorPickerInfo(Pawn pawn)
    {
        var colorComp = pawn.GetComp<ThingComp_RandomColorPicker>();

        if (colorComp == null)
        {
            Log.Message("Pawn " + pawn.LabelShort + " had no ThingComp_RandomColorPicker");
            return;
        }
        
        Log.Message("Pawn " + pawn.LabelShort + " color:");
        Log.Message("HueFactor: " + colorComp.HueFactor + 
                    "; SatFactor: " + colorComp.SatFactor + 
                    "; LitFactor: " + colorComp.LitFactor + 
                    "; Color: " + colorComp.Color);
    }

    [DebugAction("RainRim", "Reroll random color", actionType = DebugActionType.ToolMapForPawns, 
        allowedGameStates = AllowedGameStates.PlayingOnMap)]
    public static void RerollRandomColor(Pawn pawn)
    {
        var colorComp = pawn.GetComp<ThingComp_RandomColorPicker>();

        if (colorComp == null)
        {
            Log.Message("Pawn " + pawn.LabelShort + " had no ThingComp_RandomColorPicker");
            return;
        }

        float hf = Random.value, sf = Random.value, lf = Random.value;
        var newColor = colorComp.Props.gradientPalette.PickColor(hf, sf, lf);
        Log.Message("Pawn " + pawn.LabelShort + " new color:");
        Log.Message("HueFactor: " + hf + 
                    "; SatFactor: " + sf + 
                    "; LitFactor: " + lf + 
                    "; Color: " + newColor);

        colorComp.HueFactor = hf;
        colorComp.SatFactor = sf;
        colorComp.LitFactor = lf;
        colorComp.Color = newColor;
    }
}