using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TimberApi.ConfigSystem;
using Timberborn.CameraSystem;
using Timberborn.Navigation;
using Timberborn.Persistence;
using Timberborn.SingletonSystem;

namespace TANSTAAFL.TIMBERBORN.DistrictManagement.Config
{
    public class DistrictManagementConfigLoader : ISaveableSingleton, ILoadableSingleton, IPostLoadableSingleton
    {
        private static readonly SingletonKey DistrictManagementStateRestorerKey = new SingletonKey("DistrictManagementStateRestorer");
        private static readonly PropertyKey<DistrictManagementConfig> SavedDistrictManagementStateKey = new PropertyKey<DistrictManagementConfig>("DistrictManagementState");

        private NavigationDistance _navigationDistance;
        internal static DistrictManagementConfig _savedConfig;
        private DistrictManagementConfigSerializer _configSerializer;

        private readonly ISingletonLoader _singletonLoader;

        public DistrictManagementConfigLoader(NavigationDistance navigationDistance, ISingletonLoader singletonLoader, DistrictManagementConfigSerializer configSerializer)
        {
            _navigationDistance = navigationDistance;
            _singletonLoader = singletonLoader;
            _configSerializer = configSerializer;
        }

        public void Load()
        {
            _savedConfig = null;
            if (_singletonLoader.HasSingleton(DistrictManagementStateRestorerKey))
            {
                IObjectLoader singleton = _singletonLoader.GetSingleton(DistrictManagementStateRestorerKey);
                _savedConfig = singleton.Get(SavedDistrictManagementStateKey, _configSerializer);
            }
        }

        public void PostLoad()
        {
            if (_savedConfig == null)
            {
                _savedConfig = new DistrictManagementConfig(1, _navigationDistance.ResourceBuildings, _navigationDistance.DistrictTerrain);
            }

            ApplyConfigs(_navigationDistance);
        }

        public void Save(ISingletonSaver singletonSaver)
        {
            singletonSaver.GetSingleton(DistrictManagementStateRestorerKey).Set(SavedDistrictManagementStateKey, _savedConfig, _configSerializer);
        }

        public static void ApplyConfigs(NavigationDistance navigationDistance)
        {
            FieldInfo ResourceBuildingsField = typeof(NavigationDistance).GetField("<ResourceBuildings>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            ResourceBuildingsField.SetValue(navigationDistance, _savedConfig.ResourceBuildingsRange);
            FieldInfo DistrictTerrainField = typeof(NavigationDistance).GetField("<DistrictTerrain>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            DistrictTerrainField.SetValue(navigationDistance, _savedConfig.BuildersRange);
        }
    }
}
