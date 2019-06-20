using BrElements.Classes;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace BrElements.Controls
{
    public class BrArcCircle : Control
    {

        #region Constructor

        static BrArcCircle()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BrArcCircle), new FrameworkPropertyMetadata(typeof(BrArcCircle)));
        }

        public BrArcCircle()
        {
            ItemsSource = new ObservableCollection<BrArcCircleItem>();
            ItemsSource.CollectionChanged += OnItemsSourceCollectionChanged;
        }

        #endregion



        #region Dependency Properties

        public static readonly DependencyProperty ItemsSourceProperty = 
            DependencyProperty.Register
            (
                "ItemsSource", 
                typeof(ObservableCollection<BrArcCircleItem>), 
                typeof(BrArcCircle),
                new FrameworkPropertyMetadata
                (
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnItemsSourceChanged),
                    new CoerceValueCallback(OnItemsSourceCoerce)
                )
            );

        #endregion



        #region Dependency Property Wrapper

        public ObservableCollection<BrArcCircleItem> ItemsSource
        {
            get
            {
                return (ObservableCollection<BrArcCircleItem>)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        #endregion



        #region Property Changed Callback Handler

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BrArcCircle brArcCircle)
            {
                ObservableCollection<BrArcCircleItem> oldValue = (ObservableCollection<BrArcCircleItem>)e.OldValue;
                ObservableCollection<BrArcCircleItem> newValue = (ObservableCollection<BrArcCircleItem>)e.NewValue;

                if (newValue != null)
                {
                    brArcCircle.OnItemsSourceChanged(oldValue, newValue);
                }
            }
        }

        #endregion



        #region Property Changed Callback Methods

        protected virtual void OnItemsSourceChanged(ObservableCollection<BrArcCircleItem> oldValue, ObservableCollection<BrArcCircleItem> newValue)
        {
            ItemsSource = newValue;
        }

        #endregion



        #region Property Coerce Callback Methods

        private static object OnItemsSourceCoerce(DependencyObject d, object baseValue)
        {
            return baseValue;
        }

        #endregion



        #region Eventhandler Methods

        private void OnItemsSourceCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // todo
        }

        #endregion
    }
}
