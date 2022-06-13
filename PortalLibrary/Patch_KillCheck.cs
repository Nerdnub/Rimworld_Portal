using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using HarmonyLib;

namespace PortalLibrary
{
    [HarmonyPatch(typeof(Targeter), "ConfirmStillValid")]
    public class Patch_KillCheck
    {
        [HarmonyPrefix]
        public static bool prefix(Targeter __instance)
        {
            if (__instance.targetingSource != null && __instance.targetingSource.GetVerb is Verb_Link) 
            {
                return false;
            }
            return true;
        }
    }
}
