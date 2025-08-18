using AvaloniaApplication1.Service;
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
            WeakReferenceMessenger.Default.Register<MyMessageType, string>(this, this.GetType().ToString(), ReceiveMessage);
        }

        private void ReceiveMessage(object recipient, MyMessageType message)
        {
            throw new NotImplementedException();
        }
    }
}
