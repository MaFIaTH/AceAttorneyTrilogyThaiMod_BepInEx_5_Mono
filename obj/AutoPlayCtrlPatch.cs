using System;
using HarmonyLib;

namespace AceAttorneyTrilogyThaiMod_BepInEx_5_Mono.obj;

[HarmonyPatch]
public class AutoPlayCtrlPatch
{
    public static void Patch()
    {
        Console.WriteLine($"Patching class: {nameof(autoPlayCtrl)}...");
        Harmony.CreateAndPatchAll(typeof(AutoPlayCtrlPatch));
        Console.WriteLine($"Patching class: {nameof(autoPlayCtrl)}... Done");
    }
    
    #region Patches
    [HarmonyPatch(typeof(autoPlayCtrl), "GetDieMessData")]
    [HarmonyPostfix]
    public static void autoPlayCtrl_GetDieMessData_Postfix(ref int[,] __result, autoPlayCtrl __instance)
    {
        if (GSStatic.global_work_.language is not Language.USA) return;
        if (Plugin.TranslationMode is TranslationMode.Translated)
        {
            __result = new[,]
            {
                { 0, 1 },
                { 4, 3 },
                { 3, 2 },
                { 5, 7 },
                { 6, 8 },
                { 8, 9 },
                { 10, 11 }
            };
        }
        else
        {
            __result = new[,]
            {
                { 0, 1 },
                { 1, 2 },
                { 4, 3 },
                { 10, 11 },
                { 5, 6 },
                { 6, 7 },
                { 9, 8 }
            };
        }
    }
    #endregion
}