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
using Timberborn.CoreUI;
using Timberborn.EditorStarter;
using Timberborn.EntitySystem;
using Timberborn.Growing;
using Timberborn.IrrigationSystem;
using Timberborn.MapIndexSystem;
using Timberborn.NaturalResourcesModelSystem;
using Timberborn.NaturalResourcesReproduction;
using Timberborn.Navigation;
using Timberborn.Options;
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
        internal static IConsoleWriter Log;

        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            Log = consoleWriter;

            var harmony = new Harmony("tanstaafl.plugins.DistrictManagement");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(ConstructionSiteAccessible), nameof(ConstructionSiteAccessible.MinZ), MethodType.Getter)]
        public static bool Prefix(ConstructionSiteAccessible __instance, ref int __result)
        {
            __result = __instance._blockObject.CoordinatesAtBaseZ.z - (DistrictManagement._savedConfig?.BeaverArmsLength??1);
            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(OptionsBox), "GetPanel")]
        static void ShowConfigBox(ref VisualElement __result)
        {
            VisualElement root = __result.Query("OptionsBox");
            Button button = new() { classList = { "menu-button" } };

            button.text = "DistrictManagement config";
            button.clicked += ConfigBox.OpenOptionsDelegate;
            root.Insert(4, button);
        }
    }
}
