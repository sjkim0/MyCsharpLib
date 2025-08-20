using AvaloniaApplication1.Service.Interface;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AvaloniaApplication1.DataType
{
    public class MyMessengerType : ValueChangedMessage<string>
    {
        // viewmodel에서 등록하고 send하는것으로 한다. serivce화 하기엔 현재 기술부채가 너무 크다
        // token은 뷰모델의 type으로 한다. ex) typeof(Window1ViewModel).ToString()
        public MyMessengerType(string value) : base(value)
        {
        }

        // 토큰 버전 receive
        // 아래는 토큰을 this.GetType().ToString()으로 생성해서 처리하는 코드이다.
        //WeakReferenceMessenger.Default.Register<MyMessengerType, string>(this, this.GetType().ToString(), ReceiveMessage);
        //private void ReceiveMessage(object recipient, MyMessengerType message)
        //{
        //}
        // 토큰 버전 send
        //  WeakReferenceMessenger.Default.Send(new MyMessengerType("TEST_CODE"), this.GetType().ToString());


        // 단순 구현 receive
        // How to register receiver in viewmodel.. Make code below in viewmodel
        //  WeakReferenceMessenger.Default.Register<MyMessengerType>(this, (r, m) =>
        //  {
        //      MyMessengerReceive(m.Value);
        //  });
        // 
        //  private void MyMessengerReceive(string m)
        //  {
        //  }

        // 단순 구현 send
        //  How to send message in viewmodel
        //  private void SendMessage()
        //  {
        //      WeakReferenceMessenger.Default.Send("TEST CODE");
        //  }
    }
}
