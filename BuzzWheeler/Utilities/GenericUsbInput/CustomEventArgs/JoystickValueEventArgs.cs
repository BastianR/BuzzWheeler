using System;

namespace GenericUsbInput.CustomEventArgs
{
    public class JoystickValueEventArgs : EventArgs
    {
        public int Value { get; set; }
    }
}
