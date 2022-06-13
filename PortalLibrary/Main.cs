using Verse;
using HarmonyLib;
using System.Reflection;

namespace PortalLibrary
{
    [StaticConstructorOnStartup]
    public class PatchMain
    {
        static PatchMain()
        {
            var instance = new Harmony("PortalLibrary");
            instance.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}