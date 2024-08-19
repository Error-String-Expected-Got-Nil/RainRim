using System.Reflection;
using HarmonyLib;
using Verse;

namespace RainRim;

[StaticConstructorOnStartup]
public static class Startup
{
    static Startup()
    {
        Harmony.DEBUG = false;
        var harmony = new Harmony("RainRim");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}