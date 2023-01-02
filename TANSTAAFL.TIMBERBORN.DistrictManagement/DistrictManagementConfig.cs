using System;
using System.Collections.Generic;
using System.Text;
using TimberApi.ConfigSystem;

namespace TANSTAAFL.TIMBERBORN.DistrictManagement
{
    public class DistrictManagementConfig : IConfig
    {
        public DistrictManagementConfig()
        {
            District = new DistrictConfig();
            RangeModifier = new RangeModifierConfig();
        }

        public string ConfigFileName => "DistrictManagement";

        public int BeaverArmsLength = 1;

        public DistrictConfig District { get; set; }
        public RangeModifierConfig RangeModifier { get; set; }

        public class RangeModifierConfig
        {
            public bool Enabled = false;
            public int ReductionAmount = 1;
            public int CyclesPerReduction = 1;
            public int MinimumRange = 35;
            public int MaximumRange = 125;
        }

        public class DistrictConfig
        {
            public int Range = 70;
            public int Pathfinding = 55;
            public int Resource = 20;
            public int Terrain = 10;
        }
    }
}
