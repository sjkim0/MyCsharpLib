using AvaloniaApplication1.DataType.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Service.Interface
{
    public interface ILibIIOService
    {
        public void contextScanStart();
        public void stop();

        public event EventHandler<ENUM_LIBIIO_SCAN_TASK_STATE> taskStateCallBack;
    }

    public enum ENUM_LIBIIO_SCAN_TASK_STATE
    {
        ENUM_LIBIIO_SCAN_TASK_STATE_NONE,
        ENUM_LIBIIO_SCAN_TASK_STATE_START,
        ENUM_LIBIIO_SCAN_TASK_STATE_OK,
        ENUM_LIBIIO_SCAN_TASK_STATE_TIMEOUT,
        ENUM_LIBIIO_SCAN_TASK_STATE_ERR
    }
}
