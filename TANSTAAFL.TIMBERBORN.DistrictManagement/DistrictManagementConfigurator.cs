using Bindito.Core;
using System;
using System.Collections.Generic;
using System.Text;
using TANSTAAFL.TIMBERBORN.DistrictManagement.Config;
using TimberApi.ConfiguratorSystem;
using TimberApi.EntityLinkerSystem;
using TimberApi.SceneSystem;
using Timberborn.Buildings;
using Timberborn.CameraSystem;
using Timberborn.Growing;
using Timberborn.IrrigationSystem;
using Timberborn.TemplateSystem;

namespace TANSTAAFL.TIMBERBORN.DistrictManagement
{
    [Configurator(SceneEntrypoint.InGame)]
    public class DistrictManagementConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<DistrictManagementConfigLoader>().AsSingleton();
            containerDefinition.Bind<DistrictManagementConfigSerializer>().AsSingleton();
            containerDefinition.Bind<DistrictManagementConfigBox>().AsSingleton();
        }
    }
}
