using System;
using System.Collections.Generic;
using System.Windows;
using log4net;

namespace WPF.RealTime.Shell
{
    public partial class App
    {
        private Bootstrapper _bootstrapper;
        private Dictionary<string, ResourceDictionary> _resourceDictionaries;
        private static readonly ILog Log;

        static App()
        {
            log4net.Config.XmlConfigurator.Configure();
            Log = LogManager.GetLogger(typeof(App));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            //dispose the disposables
        }

        public void ChangeTheme(string theme)
        {
            
            try
            {
                if ((theme == null) || _resourceDictionaries[theme] == null) return;

                Resources.MergedDictionaries.Clear();
                Resources.MergedDictionaries.Add(_resourceDictionaries[theme]);
                Log.Info(String.Format("Theme changed: {0}", theme));
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("Error ChangeTheme: {0}",ex));
            }
        }

        protected override void OnStartup(StartupEventArgs args)
        {
            base.OnStartup(args);

            //themes
            _resourceDictionaries = new Dictionary<string, ResourceDictionary>();
            _resourceDictionaries.Add(Constants.ThemeAero, new ResourceDictionary() 
            { Source = new Uri("pack://application:,,,/WPF.RealTime.ResourceLibrary;component/Aero/SharedDictionary.xaml") });
            _resourceDictionaries.Add(Constants.ThemeLuna, new ResourceDictionary() 
            { Source = new Uri("pack://application:,,,/WPF.RealTime.ResourceLibrary;component/Luna/SharedDictionary.xaml") });
            _resourceDictionaries.Add(Constants.ThemeRoyale, new ResourceDictionary() 
            { Source = new Uri("pack://application:,,,/WPF.RealTime.ResourceLibrary;component/Royale/SharedDictionary.xaml") });
                      
            // default theme
            Resources.MergedDictionaries.Add(_resourceDictionaries[Constants.ThemeAero]);

            try
            {
                ShutdownMode = ShutdownMode.OnMainWindowClose;
                _bootstrapper = new Bootstrapper();
                _bootstrapper.Run();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Oops", ex);
            }
        }
    }
}
