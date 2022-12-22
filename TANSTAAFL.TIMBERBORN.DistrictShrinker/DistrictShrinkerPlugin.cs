using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using TimberApi.Common.Extensions;
using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;
using Timberborn.EditorStarter;
using Timberborn.EntitySystem;
using Timberborn.Growing;
using Timberborn.IrrigationSystem;
using Timberborn.MapIndexSystem;
using Timberborn.NaturalResourcesModelSystem;
using Timberborn.NaturalResourcesReproduction;
using Timberborn.Navigation;
using Timberborn.Persistence;
using Timberborn.SoilMoistureSystem;
using Timberborn.TickSystem;
using Timberborn.WaterSystem;
using Timberborn.WeatherSystem;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.UIElements;

namespace TANSTAAFL.TIMBERBORN.DistrictShrinker
{
    [HarmonyPatch]
    public class DistrictShrinkerPlugin : IModEntrypoint
    {
        public static DistrictShrinkerConfig Config;
        internal static IConsoleWriter Log;

        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            Log = consoleWriter;
            Config = mod.Configs.Get<DistrictShrinkerConfig>();

            var harmony = new Harmony("tanstaafl.plugins.DistrictShrinker");
            harmony.PatchAll();
        }

        /// <summary>
        /// Patch the NavigationDistance.Load to shrink the distric range
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(typeof(NavigationDistance), nameof(NavigationDistance.Load))]
        public static void Postfix(NavigationDistance __instance)
        {
            UdateDistrictRange(__instance, false);
        }

        public static void UdateDistrictRange(NavigationDistance __instance, bool isDroughtEndedEvent)
        {
            var cycle = TimberApi.DependencyContainerSystem.DependencyContainer.GetInstance<WeatherService>().Cycle;

            var reductionsToApply = Config.CyclesPerReduction == 0 ? 0 : (cycle / Config.CyclesPerReduction) - 1;
            var droughtEndedOffByOne = isDroughtEndedEvent ? -1 : 0;
            var newValue = Config.StartingRange - (reductionsToApply * Config.ReductionAmount) + droughtEndedOffByOne;
            newValue = Math.Min(Math.Max(newValue, Config.MinimumRange), Config.MaximumRange);

            Log.LogInfo($"DistrictShrinker. Cycle: {cycle} old range: {__instance.DistrictRoad} new range: {newValue}");

            FieldInfo DistrictRoadField = typeof(NavigationDistance).GetField("<DistrictRoad>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            DistrictRoadField.SetValue(__instance, newValue);
        }
    }
}
