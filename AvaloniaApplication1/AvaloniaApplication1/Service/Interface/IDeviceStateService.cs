using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Service.Interface
{
    public interface IDeviceStateService
    {
        public void AddBoard(string name);
        int GetBoardCount();
    }
}
