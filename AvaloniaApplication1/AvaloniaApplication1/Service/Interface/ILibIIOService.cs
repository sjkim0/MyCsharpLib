using AvaloniaApplication1.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Service.Interface
{
    public interface ILibIIOService
    {
        public void start();
        public void stop();
        public void command<T>() where T : MyLibiioDeviceType;
    }
}
