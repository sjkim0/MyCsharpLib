using Avalonia.Controls;
using AvaloniaApplication1.Service;
using AvaloniaApplication1.Service.Interface;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Text;

namespace AvaloniaApplication1.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        private readonly IDeviceStateService _deviceStateService;
        private readonly IWindowService _windowservice;
        private readonly ISerialService _serialservice;
        
        // just for design
        public MainWindowViewModel()
        {
        }

        public MainWindowViewModel(IDeviceStateService deviceStateService,
                                   IWindowService windowservice,
                                   ISerialService serialservice)
        {
            _deviceStateService = deviceStateService;
            _windowservice = windowservice;
            _serialservice = serialservice;

            _deviceStateService.AddBoard("my_board before");
            _deviceStateService.AddBoard("my_board after");

            // messenger 등록
            WeakReferenceMessenger.Default.Register<MyMessageType, string>(this, this.GetType().ToString(), ReceiveMessage);


            // serial service 등록
            _serialservice.DataReceivedByte += SerialDataReceived;
        }

        private void SerialDataReceived(object? sender, byte[] e)
        {
            string test = Encoding.Default.GetString(e);
        }

        [RelayCommand]
        private void MySerialTestButton()
        {
            string[] scan_port_arr = _serialservice.scanPort();
            _serialservice.Open(scan_port_arr[0], 115200);
            _serialservice.writeAsyncString("1234\r\n");
        }


        [RelayCommand]
        private void MySubWindowTestButton()
        {
            _windowservice.show<Window1>();
            WeakReferenceMessenger.Default.Send(new MyMessageType("TEST_CODE"), this.GetType().ToString());
        }

        [RelayCommand]
        private void MyButtonClose()
        {
            _windowservice.close<Window1>();
        }

        private void ReceiveMessage(object recipient, MyMessageType message)
        {

        }
    }
}
