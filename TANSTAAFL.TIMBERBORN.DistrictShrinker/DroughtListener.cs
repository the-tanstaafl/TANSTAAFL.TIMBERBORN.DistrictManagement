using Bindito.Core;
using Timberborn.Navigation;
using Timberborn.SingletonSystem;
using Timberborn.SoilMoistureSystem;
using Timberborn.WeatherSystem;

namespace TANSTAAFL.TIMBERBORN.DistrictShrinker
{
    public class DroughtListener : ILoadableSingleton
    {
        private EventBus _eventBus;

        [Inject]
        public void InjectDependencies(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Load()
        {
            _eventBus.Register(this);
        }

        [OnEvent]
        public void OnDroughtEnded(DroughtEndedEvent droughtEndedEvent)
        {
            var instance = TimberApi.DependencyContainerSystem.DependencyContainer.GetInstance<NavigationDistance>();
            DistrictShrinkerPlugin.UdateDistrictRange(instance, true);
        }
    }
}
