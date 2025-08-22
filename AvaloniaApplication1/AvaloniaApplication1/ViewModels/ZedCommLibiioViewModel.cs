using AvaloniaApplication1.DataType;
using AvaloniaApplication1.Service.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.ViewModels
{
    public partial class ZedCommLibiioViewModel : ViewModelBase
    {
        IMyMessageBoxService _myMessageBoxService;
        ILibIIOService _libIIOService;

        [ObservableProperty]
        public string scanState;

        // for desing
        public ZedCommLibiioViewModel()
        {
        }

        public ZedCommLibiioViewModel(IMyMessageBoxService myMessageBoxService, ILibIIOService libIIOService)
        {
            _myMessageBoxService = myMessageBoxService;
            _libIIOService = libIIOService;
            _libIIOService.taskStateCallBack += taskStateCallback;

            // messenger 등록
            string token = typeof(ZedCommLibiioViewModel).ToString();
            WeakReferenceMessenger.Default.Register<MyMessengerType, string>(this, token, ReceiveMessage);

            _libIIOService.contextScanStart();
        }

        private void taskStateCallback(object? sender, ENUM_LIBIIO_SCAN_TASK_STATE e)
        {
            switch(e)
            {
                case ENUM_LIBIIO_SCAN_TASK_STATE.ENUM_LIBIIO_SCAN_TASK_STATE_START:
                    ScanState = "SCAN STARTED";
                    break;
                case ENUM_LIBIIO_SCAN_TASK_STATE.ENUM_LIBIIO_SCAN_TASK_STATE_OK:
                    ScanState = "SCAN DONE";
                    break;
                case ENUM_LIBIIO_SCAN_TASK_STATE.ENUM_LIBIIO_SCAN_TASK_STATE_TIMEOUT:
                    ScanState = "TIMEOUT ERR";
                    break;
                case ENUM_LIBIIO_SCAN_TASK_STATE.ENUM_LIBIIO_SCAN_TASK_STATE_ERR:
                    ScanState = "FAILED";
                    break;
                default:
                    ScanState = "";
                    break;
            }
        }

        private void ReceiveMessage(object recipient, MyMessengerType message)
        {
            throw new NotImplementedException();
        }
    }
}
