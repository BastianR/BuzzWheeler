using BrElements.Classes;
using Microsoft.Expression.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace BrElements.Controls
{
    public class BrArcCircle : Control
    {
        #region Fields

        private const string partCircleGridName = "PART_CircleGrid";
        private const string partMiddleTextBlock = "PART_MiddleTextBlock";
        private Grid m_Grid;
        private TextBlock m_TextBlock;
        private Style m_ArcStyle;
        private Style m_ArcSelectedStyle;
        private Style m_BrTextBlockStyle;
        private Style m_BrTextBlockSelectedStyle;
        private double m_CalculatedArcWidth;
        private double m_CalculatedCurrentStartAngle;
        private double m_CalculatedTextDiameter;
        private double m_CalculatedDiameter;
        private int m_OldSelectedItemIndex;

        #endregion



        #region Dependency Properties

        public static readonly DependencyProperty BrItemsSourceProperty = 
            DependencyProperty.Register
            (
                "BrItemsSource", 
                typeof(ObservableCollection<BrArcCircleItem>), 
                typeof(BrArcCircle),
                new FrameworkPropertyMetadata
                (
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnBrItemsSourceChanged),
                    new CoerceValueCallback(OnBrItemsSourceCoerce)
                )
            );

        public static readonly DependencyProperty BrSelectedItemIndexProperty =
            DependencyProperty.Register
            (
                "BrSelectedItemIndex",
                typeof(int),
                typeof(BrArcCircle),
                new FrameworkPropertyMetadata
                (
                    0,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnBrSelectedItemIndexChanged),
                    new CoerceValueCallback(OnBrSelectedItemIndexCoerce)
                )
            );

        public static readonly DependencyProperty CircleStartAngleProperty =
            DependencyProperty.Register
            (
                "CircleStartAngle", 
                typeof(double), 
                typeof(BrArcCircle), 
                new FrameworkPropertyMetadata
                (
                    0.0d,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnCircleStartAngleChanged),
                    new CoerceValueCallback(OnCircleStartAngleCoerce)
                )
            );

        public static readonly DependencyProperty CircleEndAngleProperty =
            DependencyProperty.Register
            (
                "CircleEndAngle", 
                typeof(double), 
                typeof(BrArcCircle),
                new FrameworkPropertyMetadata
                (
                    360.0d,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnCircleEndAngleChanged),
                    new CoerceValueCallback(OnCircleEndAngleCoerce)
                )
            );

        public static readonly DependencyProperty ArcGapProperty =
            DependencyProperty.Register
            (
                "ArcGap",
                typeof(double),
                typeof(BrArcCircle),
                new FrameworkPropertyMetadata
                (
                    5.0d,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnArcGapChanged),
                    new CoerceValueCallback(OnArcGapCoerce)
                )
            );

        #endregion



        #region Dependency Property Wrapper

        public ObservableCollection<BrArcCircleItem> BrItemsSource
        {
            get { return (ObservableCollection<BrArcCircleItem>)GetValue(BrItemsSourceProperty); }
            set { SetValue(BrItemsSourceProperty, value); }
        }

        public int BrSelectedItemIndex
        {
            get { return (int)GetValue(BrSelectedItemIndexProperty); }
            set { SetValue(BrSelectedItemIndexProperty, value); }
        }

        public double CircleStartAngle
        {
            get { return (double)GetValue(CircleStartAngleProperty); }
            set { SetValue(CircleStartAngleProperty, value); }
        }

        public double CircleEndAngle
        {
            get { return (double)GetValue(CircleEndAngleProperty); }
            set { SetValue(CircleEndAngleProperty, value); }
        }

        public double ArcGap
        {
            get { return (double)GetValue(ArcGapProperty); }
            set { SetValue(ArcGapProperty, value); }
        }

        #endregion



        #region Constructor

        static BrArcCircle()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BrArcCircle), new FrameworkPropertyMetadata(typeof(BrArcCircle)));
        }

        public BrArcCircle()
        {
            BrItemsSource = new ObservableCollection<BrArcCircleItem>();
        }

        #endregion



        #region Override Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            m_Grid = GetTemplateChild(partCircleGridName) as Grid;
            m_Grid.SizeChanged += OnPartCircleGridSizeChanged;
            m_TextBlock = GetTemplateChild(partMiddleTextBlock) as TextBlock;

            m_ArcStyle = m_Grid.Resources["BrArcStyle"] as Style;
            m_ArcSelectedStyle = m_Grid.Resources["BrArcSelectedStyle"] as Style;
            m_BrTextBlockStyle = m_Grid.Resources["BrTextBlockStyle"] as Style;
            m_BrTextBlockSelectedStyle = m_Grid.Resources["BrTextBlockSelectedStyle"] as Style;
        }

        #endregion



        #region Property Changed Callback Handler

        private static void OnBrItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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

        private static void OnBrSelectedItemIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BrArcCircle brArcCircle)
            {
                int oldValue = (int)e.OldValue;
                int newValue = (int)e.NewValue;

                brArcCircle.OnBrSelectedItemIndexChanged(oldValue, newValue);
            }
        }

        private static void OnCircleStartAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BrArcCircle brArcCircle)
            {
                double oldValue = (double)e.OldValue;
                double newValue = (double)e.NewValue;

                brArcCircle.OnCircleStartAngleChanged(oldValue, newValue);
            }
        }

        private static void OnCircleEndAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BrArcCircle brArcCircle)
            {
                double oldValue = (double)e.OldValue;
                double newValue = (double)e.NewValue;

                brArcCircle.OnCircleEndAngleChanged(oldValue, newValue);
            }
        }

        private static void OnArcGapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // todo
        }

        #endregion



        #region Property Changed Callback Methods

        protected virtual void OnItemsSourceChanged(ObservableCollection<BrArcCircleItem> oldValue, ObservableCollection<BrArcCircleItem> newValue)
        {
            if (oldValue != null)
            {
                BrItemsSource.CollectionChanged -= OnItemsSourceCollectionChanged;
            }

            BrItemsSource = newValue;
            BrItemsSource.CollectionChanged += OnItemsSourceCollectionChanged;
        }

        protected virtual void OnBrSelectedItemIndexChanged(int oldValue, int newValue)
        {
            if (oldValue != newValue && BrItemsSource?.Count > 0)
            {
                m_OldSelectedItemIndex = oldValue;

                if (newValue < 0)
                {
                    BrSelectedItemIndex = 0;
                }
                else if (newValue > BrItemsSource.Count - 1)
                {
                    BrSelectedItemIndex = BrItemsSource.Count - 1;
                }
                else
                {
                    BrSelectedItemIndex = newValue;
                }
                
                ChangeSelectedItemStyles();
            }
        }

        protected virtual void OnCircleStartAngleChanged(double oldValue, double newValue)
        {
            if (newValue < CircleEndAngle)
            {
                CircleStartAngle = newValue;
            }
            else
            {
                CircleStartAngle = 0.0f;
            }
        }

        protected virtual void OnCircleEndAngleChanged(double oldValue, double newValue)
        {
            if (newValue > CircleStartAngle)
            {
                CircleEndAngle = newValue;
            }
            else
            {
                CircleEndAngle = 360.0f;
            }
        }

        #endregion



        #region Property Coerce Callback Methods

        private static object OnBrItemsSourceCoerce(DependencyObject d, object baseValue)
        {
            return baseValue;
        }

        private static object OnBrSelectedItemIndexCoerce(DependencyObject d, object baseValue)
        {
            return baseValue;
        }

        private static object OnCircleStartAngleCoerce(DependencyObject d, object baseValue)
        {
            double value = 0.0f;

            try
            {
                value = CheckAngleLimits(Convert.ToDouble(baseValue));
            }
            catch (Exception exception)
            {
                // todo Logging
            }
            
            return value;
        }

        private static object OnCircleEndAngleCoerce(DependencyObject d, object baseValue)
        {
            double value = 360.0f;

            try
            {
                value = CheckAngleLimits(Convert.ToDouble(baseValue));
            }
            catch (Exception exception)
            {
                // todo Logging
            }

            return value;
        }

        private static object OnArcGapCoerce(DependencyObject d, object baseValue)
        {
            return baseValue;
        }

        #endregion



        #region Eventhandler Methods

        private void OnItemsSourceCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CheckBrItemsSourceForCalculation();
        }

        #endregion



        #region Private Static Methods

        private static double CheckAngleLimits(double value)
        {
            double returnValue = 0.0d;

            if (value < 0.0d)
            {
                returnValue = 0.0d;
            }
            else if (value > 360.0d)
            {
                returnValue = 360.0d;
            }

            return returnValue;
        }

        #endregion



        #region Private Methods

        private void CheckBrItemsSourceForCalculation()
        {
            if (BrItemsSource?.Count > 0)
            {
                CalculateBaseParameter();
                CreateArcCircleItems();
                ClearGridOfBrArcCircleItems();
                FillGridWithBrArcCircleItems();
                ChangeSelectedItemStyles();
            }
            else
            {
                ClearGridOfBrArcCircleItems();
                ChangeMiddleTextBlockText("");
            }
        }

        private void CalculateBaseParameter()
        {
            double totalCircleWidth = CircleEndAngle - CircleStartAngle;
            m_CalculatedArcWidth = (totalCircleWidth / BrItemsSource.Count) - ArcGap;
            m_CalculatedCurrentStartAngle = ArcGap / 2;
            m_CalculatedDiameter = m_Grid.ActualWidth < m_Grid.ActualHeight ? m_Grid.ActualWidth : m_Grid.ActualHeight;
            m_CalculatedTextDiameter = m_CalculatedDiameter - 50;
        }

        private void CreateArcCircleItems()
        {
            foreach (BrArcCircleItem brArcCircleItem in BrItemsSource)
            {
                brArcCircleItem.Arc = new Arc
                {
                    StartAngle = m_CalculatedCurrentStartAngle,
                    EndAngle = m_CalculatedCurrentStartAngle + m_CalculatedArcWidth,
                    Width = m_CalculatedDiameter,
                    Height = m_CalculatedDiameter
                };

                brArcCircleItem.Arc.Style = m_ArcStyle;

                double arcMiddleAngle = m_CalculatedCurrentStartAngle + (m_CalculatedArcWidth / 2);
                arcMiddleAngle = arcMiddleAngle > 360.0d ? arcMiddleAngle %= 360 : arcMiddleAngle;

                brArcCircleItem.TextCoordinateX = Convert.ToInt32(Math.Round(m_CalculatedTextDiameter * Math.Sin(arcMiddleAngle * Math.PI / 180), MidpointRounding.AwayFromZero));
                brArcCircleItem.TextCoordinateY = Convert.ToInt32(Math.Round(-1 * m_CalculatedTextDiameter * Math.Cos(arcMiddleAngle * Math.PI / 180), MidpointRounding.AwayFromZero));

                brArcCircleItem.TextBlock = new TextBlock
                {
                    Text = brArcCircleItem.Name[0].ToString(),
                    Margin = new Thickness(brArcCircleItem.TextCoordinateX, brArcCircleItem.TextCoordinateY, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Foreground = Brushes.White,
                    Style = m_BrTextBlockStyle
                };

                m_CalculatedCurrentStartAngle += m_CalculatedArcWidth + ArcGap;
            }
        }

        private void ClearGridOfBrArcCircleItems()
        {
            if (m_Grid != null)
            {
                List<UIElement> uIElements = new List<UIElement>();

                foreach (UIElement uIElement in m_Grid.Children)
                {
                    if (uIElement.GetType() == typeof(Arc))
                    {
                        uIElements.Add(uIElement);
                    }

                    if (uIElement.GetType() == typeof(TextBlock))
                    {
                        TextBlock textBlock = uIElement as TextBlock;

                        if (textBlock.Name != partMiddleTextBlock)
                        {
                            uIElements.Add(uIElement);
                        }
                    }
                }

                foreach (UIElement uIElement in uIElements)
                {
                    m_Grid.Children.Remove(uIElement);
                }
            }
        }

        private void FillGridWithBrArcCircleItems()
        {
            if (m_Grid != null)
            {
                foreach (BrArcCircleItem brArcCircleItem in BrItemsSource)
                {
                    m_Grid.Children.Add(brArcCircleItem.Arc);
                    m_Grid.Children.Add(brArcCircleItem.TextBlock);
                }
            }
        }

        private void ChangeSelectedItemStyles()
        {
            if (m_OldSelectedItemIndex != BrSelectedItemIndex)
            {
                Console.WriteLine(m_OldSelectedItemIndex);
                Console.WriteLine(BrSelectedItemIndex);
                Console.WriteLine("---");
                BrItemsSource[m_OldSelectedItemIndex].Arc.Style = m_ArcStyle;
                BrItemsSource[m_OldSelectedItemIndex].TextBlock.Style = m_BrTextBlockStyle;
                BrItemsSource[BrSelectedItemIndex].Arc.Style = m_ArcSelectedStyle;
                BrItemsSource[BrSelectedItemIndex].TextBlock.Style = m_BrTextBlockSelectedStyle;

                ChangeMiddleTextBlockText(BrItemsSource[BrSelectedItemIndex].Name);
            }
            else
            {
                Console.WriteLine(100);
                Console.WriteLine(BrSelectedItemIndex);
                BrItemsSource[BrSelectedItemIndex].Arc.Style = m_ArcSelectedStyle;
                BrItemsSource[BrSelectedItemIndex].TextBlock.Style = m_BrTextBlockSelectedStyle;

                ChangeMiddleTextBlockText(BrItemsSource[BrSelectedItemIndex].Name);
            }
        }

        private void ChangeMiddleTextBlockText(string text)
        {
            m_TextBlock.Text = text;
        }

        #endregion



        #region Event-Handler

        private void OnPartCircleGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            CheckBrItemsSourceForCalculation();
        }

        #endregion
    }
}
