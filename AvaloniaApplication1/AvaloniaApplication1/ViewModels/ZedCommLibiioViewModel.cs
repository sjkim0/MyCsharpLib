using AvaloniaApplication1.DataType;
using AvaloniaApplication1.Service.Interface;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.ViewModels
{
    public class ZedCommLibiioViewModel : ViewModelBase
    {
        IMyMessageBoxService _myMessageBoxService;
        ILibIIOService _libIIOService;

        // for desing
        public ZedCommLibiioViewModel()
        {
        }

        public ZedCommLibiioViewModel(IMyMessageBoxService myMessageBoxService, ILibIIOService libIIOService)
        {
            _myMessageBoxService = myMessageBoxService;
            _libIIOService = libIIOService;

            // messenger 등록
            string token = typeof(ZedCommLibiioViewModel).ToString();
            WeakReferenceMessenger.Default.Register<MyMessengerType, string>(this, token, ReceiveMessage);

            _libIIOService.start();
        }

        private void ReceiveMessage(object recipient, MyMessengerType message)
        {
            throw new NotImplementedException();
        }
    }
}
