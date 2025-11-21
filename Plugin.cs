using AceAttorneyTrilogyThaiMod_BepInEx_5_Mono.obj;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;

namespace AceAttorneyTrilogyThaiMod_BepInEx_5_Mono;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("PWAAT.exe")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    private static ConfigEntry<TranslationMode> _translationMode;
    public static TranslationMode TranslationMode => _translationMode.Value;
        
    private void Awake()
    {
        // Plugin startup logic
        _translationMode = Config.Bind(
            "Translation", 
            "TranslationMode", 
            TranslationMode.Translated,
            "Choose the translation mode: Translated or Localized.");
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        Logger.LogInfo($"Translation Mode: {TranslationMode}");
        JudgmentCtrlPatch.Patch();
        DyingMessageMiniGamePatch.Patch();
        DyingMessageUtilPatch.Patch();
        AutoPlayCtrlPatch.Patch();
    }
}

public enum TranslationMode
{
    Translated = 0,
    Localized = 1
}
