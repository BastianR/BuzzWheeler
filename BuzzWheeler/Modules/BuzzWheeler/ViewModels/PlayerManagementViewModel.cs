using BrElements.Classes;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace BuzzWheeler.ViewModels
{
    public class PlayerManagementViewModel : BindableBase
    {
        #region Fields

        private ObservableCollection<BrArcCircleItem> m_BrArcCircleItems;

        #endregion



        #region Properties

        public ObservableCollection<BrArcCircleItem> BrArcCircleItems
        {
            get => m_BrArcCircleItems;
            set => SetProperty(ref m_BrArcCircleItems, value);
        }

        #endregion



        #region Commands

        public DelegateCommand AddPlayerCommand { get; private set; }
        public DelegateCommand<string> RemovePlayerCommand { get; private set; }

        #endregion



        #region Constructor

        public PlayerManagementViewModel()
        {
            m_BrArcCircleItems = new ObservableCollection<BrArcCircleItem>();

            AddPlayerCommand = new DelegateCommand(AddPlayer);
            RemovePlayerCommand = new DelegateCommand<string>(RemovePlayer);
        }

        #endregion



        #region CommandMethods

        private void AddPlayer()
        {

        }

        private void RemovePlayer(string playerName)
        {

        }

        #endregion
    }
}
