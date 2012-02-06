using System.Windows;
using System.Windows.Forms;

namespace WPF.RealTime.Infrastructure.AttachedProperty
{
    public class WindowProperties 
    {
        private static readonly int _height;
        private static readonly int _width;
        private const double DivineProportion = 1.618;

        static WindowProperties()
        {
            _height = Screen.PrimaryScreen.WorkingArea.Height;
            _width = Screen.PrimaryScreen.WorkingArea.Width;
            OneQuarterHeight = _height / 4;
            OneThirdHeight = _height / 3;
            TwoThirdsHeight = 2 * _height / 3;
            TwoQuarterHeight = 2 * OneQuarterHeight;
            ThreeQuarterHeight = 3 * OneQuarterHeight;
            TotalHeight = _height;
            OneHalfHeight = _height / 2;
            OneHalfHeightNoBorder = OneHalfHeight - 50;
            TotalWidth = _width;
            HalfWidth = _width / 2;
        }
        public static readonly int OneThirdHeight;
        public static readonly int OneHalfHeight;
        public static readonly double OneHalfHeightNoBorder;
        public static readonly int TwoThirdsHeight;
        public static readonly int OneQuarterHeight;
        public static readonly int TwoQuarterHeight;
        public static readonly int ThreeQuarterHeight;
        public static readonly int TotalHeight;
        public static readonly int TotalWidth;
        public static readonly int HalfWidth;

        public static readonly DependencyProperty
        LeftProperty = DependencyProperty.RegisterAttached("Left", typeof(int), typeof(WindowProperties));

        public static int GetLeft(DependencyObject target)
        {
            return (int)target.GetValue(LeftProperty);
        }

        public static void SetLeft(DependencyObject target, int value)
        {
            target.SetValue(LeftProperty, value);
        }

        public static readonly DependencyProperty
        TopProperty = DependencyProperty.RegisterAttached("Top", typeof(int), typeof(WindowProperties));

        public static int GetTop(DependencyObject target)
        {
            return (int)target.GetValue(TopProperty);
        }

        public static void SetTop(DependencyObject target, int value)
        {
            target.SetValue(TopProperty, value);
        }

        public static readonly DependencyProperty
        WidthProperty = DependencyProperty.RegisterAttached("Width", typeof(int), typeof(WindowProperties));

        public static int GetWidth(DependencyObject target)
        {
            return (int)target.GetValue(WidthProperty);
        }

        public static void SetWidth(DependencyObject target, int value)
        {
            target.SetValue(WidthProperty, value);
        }

        public static readonly DependencyProperty
        HeightProperty = DependencyProperty.RegisterAttached("Height", typeof(int), typeof(WindowProperties));

        public static int GetHeight(DependencyObject target)
        {
            return (int)target.GetValue(HeightProperty);
        }

        public static void SetHeight(DependencyObject target, int value)
        {
            target.SetValue(HeightProperty, value);
        }
    }
}
