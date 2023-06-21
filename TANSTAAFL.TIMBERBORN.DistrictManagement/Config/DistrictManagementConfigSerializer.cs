using System;
using System.Collections.Generic;
using System.Text;
using Timberborn.CameraSystem;
using Timberborn.Persistence;

namespace TANSTAAFL.TIMBERBORN.DistrictManagement.Config
{
    public class DistrictManagementConfigSerializer : IObjectSerializer<DistrictManagementConfig>
    {
        private static readonly PropertyKey<int> BeaverArmsLengthKey = new PropertyKey<int>("BeaverArmsLength");
        private static readonly PropertyKey<float> ResourceBuildingsRangeKey = new PropertyKey<float>("ResourceBuildingsRange");
        private static readonly PropertyKey<int> BuildersRangeKey = new PropertyKey<int>("BuildersRange");

        public void Serialize(DistrictManagementConfig value, IObjectSaver objectSaver)
        {
            objectSaver.Set(BeaverArmsLengthKey, value.BeaverArmsLength);
            objectSaver.Set(ResourceBuildingsRangeKey, value.ResourceBuildingsRange);
            objectSaver.Set(BuildersRangeKey, value.BuildersRange);
        }

        public Obsoletable<DistrictManagementConfig> Deserialize(IObjectLoader objectLoader)
        {
            return new DistrictManagementConfig(objectLoader.Get(BeaverArmsLengthKey), objectLoader.Get(ResourceBuildingsRangeKey), objectLoader.Get(BuildersRangeKey));
        }
    }
}
