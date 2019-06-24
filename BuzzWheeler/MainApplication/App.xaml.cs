using CommonServiceLocator;
using MainApplication.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using Support.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace MainApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        #region Protected Methods

        protected override async void OnStartup(StartupEventArgs e)
        {
            Logger.Info("[App.xaml.cs] On startup");
            base.OnStartup(e);

            Logger.Info("[App.xaml.cs] Show splashscreen");
            await ShowSplashScreen();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            Logger.Info("[App.xaml.cs] Creating module catalog");
            return new DirectoryModuleCatalog() { ModulePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@".\Libraries") };
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            Logger.Info("[App.xaml.cs] Registering types");
        }

        protected override Window CreateShell()
        {
            Logger.Info("[App.xaml.cs] Creating shell");
            return ServiceLocator.Current.GetInstance<Shell>();
        }

        #endregion



        #region Private Methods

        private async Task ShowSplashScreen()
        {
            SplashScreenView splashScreenView = new SplashScreenView();

            Task splashTask = Task.Factory.StartNew(() =>
            {
                Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    splashScreenView.Show();
                }));
            });

            await Task.Delay(MainApplication.Properties.Settings.Default.SplashScreenTime);

            splashTask = Task.Factory.StartNew(() =>
            {
                Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    splashScreenView.Close();
                }));
            });
        }

        #endregion
    }
}
