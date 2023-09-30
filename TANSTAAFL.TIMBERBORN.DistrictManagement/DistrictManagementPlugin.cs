using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TANSTAAFL.TIMBERBORN.DistrictManagement.Config;
using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;
using Timberborn.BuildingsNavigation;
using Timberborn.OptionsGame;
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
            __result = __instance._blockObject.CoordinatesAtBaseZ.z - (DistrictManagementConfigLoader._savedConfig?.BeaverArmsLength??1);
            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameOptionsBox), "GetPanel")]
        static void ShowConfigBox(ref VisualElement __result)
        {
            VisualElement root = __result.Query("Game/GameOptionsBox");
            Button button = new() { classList = { "menu-button" } };

            button.text = "DistrictManagement config";
            button.clicked += DistrictManagementConfigBox.OpenOptionsDelegate;
            root.Insert(4, button);
        }
    }
}
