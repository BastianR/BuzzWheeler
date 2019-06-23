using Microsoft.Expression.Shapes;
using Prism.Mvvm;
using System.Windows.Controls;

namespace BrElements.Classes
{
    public class BrArcCircleItem : BindableBase
    {
        #region Fields

        private string m_Name;
        private Arc m_Arc;
        private TextBlock m_TextBlock;
        private int m_TextCoordinateX;
        private int m_TextCoordinateY;

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

        public Arc Arc
        {
            get => m_Arc;
            set => SetProperty(ref m_Arc, value);
        }

        public TextBlock TextBlock
        {
            get => m_TextBlock;
            set => SetProperty(ref m_TextBlock, value);
        }

        public int TextCoordinateX
        {
            get => m_TextCoordinateX;
            set => SetProperty(ref m_TextCoordinateX, value);
        }

        public int TextCoordinateY
        {
            get => m_TextCoordinateY;
            set => SetProperty(ref m_TextCoordinateY, value);
        }

        #endregion
    }
}
