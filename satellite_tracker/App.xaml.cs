using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;
using satellite_tracker.ViewModels;
using System.Windows;

namespace satellite_tracker
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
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
    }
}