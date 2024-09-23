using UnityEngine;
using Verse;

namespace RainRim;

// ReSharper disable InconsistentNaming

public class RW_Mod : Mod
{
    public static RW_Settings Settings;
    
    public RW_Mod(ModContentPack content) : base(content)
    {
        Settings = GetSettings<RW_Settings>();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var list = new Listing_Standard();
        list.Begin(inRect);
        
        Check("RW_Settings_RainbowMode", ref Settings.RainbowMode, "RW_Settings_RainbowMode_Tooltip");
        
        list.End();
        
        base.DoSettingsWindowContents(inRect);
        
        return;
        void Check(string labelKey, ref bool setting, string tooltipKey) =>
            list.CheckboxLabeled(labelKey.Translate(), ref setting, tooltipKey.Translate());
    }

    public override string SettingsCategory()
    {
        return "RW_Settings_Category".Translate();
    }
}

public class RW_Settings : ModSettings
{
    public bool RainbowMode;

    public override void ExposeData()
    {
        base.ExposeData();
        
        Scribe_Values.Look(ref RainbowMode, nameof(RainbowMode));
    }
}