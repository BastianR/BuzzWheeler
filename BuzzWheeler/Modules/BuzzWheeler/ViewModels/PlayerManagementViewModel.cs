using BrElements.Classes;
using BuzzWheeler.AggregatorEvents;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace BuzzWheeler.ViewModels
{
    public class PlayerManagementViewModel : BindableBase
    {
        #region Fields

        private readonly IEventAggregator m_EventAggregator;
        private string m_EnteredName;
        private ObservableCollection<BrArcCircleItem> m_BrArcCircleItems;
        private bool m_WheelIsRunning;

        #endregion



        #region Properties

        public string EnteredName
        {
            get => m_EnteredName;
            set
            {
                SetProperty(ref m_EnteredName, value);
                AddPlayerCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<BrArcCircleItem> BrArcCircleItems
        {
            get => m_BrArcCircleItems;
            set => SetProperty(ref m_BrArcCircleItems, value);
        }

        public bool WheelIsRunning
        {
            get => m_WheelIsRunning;
            set
            {
                SetProperty(ref m_WheelIsRunning, value);
                RemovePlayerCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion



        #region Commands

        public DelegateCommand RemoveEnteredNameCommand { get; private set; }
        public DelegateCommand AddPlayerCommand { get; private set; }
        public DelegateCommand<string> RemovePlayerCommand { get; private set; }

        #endregion



        #region Constructor

        public PlayerManagementViewModel(IEventAggregator eventAggregator)
        {
            m_EventAggregator = eventAggregator;
            m_EventAggregator.GetEvent<MessageBrCircleIsRunningEvent>().Subscribe(asdd);

            m_BrArcCircleItems = new ObservableCollection<BrArcCircleItem>();

            RemoveEnteredNameCommand = new DelegateCommand(RemoveEnteredName);
            AddPlayerCommand = new DelegateCommand(AddPlayer, CanAddPlayer);
            RemovePlayerCommand = new DelegateCommand<string>(RemovePlayer, CanRemovePlayer);

            SendMessageBrCircleItems();
        }

        #endregion



        #region CommandMethods

        private void RemoveEnteredName()
        {
            EnteredName = "";
        }

        private bool CanAddPlayer()
        {
            bool canAddPlayer = false;

            if (m_EnteredName != null && m_EnteredName.Length > 0)
            {
                m_EnteredName = m_EnteredName.Trim();
            }

            bool checkNullOrEmpty = string.IsNullOrEmpty(EnteredName);
            bool checkRedundancy = BrArcCircleItems?.FirstOrDefault(brArcCircleItem => brArcCircleItem.Name == EnteredName) != null ? true : false;
            bool checkWheelIsRunning = WheelIsRunning;

            if (checkNullOrEmpty || checkRedundancy || checkWheelIsRunning)
            {
                canAddPlayer = false;
            }
            else
            {
                canAddPlayer = true;
            }

            return canAddPlayer;
        }

        private void AddPlayer()
        {
            BrArcCircleItems.Add(new BrArcCircleItem(EnteredName));
            EnteredName = "";
        }

        private bool CanRemovePlayer(string playerName)
        {
            return WheelIsRunning ? false : true;
        }

        private void RemovePlayer(string playerName)
        {
            BrArcCircleItem item = BrArcCircleItems?.FirstOrDefault(brArcCircleItem => brArcCircleItem.Name == playerName);

            if (item != null)
            {
                BrArcCircleItems.Remove(item);
            }
        }

        #endregion



        #region Private Methods

        private void SendMessageBrCircleItems()
        {
            m_EventAggregator.GetEvent<MessageBrCircleItemsEvent>().Publish(BrArcCircleItems);
        }

        private void asdd(bool wheelIsRunning)
        {
            WheelIsRunning = wheelIsRunning;
        }

        #endregion
    }
}
