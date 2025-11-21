using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace AceAttorneyTrilogyThaiMod_BepInEx_5_Mono.obj;

[HarmonyPatch]
public class DyingMessageMiniGamePatch
{
    public static void Patch()
    {
        Console.WriteLine($"Patching class: {nameof(DyingMessageMiniGame)}...");
        Harmony.CreateAndPatchAll(typeof(DyingMessageMiniGamePatch));
        Console.WriteLine($"Patching class: {nameof(DyingMessageMiniGame)}... Done");
    }

    #region Patchs
    
    private static FieldInfo _spriteListField;
    [HarmonyPatch(typeof(DyingMessageMiniGame), "_setSpritePosition")]
    [HarmonyPostfix]
    public static void DyingMessageMiniGame__setSpritePosition_Postfix(DyingMessageMiniGame __instance)
    {
        if (GSStatic.global_work_.language is not Language.USA) return;
        _spriteListField ??= AccessTools.Field(typeof(DyingMessageMiniGame), "sprite_list_");
        var sprite_list_ = (List<KeyValuePair<GameObject, AssetBundleSprite>>)_spriteListField.GetValue(__instance);
        if (Plugin.TranslationMode is TranslationMode.Translated)
        {
            sprite_list_[0].Key.transform.localPosition = new Vector3(-423f, 312f, -10f);
            sprite_list_[1].Key.transform.localPosition = new Vector3(-570f, -96f, -10f);
            sprite_list_[2].Key.transform.localPosition = new Vector3(-319f, -137f, -10f);
            sprite_list_[3].Key.transform.localPosition = new Vector3(-134f, -181f, -10f);
            sprite_list_[4].Key.transform.localPosition = new Vector3(-40f, 25f, -10f);
            sprite_list_[5].Key.transform.localPosition = new Vector3(140f, 80f, -10f);
            sprite_list_[6].Key.transform.localPosition = new Vector3(20f, -97f, -10f);
            sprite_list_[7].Key.transform.localPosition = new Vector3(80f, -202f, -10f);
            sprite_list_[8].Key.transform.localPosition = new Vector3(250f, -202f, -10f);
            sprite_list_[9].Key.transform.localPosition = new Vector3(389f, 203f, -10f);
            sprite_list_[10].Key.transform.localPosition = new Vector3(689f, 109f, -10f);
            sprite_list_[11].Key.transform.localPosition = new Vector3(591f, -226f, -10f);
        }
        else
        {
            sprite_list_[0].Key.transform.localPosition = new Vector3(-496f, 94f, -10f);
            sprite_list_[1].Key.transform.localPosition = new Vector3(-399f, -398f, -10f);
            sprite_list_[2].Key.transform.localPosition = new Vector3(-75f, -111f, -10f);
            sprite_list_[3].Key.transform.localPosition = new Vector3(-169f, -132f, -10f);
            sprite_list_[4].Key.transform.localPosition = new Vector3(-138f, -337f, -10f);
            sprite_list_[5].Key.transform.localPosition = new Vector3(102f, 63f, -10f);
            sprite_list_[6].Key.transform.localPosition = new Vector3(154f, -215f, -10f);
            sprite_list_[7].Key.transform.localPosition = new Vector3(480f, 63f, -10f);
            sprite_list_[8].Key.transform.localPosition = new Vector3(304f, 296f, -10f);
            sprite_list_[9].Key.transform.localPosition = new Vector3(375f, -145f, -10f);
            sprite_list_[10].Key.transform.localPosition = new Vector3(-486f, 274f, -10f);
            sprite_list_[11].Key.transform.localPosition = new Vector3(-247f, 304f, -10f);
        }
    }

    #endregion
}