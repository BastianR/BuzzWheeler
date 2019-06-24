using GenericUsbInput.CustomEventArgs;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenericUsbInput
{
    public class JoystickInput
    {
        #region Fields

        private DirectInput m_DirectInput;
        private List<Guid> m_InputGuids;
        private List<Joystick> m_InputDevices;
        private bool m_IsRunning;

        #endregion



        #region Constructor

        public JoystickInput()
        {
            m_DirectInput = new DirectInput();
            m_InputGuids = new List<Guid>();
            m_InputDevices = new List<Joystick>();

            Task getInputDeviceIdsTask = Task.Factory.StartNew(GetActiveGamepadsAndJoystickIds);
            getInputDeviceIdsTask.Wait();

            if (m_InputGuids.Count > 0)
            {
                Task getInputDeviceTask = Task.Factory.StartNew(GetAllActiveDevices);
                getInputDeviceTask.Wait();
            }

            if (m_InputDevices.Count > 0)
            {
                m_IsRunning = true;
                Task.Factory.StartNew(DevicePoolLoop);
            }
        }

        #endregion



        #region Private Methods

        private void GetActiveGamepadsAndJoystickIds()
        {
            foreach (DeviceInstance deviceInstance in m_DirectInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices))
            {
                Guid gamepadGuid = Guid.Empty;
                gamepadGuid = deviceInstance.InstanceGuid;
                if (gamepadGuid != Guid.Empty)
                {
                    m_InputGuids.Add(gamepadGuid);
                }
            }

            foreach (DeviceInstance deviceInstance in m_DirectInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
            {
                Guid joystickGuid = Guid.Empty;
                joystickGuid = deviceInstance.InstanceGuid;
                if (joystickGuid != Guid.Empty)
                {
                    m_InputGuids.Add(joystickGuid);
                }
            }
        }

        private void GetAllActiveDevices()
        {
            foreach (Guid deviceGuid in m_InputGuids)
            {
                Joystick joystick = new Joystick(m_DirectInput, deviceGuid);
                if (joystick != null)
                {
                    joystick.Properties.BufferSize = 128;
                    joystick.Acquire();
                    m_InputDevices.Add(joystick);
                }
            }
        }

        private void DevicePoolLoop()
        {
            while (m_IsRunning)
            {
                foreach (Joystick joystick in m_InputDevices)
                {
                    try
                    {
                        joystick.Poll();
                        JoystickUpdate[] joystickUpdates = joystick.GetBufferedData();
                        ReadJoystickBufferData(joystickUpdates);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error by reading device information");
                    }
                }
            }
        }

        private void ReadJoystickBufferData(JoystickUpdate[] joystickUpdates)
        {
            if (joystickUpdates.Length > 0)
            {
                foreach (JoystickUpdate joystickUpdate in joystickUpdates)
                {
                    switch (joystickUpdate.Offset)
                    {
                        case JoystickOffset.X:
                            JoystickOffsetX(joystickUpdate);
                            break;
                        case JoystickOffset.Y:
                            JoystickOffsetY(joystickUpdate);
                            break;
                        case JoystickOffset.Buttons0:
                            JoystickOffsetButtons0(joystickUpdate);
                            break;
                        case JoystickOffset.Buttons1:
                            JoystickOffsetButtons1(joystickUpdate);
                            break;
                        case JoystickOffset.Buttons2:
                            JoystickOffsetButtons2(joystickUpdate);
                            break;
                        case JoystickOffset.Buttons3:
                            JoystickOffsetButtons3(joystickUpdate);
                            break;
                        case JoystickOffset.Buttons4:
                            JoystickOffsetButtons4(joystickUpdate);
                            break;
                        case JoystickOffset.Buttons5:
                            JoystickOffsetButtons5(joystickUpdate);
                            break;
                        case JoystickOffset.Buttons6:
                            JoystickOffsetButtons6(joystickUpdate);
                            break;
                        case JoystickOffset.Buttons7:
                            JoystickOffsetButtons7(joystickUpdate);
                            break;
                        case JoystickOffset.Buttons8:
                            JoystickOffsetButtons8(joystickUpdate);
                            break;
                        case JoystickOffset.Buttons9:
                            JoystickOffsetButtons9(joystickUpdate);
                            break;
                        case JoystickOffset.Buttons10:
                            JoystickOffsetButtons10(joystickUpdate);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #endregion



        #region InputDevice Update Methods

        private void JoystickOffsetX(JoystickUpdate joystickUpdate)
        {
            if (joystickUpdate.Value < 32511)
            {
                InputDeviceHorizontalAxisLeftEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
            else if (joystickUpdate.Value > 32511)
            {
                InputDeviceHorizontalAxisRightEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
        }

        private void JoystickOffsetY(JoystickUpdate joystickUpdate)
        {
            if (joystickUpdate.Value < 32511)
            {
                InputDeviceVerticalAxisUpEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
            else if (joystickUpdate.Value > 32511)
            {
                InputDeviceVerticalAxisDownEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
        }

        private void JoystickOffsetButtons0(JoystickUpdate joystickUpdate)
        {
            if (joystickUpdate.Value == 0)
            {
                InputDeviceButton0UpEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
            else if (joystickUpdate.Value > 0)
            {
                InputDeviceButton0DownEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
        }

        private void JoystickOffsetButtons1(JoystickUpdate joystickUpdate)
        {
            if (joystickUpdate.Value == 0)
            {
                InputDeviceButton1UpEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
            else if (joystickUpdate.Value > 0)
            {
                InputDeviceButton1DownEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
        }

        private void JoystickOffsetButtons2(JoystickUpdate joystickUpdate)
        {
            if (joystickUpdate.Value == 0)
            {
                InputDeviceButton2UpEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
            else if (joystickUpdate.Value > 0)
            {
                InputDeviceButton2DownEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
        }

        private void JoystickOffsetButtons3(JoystickUpdate joystickUpdate)
        {
            if (joystickUpdate.Value == 0)
            {
                InputDeviceButton3UpEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
            else if (joystickUpdate.Value > 0)
            {
                InputDeviceButton3DownEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
        }

        private void JoystickOffsetButtons4(JoystickUpdate joystickUpdate)
        {
            if (joystickUpdate.Value == 0)
            {
                InputDeviceButton4UpEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
            else if (joystickUpdate.Value > 0)
            {
                InputDeviceButton4DownEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
        }

        private void JoystickOffsetButtons5(JoystickUpdate joystickUpdate)
        {
            if (joystickUpdate.Value == 0)
            {
                InputDeviceButton5UpEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
            else if (joystickUpdate.Value > 0)
            {
                InputDeviceButton5DownEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
        }

        private void JoystickOffsetButtons6(JoystickUpdate joystickUpdate)
        {
            if (joystickUpdate.Value == 0)
            {
                InputDeviceButton6UpEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
            else if (joystickUpdate.Value > 0)
            {
                InputDeviceButton6DownEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
        }

        private void JoystickOffsetButtons7(JoystickUpdate joystickUpdate)
        {
            if (joystickUpdate.Value == 0)
            {
                InputDeviceButton7UpEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
            else if (joystickUpdate.Value > 0)
            {
                InputDeviceButton7DownEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
        }

        private void JoystickOffsetButtons8(JoystickUpdate joystickUpdate)
        {
            if (joystickUpdate.Value == 0)
            {
                InputDeviceButton8UpEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
            else if (joystickUpdate.Value > 0)
            {
                InputDeviceButton8DownEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
        }

        private void JoystickOffsetButtons9(JoystickUpdate joystickUpdate)
        {
            if (joystickUpdate.Value == 0)
            {
                InputDeviceButton9UpEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
            else if (joystickUpdate.Value > 0)
            {
                InputDeviceButton9DownEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
        }

        private void JoystickOffsetButtons10(JoystickUpdate joystickUpdate)
        {
            if (joystickUpdate.Value == 0)
            {
                InputDeviceButton10UpEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
            else if (joystickUpdate.Value > 0)
            {
                InputDeviceButton10DownEvent?.Invoke(this, new JoystickValueEventArgs { Value = joystickUpdate.Value });
            }
        }

        #endregion



        #region InputDevice Events

        public event EventHandler InputDeviceHorizontalAxisLeftEvent;
        public event EventHandler InputDeviceHorizontalAxisRightEvent;
        public event EventHandler InputDeviceVerticalAxisDownEvent;
        public event EventHandler InputDeviceVerticalAxisUpEvent;
        public event EventHandler InputDeviceButton0DownEvent;
        public event EventHandler InputDeviceButton0UpEvent;
        public event EventHandler InputDeviceButton1DownEvent;
        public event EventHandler InputDeviceButton1UpEvent;
        public event EventHandler InputDeviceButton2DownEvent;
        public event EventHandler InputDeviceButton2UpEvent;
        public event EventHandler InputDeviceButton3DownEvent;
        public event EventHandler InputDeviceButton3UpEvent;
        public event EventHandler InputDeviceButton4DownEvent;
        public event EventHandler InputDeviceButton4UpEvent;
        public event EventHandler InputDeviceButton5DownEvent;
        public event EventHandler InputDeviceButton5UpEvent;
        public event EventHandler InputDeviceButton6DownEvent;
        public event EventHandler InputDeviceButton6UpEvent;
        public event EventHandler InputDeviceButton7DownEvent;
        public event EventHandler InputDeviceButton7UpEvent;
        public event EventHandler InputDeviceButton8DownEvent;
        public event EventHandler InputDeviceButton8UpEvent;
        public event EventHandler InputDeviceButton9DownEvent;
        public event EventHandler InputDeviceButton9UpEvent;
        public event EventHandler InputDeviceButton10DownEvent;
        public event EventHandler InputDeviceButton10UpEvent;

        #endregion
    }
}
