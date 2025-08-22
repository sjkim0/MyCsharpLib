using Avalonia.Threading;
using AvaloniaApplication1.DataType.Base;
using AvaloniaApplication1.Service.Interface;
using iio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AvaloniaApplication1.Service
{
    public class ZedFmcomm3LibiioService : ILibIIOService
    {
        private Context? ctx;
        Int32 timeout = 5000;
        List<Context>? ctxList;
        public event EventHandler<ENUM_LIBIIO_SCAN_TASK_STATE>? taskStateCallBack;

        public void start(string ip)
        {
            /* iio class list */
            //  Attr attr = null;
            //  Channel channel = null;
            //  Context context = null;
            //  Device device = null;
            //  IioLib iioLib = null;
            //  IOBuffer iOBuffer = null;
            //  ScanContext scanContext = null;
            //  Trigger trigger = null;
            //  Version version = null;
            //  ScanContext iio = new ScanContext();

            ctx = new Context("ip:" + ip);
            List<string> buff = new List<string>();

            foreach (Device dev in ctx.devices)
            {
                buff.Add(dev.id);
            }
        }

        public void stop()
        {
            throw new NotImplementedException();
        }

        public async void contextScanStart()
        {
            ScanContext scanContext = new ScanContext();
            
            await ScanStart();
        }

        private async Task ScanStart()
        {
            if (taskStateCallBack == null)
            {
                return;
            }
            try
            {
                taskStateCallBack(this, ENUM_LIBIIO_SCAN_TASK_STATE.ENUM_LIBIIO_SCAN_TASK_STATE_START);
                var task = Task.Run(() =>
                {
                    ScanContext scanContext = new ScanContext();

                    Dictionary<string, string> dns_sd = scanContext.get_dns_sd_backend_contexts();
                    ctxList = new List<Context>();

                    foreach (string key in dns_sd.Keys)
                    {
                        ctxList.Add(new Context(key));
                    }
                });

                if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
                {
                    // task completed
                    taskStateCallBack(this, ENUM_LIBIIO_SCAN_TASK_STATE.ENUM_LIBIIO_SCAN_TASK_STATE_OK);
                }
                else
                {
                    // timeout logic
                    taskStateCallBack(this, ENUM_LIBIIO_SCAN_TASK_STATE.ENUM_LIBIIO_SCAN_TASK_STATE_TIMEOUT);
                }
            }
            catch(OperationCanceledException)
            {
                taskStateCallBack(this, ENUM_LIBIIO_SCAN_TASK_STATE.ENUM_LIBIIO_SCAN_TASK_STATE_ERR);
            }
        }
    }
}
