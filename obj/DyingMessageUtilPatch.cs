using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace AceAttorneyTrilogyThaiMod_BepInEx_5_Mono.obj;

[HarmonyPatch]
public class DyingMessageUtilPatch
{
    public static void Patch()
    {
        Console.WriteLine($"Patching class: {nameof(DyingMessageUtil)}...");
        Harmony.CreateAndPatchAll(typeof(DyingMessageUtilPatch));
        Console.WriteLine($"Patching class: {nameof(DyingMessageUtil)}... Done");
    }
    
    #region Patches

    private static readonly FieldInfo _lineListField = AccessTools.Field(typeof(DyingMessageUtil), "line_list_");
    [HarmonyPatch(typeof(DyingMessageUtil), "checkDieMessage_us")]
    [HarmonyPrefix]
    public static void DyingMessageUtil_checkDieMessage_us_Prefix(ref bool __result, ref bool __runOriginal, DyingMessageUtil __instance)
    {
        __runOriginal = false;
        var line_list_ = (List<DyingMessageUtil.tagLINE_LIST>)_lineListField.GetValue(__instance);
        if (Plugin.TranslationMode is TranslationMode.Translated)
        {
            bool flag = false;
            int num = line_list_.Count;
            for (int i = 0; i < line_list_.Count; i++)
            {
                int pt = line_list_[i].pt1;
                int pt2 = line_list_[i].pt2;
                switch (pt)
                {
                    case 0:
                        if (pt2 != 1)
                        {
                            flag = true;
                        }
                        break;
                    case 1:
                        flag = true;
                        break;
                    case 2:
                        if (pt2 != 3)
                        {
                            flag = true;
                        }
                        break;
                    case 3:
                        if (pt2 != 4)
                        {
                            flag = true;
                        }
                        break;
                    case 4:
                        flag = true;
                        break;
                    case 5:
                        if (pt2 != 7)
                        {
                            flag = true;
                        }
                        break;
                    case 6:
                        if (pt2 != 7)
                        {
                            if (pt2 != 8)
                            {
                                flag = true;
                            }
                        }
                        else
                        {
                            num--;
                        }
                        break;
                    case 7:
                        flag = true;
                        break;
                    case 8:
                        if (pt2 != 9)
                        {
                            flag = true;
                        }
                        break;
                    case 9:
                        flag = true;
                        break;
                    case 10:
                        if (pt2 != 11)
                        {
                            flag = true;
                        }
                        break;
                    case 11:
                        flag = true;
                        break;
                }
            }
            __result = !flag && num == 7;
        }
        else
        {
            bool flag = false;
            int num = line_list_.Count;
            for (int i = 0; i < line_list_.Count; i++)
            {
                int pt = line_list_[i].pt1;
                int pt2 = line_list_[i].pt2;
                switch (pt)
                {
                    case 0:
                        if (pt2 != 1)
                        {
                            flag = true;
                        }
                        break;
                    case 1:
                        if (pt2 != 2)
                        {
                            flag = true;
                        }
                        break;
                    case 2:
                        if (pt2 != 4)
                        {
                            flag = true;
                        }
                        else
                        {
                            num--;
                        }
                        break;
                    case 3:
                        if (pt2 != 4)
                        {
                            flag = true;
                        }
                        break;
                    case 4:
                        flag = true;
                        break;
                    case 5:
                        if (pt2 != 6)
                        {
                            flag = true;
                        }
                        break;
                    case 6:
                        if (pt2 != 7)
                        {
                            flag = true;
                        }
                        break;
                    case 7:
                        if (pt2 != 9)
                        {
                            flag = true;
                        }
                        else
                        {
                            num--;
                        }
                        break;
                    case 8:
                        if (pt2 != 9)
                        {
                            flag = true;
                        }
                        break;
                    case 9:
                        flag = true;
                        break;
                    case 10:
                        if (pt2 != 11)
                        {
                            flag = true;
                        }
                        break;
                    case 11:
                        flag = true;
                        break;
                }
            }
            __result = !flag && num == 7;
        }
    }

    private static readonly FieldInfo _mg_chk_die_usField = AccessTools.Field(typeof(DyingMessageUtil), "mg_chk_die_us");
    [HarmonyPatch(typeof(DyingMessageUtil), MethodType.Constructor)]
    [HarmonyPostfix]
    public static void DyingMessageUtil_Constructor_Postfix(DyingMessageUtil __instance)
    {
        _mg_chk_die_usField.SetValue(__instance, CustomLinePointsDict[Plugin.TranslationMode]);
    }
    #endregion
    
    #region Helpers

    private static readonly Dictionary<TranslationMode, DyingMessageUtil.tagMG_CHECK_LINEPOINT[]>
        CustomLinePointsDict =
            new()
            {
                {
                    TranslationMode.Translated,
                    new DyingMessageUtil.tagMG_CHECK_LINEPOINT[]
                    {
                        new(52, 47, 16, 16, 59, 54),
                        new(30, 109, 16, 64, 37, 118),
                        new(68, 116, 16, 16, 75, 125),
                        new(99, 124, 16, 16, 106, 133),
                        new(113, 91, 16, 16, 120, 98),
                        new(141, 85, 16, 16, 148, 92),
                        new(123, 110, 16, 16, 130, 119),
                        new(132, 126, 16, 16, 139, 135),
                        new(161, 127, 16, 16, 168, 136),
                        new(180, 66, 16, 16, 187, 73),
                        new(226, 81, 16, 16, 233, 88),
                        new(212, 131, 16, 16, 219, 140)
                    }
                },
                {
                    TranslationMode.Localized,
                    new DyingMessageUtil.tagMG_CHECK_LINEPOINT[]
                    {
                        new(41, 79, 16, 16, 48, 86),
                        new(56, 158, 32, 32, 63, 168),
                        new(109, 112, 16, 16, 116, 120),
                        new(93, 116, 16, 16, 99, 122),
                        new(99, 147, 16, 16, 105, 157),
                        new(135, 87, 16, 16, 142, 94),
                        new(141, 132, 32, 32, 147, 141),
                        new(194, 88, 16, 16, 201, 95),
                        new(166, 51, 16, 16, 174, 58),
                        new(178, 119, 16, 16, 185, 126),
                        new(43, 54, 16, 16, 50, 61),
                        new(81, 48, 16, 16, 88, 58)
                    }
                }
            };
    #endregion
}