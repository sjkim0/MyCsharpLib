using AvaloniaApplication1.Service.Interface;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Service
{
    public class MyMessageType : ValueChangedMessage<string>
    {
        public MyMessageType(string value) : base(value)
        {
            WeakReferenceMessenger.Default.Register<MyMessageType>(this, (r, m) =>
            {
                 MyMessengerReceive(m.Value);
            });
        }

        public void MyMessengerReceive(string value)
        {
        }

        // How to register receiver in viewmodel.. Make code below in viewmodel
        //  WeakReferenceMessenger.Default.Register<MyMessageType>(this, (r, m) =>
        //  {
        //      MyMessengerReceive(m.Value);
        //  });
        // 
        //  private void MyMessengerReceive(string m)
        //  {
        //  }

        //  How to send message in viewmodel
        //  private void SendMessage()
        //  {
        //      WeakReferenceMessenger.Default.Send("TEST CODE");
        //  }
    }
}
