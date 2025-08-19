using Avalonia.Controls;
using AvaloniaApplication1.DataType;
using AvaloniaApplication1.Service.Interface;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        private readonly IDeviceStateService? _deviceStateService;
        private readonly IWindowService? _windowservice;
        private readonly ISerialService? _serialservice;
        private readonly IMyMessageBoxService? _myMessageboxService;
        private readonly IMyParserService? _myParserService;
        
        
        // just for design
        public MainWindowViewModel()
        {
        }

        public MainWindowViewModel(IDeviceStateService deviceStateService,
                                   IWindowService windowservice,
                                   ISerialService serialservice,
                                   IMyMessageBoxService myMessageboxService,
                                   IMyParserService myParserService)
        {
            _deviceStateService = deviceStateService;
            _windowservice = windowservice;
            _serialservice = serialservice;
            _myMessageboxService = myMessageboxService;
            _myParserService = myParserService;

            _deviceStateService.AddBoard("my_board before");
            _deviceStateService.AddBoard("my_board after");

            // messenger 등록
            WeakReferenceMessenger.Default.Register<MyMessengerType, string>(this, this.GetType().ToString(), ReceiveMessage);

            // serial service 등록
            _serialservice.DataReceivedByte += SerialDataReceived;

            // parse service callback 등록
            _myParserService.Parsed += ParcedCallback;
            byte[] data = { 1, 2, 3, 4 };
            _myParserService.Push(data);
        }

        private void ParcedCallback(object? sender, MyPacketType e)
        {
            throw new NotImplementedException();
        }

        private void SerialDataReceived(object? sender, byte[] e)
        {
            string test = Encoding.Default.GetString(e);
        }

        // message box test command
        [RelayCommand]
        private async Task MyMessageboxTestButton()
        {
            bool ret = await _myMessageboxService.showYesNoBox("MY CAPTION", "MY MESSAGE");

            if(ret == true)
            {
                _myMessageboxService.showErrorBox("TRUE IS COMMING", "TRUE IS COMMING");
            }
            else
            {
                _myMessageboxService.showErrorBox("FALSE OR QUIT IS COMMING", "FALSE OR QUIT IS COMMING");
            }
        }

        // serial test button command
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
            WeakReferenceMessenger.Default.Send(new MyMessengerType("TEST_CODE"), typeof(Window1ViewModel).ToString());
        }

        [RelayCommand]
        private void MyButtonClose()
        {
            _windowservice.close<Window1>();
        }

        private void ReceiveMessage(object recipient, MyMessengerType message)
        {

        }
    }
}
