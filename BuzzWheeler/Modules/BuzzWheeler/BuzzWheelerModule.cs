using BuzzWheeler.Contracts;
using BuzzWheeler.Views;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace BuzzWheeler
{
    public class BuzzWheelerModule : IModule
    {
        #region Fields

        private readonly IEventAggregator m_EventAggregator;

        #endregion



        #region Constructor

        public BuzzWheelerModule(IEventAggregator eventAggregator)
        {
            m_EventAggregator = eventAggregator;
        }

        #endregion



        #region Interface Implementations

        public void OnInitialized(IContainerProvider containerProvider)
        {
            IRegionManager regionManager = containerProvider.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion(ConstantNames.RegionPlayerManagementView, typeof(PlayerManagementView));
            regionManager.RegisterViewWithRegion(ConstantNames.RegionBuzzWheelView, typeof(BuzzWheelView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry) { }

        #endregion
    }
}
