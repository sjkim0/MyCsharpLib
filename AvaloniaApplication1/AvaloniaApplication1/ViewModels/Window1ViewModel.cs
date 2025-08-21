using AvaloniaApplication1.DataType;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.ViewModels
{
    public class Window1ViewModel : ViewModelBase
    {
        public Window1ViewModel()
        {
            // messenger 등록
            string token = typeof(Window1ViewModel).ToString();
            WeakReferenceMessenger.Default.Register<MyMessengerType, string>(this, token, ReceiveMessage);
        }

        private void ReceiveMessage(object recipient, MyMessengerType message)
        {
        }
    }
}
