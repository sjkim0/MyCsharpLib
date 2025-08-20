using Avalonia.Controls;
using AvaloniaApplication1.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Service
{
    public class WindowService : IWindowService
    {
        // singletone default injection
        private readonly IServiceProvider _provider;
        private readonly Dictionary<Type, Type> _vm_types;  // view model type 정보 저장해 datacontext 설정시 serivce provider에서 viewmodel을 긁어온다.
        private readonly Dictionary<Type, Window> _windows;  // window raw instance

        public WindowService(IServiceProvider provider)
        {
            _provider = provider;
            _windows = new Dictionary<Type, Window>();
            _vm_types = new Dictionary<Type, Type>();
        }

        public void close<VM>() where VM : class
        {
            if (_windows.ContainsKey(typeof(VM)))
            {
                _windows[typeof(VM)].Close();
            }
        }

        public void show<VM>() where VM : class
        {
            var vmtype = typeof(VM);
            var viewType = _vm_types[typeof(VM)];

            var window = _provider.GetRequiredService(viewType);

            if (_windows.TryAdd(typeof(VM), (Window)window))
            {
                _windows[typeof(VM)].DataContext = _provider.GetRequiredService(vmtype);
                _windows[typeof(VM)].Closed += (o, e) =>
                {
                    _windows.Remove(typeof(VM));
                };
                _windows[typeof(VM)].Show();
            }
        }

        // call in App.cs for push viewmodel
        public void Register<WINDOW_T, VM>() where WINDOW_T : Window
        {
            _vm_types[typeof(VM)] = typeof(WINDOW_T);
        }
        // register 함수 사용 예(in App.cs after build provider)
        //  var window_service = provider.GetRequiredService<IWindowService>();
        //  window_service.Register<Window1, Window1ViewModel>();
    }
}
