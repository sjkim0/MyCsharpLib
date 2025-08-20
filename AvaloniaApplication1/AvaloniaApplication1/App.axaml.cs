using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AvaloniaApplication1.ViewModels;
using AvaloniaApplication1.Views;
using Microsoft.Extensions.DependencyInjection;

using AvaloniaApplication1.Service.Collection;
using AvaloniaApplication1.Service.Interface;
using AvaloniaApplication1.Service;  // addCommonService ÀÎ½Ä

namespace AvaloniaApplication1
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var services = new ServiceCollection();
            services.AddCommonService();
            var provider = services.BuildServiceProvider();

            var vm_0 = provider.GetRequiredService<MainWindowViewModel>();

            // mapping in bootstrap
            var window_service = provider.GetRequiredService<IWindowService>();
            window_service.Register<Window1, Window1ViewModel>();
            window_service.Register<DataGridTestWindow, DataGridTestWindowViewModel>();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Line below is needed to remove Avalonia data validation.
                // Without this line you will get duplicate validations from both Avalonia and CT
                BindingPlugins.DataValidators.RemoveAt(0);
                desktop.MainWindow = new MainWindow
                {
                    DataContext = vm_0
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}