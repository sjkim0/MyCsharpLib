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
        private readonly IServiceProvider _provider;
        private readonly Dictionary<Type, Type> _vm_types;  // view model type 정보 저장해 datacontext 설정시 serivce provider에서 viewmodel을 긁어온다.
        private readonly Dictionary<Type, Window> _windows;

        public WindowService(IServiceProvider provider)
        {
            _provider = provider;
            _windows = new Dictionary<Type, Window>();
            _vm_types = new Dictionary<Type, Type>();
        }

        public void close<T>() where T : Window
        {
            if (_windows.ContainsKey(typeof(T)))
            {
                _windows[typeof(T)].Close();
                _windows.Remove(typeof(T));
            }
        }

        public void show<T>() where T : Window
        {
            var window = _provider.GetRequiredService<T>();

            if (_windows.TryAdd(typeof(T), window))
            {
                _windows[typeof(T)].DataContext = _provider.GetRequiredService(_vm_types[typeof(T)]);
                _windows[typeof(T)].Show();
            }
        }

        // call in App.cs for push viewmodel
        public void Register<T, VM>() where T : Window
        {
            _vm_types[typeof(T)] = typeof(VM);
        }
    }
}
