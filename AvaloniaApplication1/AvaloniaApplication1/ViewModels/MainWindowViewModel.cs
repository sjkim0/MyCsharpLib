using Avalonia.Controls;
using AvaloniaApplication1.Service;
using AvaloniaApplication1.Service.Interface;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;

namespace AvaloniaApplication1.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        private readonly IDeviceStateService _deviceStateService;
        private readonly IWindowService _windowservice;
        

        // just for design
        public MainWindowViewModel()
        {
        }

        public MainWindowViewModel(IDeviceStateService deviceStateService,
                                   IWindowService windowservice)
        {
            _deviceStateService = deviceStateService;
            _windowservice = windowservice;

            _deviceStateService.AddBoard("my_board before");
            _deviceStateService.AddBoard("my_board after");

            // messenger 등록
            WeakReferenceMessenger.Default.Register<MyMessageType>(this, (r, m) =>
            {
                MyMessageReceiveTest(m.Value);
            });
        }

        [RelayCommand]
        private void MyButtonOpen()
        {
            _windowservice.show<Window1>();
        }

        [RelayCommand]
        private void MyButtonClose()
        {
            _windowservice.close<Window1>();
        }

        private void MyMessageReceiveTest(string m)
        {

        }
    }
}
