using Avalonia.Controls;
using AvaloniaApplication1.Service.Interface;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaApplication1.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
#pragma warning disable CA1822 // Mark members as static
        public string Greeting => "Welcome to Avalonia!";
#pragma warning restore CA1822 // Mark members as static

        private readonly IDeviceStateService _deviceStateService;
        private readonly IWindowService _windowservice;

        public MainWindowViewModel(IDeviceStateService deviceStateService,
                                   IWindowService windowservice)
        {
            _deviceStateService = deviceStateService;
            _windowservice = windowservice;

            _deviceStateService.AddBoard("my_board before");
            _deviceStateService.AddBoard("my_board after");
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
    }
}
