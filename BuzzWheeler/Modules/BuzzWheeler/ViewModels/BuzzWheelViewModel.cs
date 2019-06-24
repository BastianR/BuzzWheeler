using BrElements.Classes;
using BuzzWheeler.AggregatorEvents;
using GenericUsbInput;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Media;
using System.Threading.Tasks;

namespace BuzzWheeler.ViewModels
{
    public class BuzzWheelViewModel : BindableBase
    {
        #region Fields

        private readonly IEventAggregator m_EventAggregator;
        private ObservableCollection<BrArcCircleItem> m_BrArcCircleItems;
        private int m_BrSelectedArcCircleIndex;
        private bool m_WheelIsRunning;
        private SoundPlayer m_WheelLoopSound;
        private SoundPlayer m_WheelEndSound;
        private JoystickInput m_JoystickInput;

        #endregion



        #region Commands

        public DelegateCommand StartWheelCommand { get; private set; }

        #endregion



        #region Constructor

        public BuzzWheelViewModel(IEventAggregator eventAggregator)
        {
            m_EventAggregator = eventAggregator;
            StartWheelCommand = new DelegateCommand(StartWheel, CanStartWheel);
            SubscribeToEventAggregator();
            m_WheelLoopSound = new SoundPlayer(AppDomain.CurrentDomain.BaseDirectory + @"/Resources/Sound/LoopSound.wav");
            m_WheelEndSound = new SoundPlayer(AppDomain.CurrentDomain.BaseDirectory + @"/Resources/Sound/EndSound.wav");
            m_JoystickInput = new JoystickInput();
            m_JoystickInput.InputDeviceButton0DownEvent += OnJoystickInputButton0Down;
        }

        

        #endregion



        #region Properties

        public ObservableCollection<BrArcCircleItem> BrArcCircleItems
        {
            get => m_BrArcCircleItems;
            set => SetProperty(ref m_BrArcCircleItems, value);
        }

        public int BrSelectedArcCircleIndex
        {
            get => m_BrSelectedArcCircleIndex;
            set => SetProperty(ref m_BrSelectedArcCircleIndex, value);
        }

        public bool WheelIsRunning
        {
            get => m_WheelIsRunning;
            set
            {
                SetProperty(ref m_WheelIsRunning, value);
                StartWheelCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion



        #region Private Methods

        private void SubscribeToEventAggregator()
        {
            m_EventAggregator.GetEvent<MessageBrCircleItemsEvent>().Subscribe(ReceiveMessageBrCircleItems);
        }

        private void ReceiveMessageBrCircleItems(ObservableCollection<BrArcCircleItem> brArcCircleItems)
        {
            if (BrArcCircleItems != null)
            {
                BrArcCircleItems.CollectionChanged -= BrArcCircleItemsChanged;
            }

            BrArcCircleItems = brArcCircleItems;
            BrArcCircleItems.CollectionChanged += BrArcCircleItemsChanged;
        }

        

        private void StartTheWheel()
        {
            Task wheelTask = Task.Factory.StartNew(LoopThroughSelectedIndex);
        }

        private async Task LoopThroughSelectedIndex()
        {
            Random random = new Random();
            int randomLoopTimer = random.Next(50, 200);
            int currentLoopTimer = 0;
            int currentDelayTime = 50;
            int delayFaktor = 2;

            while (WheelIsRunning)
            {
                await Task.Delay(currentDelayTime);

                if (BrSelectedArcCircleIndex >= BrArcCircleItems.Count - 1)
                {
                    BrSelectedArcCircleIndex = 0;
                }
                else
                {
                    BrSelectedArcCircleIndex++;
                }
                
                if (currentLoopTimer < randomLoopTimer)
                {
                    currentLoopTimer++;
                    m_WheelLoopSound.Play();
                }
                else
                {
                    currentDelayTime += delayFaktor;
                    delayFaktor = (int)(Convert.ToDouble(delayFaktor) * 1.5);
                    m_WheelLoopSound.Play();
                }

                if (currentDelayTime >= 2000)
                {
                    WheelIsRunning = false;
                    m_WheelEndSound.Play();
                    SendMessageWheelIsRunning(WheelIsRunning);
                }
            }
        }

        private void SendMessageWheelIsRunning(bool wheelIsRunning)
        {
            m_EventAggregator.GetEvent<MessageBrCircleIsRunningEvent>().Publish(wheelIsRunning);
        }

        #endregion



        #region Command Methods

        private bool CanStartWheel()
        {
            return !WheelIsRunning && BrArcCircleItems?.Count > 0;
        }

        private void StartWheel()
        {
            WheelIsRunning = true;
            SendMessageWheelIsRunning(WheelIsRunning);
            StartTheWheel();
        }

        #endregion



        #region Event-Handler

        private void BrArcCircleItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            StartWheelCommand.RaiseCanExecuteChanged();
        }

        private void OnJoystickInputButton0Down(object sender, EventArgs e)
        {
            if (CanStartWheel())
            {
                StartWheel();
            }
        }

        #endregion
    }
}
