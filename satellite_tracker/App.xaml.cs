using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;
using satellite_tracker.ViewModels;
using System.Windows;
using System.Windows.Threading;

namespace satellite_tracker
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            LogManager.Info("========== Application Start ==========");

#if !DEBUG
            Current.DispatcherUnhandledException += OnDispatcherUnhandledException;
#endif

            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddSingleton<IDialogService, DialogService>()
                    .AddTransient<MainWindowViewModel>()
                    .BuildServiceProvider());

            GlobalData.Default.Initialize();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            GlobalData.Default.Destory();
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogManager.Error($"[{e.Exception.Source}] {e.Exception.Message}\n{e.Exception.StackTrace}");
        }
    }
}