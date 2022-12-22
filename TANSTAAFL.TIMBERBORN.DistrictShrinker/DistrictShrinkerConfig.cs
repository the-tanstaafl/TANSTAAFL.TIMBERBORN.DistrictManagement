using System;
using System.Collections.Generic;
using System.Text;
using TimberApi.ConfigSystem;

namespace TANSTAAFL.TIMBERBORN.DistrictShrinker
{
    public class DistrictShrinkerConfig : IConfig
    {
        public string ConfigFileName => "DistrictShrinker";

        public int StartingRange = 75;
        public int ReductionAmount = 1;
        public int CyclesPerReduction = 1;
        public int MinimumRange = 35;
        public int MaximumRange = 125;
    }
}
