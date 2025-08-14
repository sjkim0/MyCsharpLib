using Avalonia.Controls;
using AvaloniaApplication1.Service.Interface;
using AvaloniaApplication1.ViewModels;
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
            collection.AddTransient<MainWindowViewModel>();

            // window service 등록
            collection.AddTransient<IWindowService, WindowService>();

            // sub window DI
            collection.AddTransient<SubWindowViewModel>();
            collection.AddTransient<Window1>();

        }
    }
}
