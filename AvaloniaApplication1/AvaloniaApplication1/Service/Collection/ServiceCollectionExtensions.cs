using Avalonia.Controls;
using AvaloniaApplication1.Service.Interface;
using AvaloniaApplication1.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Service.Collection
{
    // 확장
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonService(this IServiceCollection collection)
        {
            collection.AddTransient<IDeviceStateService, DeviceStateService>();

            // window service 등록
            collection.AddSingleton<IWindowService, WindowService>();


            // Imessenger 등록
            collection.AddSingleton<IMessenger, WeakReferenceMessenger>();

            // Iserialservice 등록
            collection.AddSingleton<ISerialService, SerialService>();

            // messagebox service 등록
            collection.AddTransient<IMyMessageBoxService, MyMessageBoxService>();

            // parser service 등록
            collection.AddTransient<IMyParserService, MyParserService>();

            // view model 등록 start
            collection.AddTransient<MainWindowViewModel>();

            collection.AddTransient<Window1ViewModel>();
            collection.AddTransient<Window1>();

            collection.AddTransient<DataGridTestWindowViewModel>();
            collection.AddTransient<DataGridTestWindow>();
            // view model 등록 end
        }
    }
}
