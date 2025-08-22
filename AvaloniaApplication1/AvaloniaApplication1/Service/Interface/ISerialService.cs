using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Service.Interface
{
    public interface ISerialService : IDisposable
    {
        bool IsOpen { get; }
        void Open(string port_name, int baud_rate);
        void Close();
        string[] scanPort();

        Task writeAsyncString(string data);
        Task writeAsyncByte(byte[] data);
        event EventHandler<string> DataReceivedString;
        event EventHandler<byte[]> DataReceivedByte;
    }

}
