using System;
using System.Collections.Generic;
using System.Text;
using TimberApi.ConfigSystem;

namespace TANSTAAFL.TIMBERBORN.DistrictManagement
{
    public class DistrictManagementConfig : IConfig
    {
        public DistrictManagementConfig() { }
        public DistrictManagementConfig(int beaverArmsLength, float resourceBuildingsRange, int buildersRange) 
        {
            BeaverArmsLength = beaverArmsLength;
            ResourceBuildingsRange = resourceBuildingsRange;
            BuildersRange = buildersRange;
        }

        public string ConfigFileName => "DistrictManagement";

        public int BeaverArmsLength = 1;
        public float ResourceBuildingsRange = 20;
        public int BuildersRange = 10;
    }
}
