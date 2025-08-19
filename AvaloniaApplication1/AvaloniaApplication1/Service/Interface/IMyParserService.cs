using AvaloniaApplication1.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Service.Interface
{
    public interface IMyParserService
    {
        void Push(byte[] data);
        void ClearParser();

        event EventHandler<MyPacketType> Parsed;
    }
}
