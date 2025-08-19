using AvaloniaApplication1.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Service.Interface
{
    public interface IMyParser
    {
        void Push(byte[] data);
        void ClearParser();
        void Loop(byte data);

        bool CheckSumCheck();

        event EventHandler<MyPacketType> Parsed;
    }
}
