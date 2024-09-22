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
        
        // TEMP; Fill with settings if/when any are needed
        
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
    
}