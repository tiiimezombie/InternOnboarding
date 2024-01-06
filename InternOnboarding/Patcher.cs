using System.Collections.Generic;
using UnityEngine;

using GameNetcodeStuff;
using HarmonyLib;
using BepInEx.Logging;
using System.Linq;
using System;

namespace TZ.InternOnboarding
{
    internal class OverallPatch
    {
        internal static Terminal terminal;
        internal static RoundManager roundManager;
    }

    [HarmonyPatch(typeof(Terminal))]
    internal class PatchedTerminal
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        static void GrabDetails(ref Terminal __instance)
        {
            OverallPatch.terminal = __instance;
        }
    }

    [HarmonyPatch(typeof(TimeOfDay))]
	internal class PatchedTimeOfDay
	{
        static internal ManualLogSource _logSource;

		[HarmonyPatch("SetBuyingRateForDay")]
		[HarmonyPostfix]
        static void GiveItems(ref TimeOfDay __instance) // ref QuotaSettings __quotaSettings
        {
            _logSource = BepInEx.Logging.Logger.CreateLogSource(InternOnboardingInfo.PLUGIN_GUID);

            // need to make this only trigger once , ie the first day or when quota fulfilled is 0
            _logSource.LogInfo("i wanna give items");
            if ((int)__instance.quotaFulfilled > 0) return;
            _logSource.LogInfo("quota not fulfilled");
            if (CalculateLootValue() > 0) return;
            _logSource.LogInfo("scrap not collected");

            //_logSource.LogInfo((int)__instance.timeUntilDeadline);
            //_logSource.LogInfo((int)__instance.totalTime);
            //_logSource.LogInfo((int)Mathf.Floor(__instance.timeUntilDeadline / __instance.totalTime)); //__quotaSettings.deadlineDaysAmount

            OverallPatch.terminal.orderedItemsFromTerminal.Add(0);
            OverallPatch.terminal.orderedItemsFromTerminal.Add(0);
            OverallPatch.terminal.orderedItemsFromTerminal.Add(1);
        }

        /// <summary>
        /// https://github.com/tinyhoot/ShipLoot/blob/main/ShipLoot/Patches/HudManagerPatcher.cs
        /// </summary>
        private static float CalculateLootValue()
        {
            GameObject ship = GameObject.Find("/Environment/HangarShip");
            var loot = ship.GetComponentsInChildren<GrabbableObject>()
                .Where(obj => obj.itemProperties.isScrap).ToList();

            return loot.Sum(scrap => scrap.scrapValue);
        }
    }
}