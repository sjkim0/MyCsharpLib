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
        string? ErrorLog;
        Int32 timeout = 3000;

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

        public async void start()
        {
            ScanContext scanContext = new ScanContext();
            
            await ScanStart();
        }

        private async Task ScanStart()
        {
            try
            {
                var task = Task.Run(() =>
                {
                    ScanContext scanContext = new ScanContext();

                    Dictionary<string, string> dns_sd = scanContext.get_dns_sd_backend_contexts();
                    foreach (string key in dns_sd.Keys)
                    {
                    }
                });

                if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
                {
                    // task completed within timeout
                }
                else
                {
                    // timeout logic
                }
            }
            catch(OperationCanceledException)
            {

            }
        }
    }
}
