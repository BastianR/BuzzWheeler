using Prism.Mvvm;

namespace BrElements.Classes
{
    public class BrArcCircleItem : BindableBase
    {
        #region Fields

        private string m_Name;

        #endregion



        #region Constructor

        public BrArcCircleItem(string name)
        {
            Name = name;
        }

        #endregion



        #region Properties

        public string Name
        {
            get => m_Name;
            set => SetProperty(ref m_Name, value);
        }

        #endregion
    }
}
