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
            WeakReferenceMessenger.Default.Register<MyMessengerType, string>(this, typeof(Window1ViewModel).ToString(), ReceiveMessage);
        }

        private void ReceiveMessage(object recipient, MyMessengerType message)
        {
        }
    }
}
