using HarmonyLib;
using Mono.Cecil;
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
using Timberborn.BuildingsNavigation;
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

namespace TANSTAAFL.TIMBERBORN.DistrictManagement
{
    [HarmonyPatch]
    public class DistrictManagementPlugin : IModEntrypoint
    {
        public static DistrictManagementConfig Config;
        internal static IConsoleWriter Log;

        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            Log = consoleWriter;
            Config = mod.Configs.Get<DistrictManagementConfig>();

            var harmony = new Harmony("tanstaafl.plugins.DistrictManagement");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(ConstructionSiteAccessible), nameof(ConstructionSiteAccessible.MinZ), MethodType.Getter)]
        public static bool Prefix(ConstructionSiteAccessible __instance, ref int __result)
        {
            __result = __instance._blockObject.CoordinatesAtBaseZ.z - Config.BeaverArmsLength;
            return false;
        }

        /// <summary>
        /// Patch the NavigationDistance.Load to shrink the distric range
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(typeof(NavigationDistance), nameof(NavigationDistance.Load))]
        public static void Postfix(NavigationDistance __instance)
        {
            UdateDistrictRange(__instance, false);

            if (Config.District != null)
            {
                FieldInfo LimitedField = typeof(NavigationDistance).GetField("<Limited>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
                LimitedField.SetValue(__instance, Config.District.Pathfinding);
                FieldInfo ResourceBuildingsField = typeof(NavigationDistance).GetField("<ResourceBuildings>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
                ResourceBuildingsField.SetValue(__instance, Config.District.Resource);
                FieldInfo DistrictTerrainField = typeof(NavigationDistance).GetField("<DistrictTerrain>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
                DistrictTerrainField.SetValue(__instance, Config.District.Terrain);
            }
        }

        public static void UdateDistrictRange(NavigationDistance __instance, bool isDroughtEndedEvent)
        {
            int newRange = Config.District?.Range??70;

            if (Config.RangeModifier != null && Config.RangeModifier.Enabled)
            {
                var cycle = TimberApi.DependencyContainerSystem.DependencyContainer.GetInstance<WeatherService>().Cycle;

                var reductionsToApply = Config.RangeModifier.CyclesPerReduction == 0 ? 0 : (cycle / Config.RangeModifier.CyclesPerReduction) - 1;
                var droughtEndedOffByOne = isDroughtEndedEvent ? -1 : 0;
                var newValue = newRange - (reductionsToApply * Config.RangeModifier.ReductionAmount) + droughtEndedOffByOne;
                newRange = Math.Min(Math.Max(newValue, Config.RangeModifier.MinimumRange), Config.RangeModifier.MaximumRange);

                Log.LogInfo($"DistrictManagement. Cycle: {cycle} old range: {__instance.DistrictRoad} new range: {newValue}");
            }

            FieldInfo DistrictRoadField = typeof(NavigationDistance).GetField("<DistrictRoad>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            DistrictRoadField.SetValue(__instance, newRange);
        }
    }
}
