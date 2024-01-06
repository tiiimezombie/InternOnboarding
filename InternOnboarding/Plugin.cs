using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

namespace TZ.InternOnboarding
{
    public class InternOnboardingInfo : PluginInfo
    {
        public const string PLUGIN_GUID = "TZ.InternOnboarding";
        public const string PLUGIN_NAME = "Intern Onboarding";
        public const string PLUGIN_VERSION = "1.0.0";
    }

    /// <summary>
    /// https://www.reddit.com/r/lethalcompany_mods/comments/18refsw/tutorial_creating_lethal_company_mods_with_c/
    /// https://www.youtube.com/watch?v=4Q7Zp5K2ywI
    /// </summary>
    [BepInPlugin(InternOnboardingInfo.PLUGIN_GUID, InternOnboardingInfo.PLUGIN_NAME, InternOnboardingInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private readonly Harmony _harmony = new Harmony(InternOnboardingInfo.PLUGIN_GUID);
        internal ManualLogSource _logSource;

        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {InternOnboardingInfo.PLUGIN_GUID} is loaded!");

            _logSource = BepInEx.Logging.Logger.CreateLogSource(InternOnboardingInfo.PLUGIN_GUID);
            _harmony.PatchAll(typeof(Plugin));

            _harmony.PatchAll(typeof(PatchedTerminal));
            _harmony.PatchAll(typeof(PatchedTimeOfDay));
        }
    }
}