using Bindito.Core;
using System;
using System.Collections.Generic;
using System.Text;
using TimberApi.ConfiguratorSystem;
using TimberApi.EntityLinkerSystem;
using TimberApi.SceneSystem;
using Timberborn.Buildings;
using Timberborn.Growing;
using Timberborn.IrrigationSystem;
using Timberborn.TemplateSystem;

namespace TANSTAAFL.TIMBERBORN.DistrictShrinker
{
    [Configurator(SceneEntrypoint.InGame)]
    public class DistrictShrinkerConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<DroughtListener>().AsSingleton();
        }
    }
}
