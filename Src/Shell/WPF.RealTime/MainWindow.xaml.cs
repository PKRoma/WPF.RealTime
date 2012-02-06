using System;
using System.Windows;
using WPF.RealTime.Infrastructure.AttachedProperty;

namespace WPF.RealTime.Shell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();

            Left = Convert.ToDouble(GetValue(WindowProperties.LeftProperty));
            Top = Convert.ToDouble(GetValue(WindowProperties.TopProperty));
            Width = Convert.ToDouble(GetValue(WindowProperties.WidthProperty));
            Height = Convert.ToDouble(GetValue(WindowProperties.HeightProperty));
        }
    }
}
