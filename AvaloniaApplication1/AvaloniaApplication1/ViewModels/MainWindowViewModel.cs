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
            string token = typeof(MainWindowViewModel).ToString();
            WeakReferenceMessenger.Default.Register<MyMessengerType, string>(this, token, ReceiveMessage);

            // serial service 등록
            _serialservice.DataReceivedByte += SerialDataReceived;

            // parse service callback 등록
            _myParserService.Parsed += ParcedCallback;
        }

        private void ParcedCallback(object? sender, MyPacketType e)
        {
            if (_myMessageboxService == null)
            {
                return;
            }
            _myMessageboxService.showErrorBox("PARSE WORKED", "PARSE WORKED");
        }



        private void SerialDataReceived(object? sender, byte[] e)
        {
            string test = Encoding.Default.GetString(e);
        }

        [RelayCommand]
        private void MyParseTestButton()
        {
            if(_myParserService == null)
            {
                return;
            }
            byte[] data = { (byte)'*', 0, 0, 1, 2, 3, 4, 4, (byte)'*', 1 };
            _myParserService.Push(data);
        }

        // message box test command
        [RelayCommand]
        private async Task MyMessageboxTestButton()
        {
            if (_myMessageboxService == null)
            {
                return;
            }
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
            if (_serialservice == null)
            {
                return;
            }
            string[] scan_port_arr = _serialservice.scanPort();
            _serialservice.Open(scan_port_arr[0], 115200);
            _serialservice.writeAsyncString("1234\r\n");
        }

        [RelayCommand]
        private void MySubWindowTestButton()
        {
            if (_windowservice == null)
            {
                return;
            }
            _windowservice.show<Window1ViewModel>();
            WeakReferenceMessenger.Default.Send(new MyMessengerType("TEST_CODE"), typeof(Window1ViewModel).ToString());
        }

        [RelayCommand]
        private void MyButtonClose()
        {
            if (_windowservice == null)
            {
                return;
            }
            _windowservice.close<Window1ViewModel>();
        }

        private void ReceiveMessage(object recipient, MyMessengerType message)
        {

        }

        [RelayCommand]
        private void MyDataGridTest()
        {
            if(_windowservice == null)
            {
                return;
            }
            _windowservice.show<DataGridTestWindowViewModel>();
        }


        [RelayCommand]
        private void LibIIOTest()
        {
            if (_windowservice == null)
            {
                return;
            }
            _windowservice.show<ZedCommLibiioViewModel>();
        }
    }
}
