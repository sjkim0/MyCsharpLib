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
        Int32 timeout = 10000;
        List<Context>? ctxList;
        public event EventHandler<ENUM_LIBIIO_SCAN_TASK_STATE>? taskStateCallBack;

        Dictionary<string, Device> dev_info = new Dictionary<string, Device>();
        Dictionary<string, Channel> chls_info = new Dictionary<string, Channel>();

        bool timeout_retry_called = false;

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
            await ScanStart();
        }

        private async Task ScanStart()
        {
            if (taskStateCallBack == null)
            {
                return;
            }
            if (timeout_retry_called == true)
            {
                timeout_retry_called = false;
            }
            try
            {
                taskStateCallBack(this, ENUM_LIBIIO_SCAN_TASK_STATE.ENUM_LIBIIO_SCAN_TASK_STATE_START);
                var task = Task.Run(() =>
                {
                    addContext();
                    deviceTest();
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
                    timeout_retry_called = true;
                    // await ScanStart();
                }
            }
            catch(OperationCanceledException)
            {
                taskStateCallBack(this, ENUM_LIBIIO_SCAN_TASK_STATE.ENUM_LIBIIO_SCAN_TASK_STATE_ERR);
            }
        }

        private void addContext()
        {
            ScanContext scanContext = new ScanContext();

            Dictionary<string, string> dns_sd = scanContext.get_dns_sd_backend_contexts();
            ctxList = new List<Context>();

            foreach (string key in dns_sd.Keys)
            {
                ctxList.Add(new Context(key));
            }
        }

        private void deviceTest()
        {
            if(ctxList == null)
            {
                return;
            }
            foreach(Context ctx_buff in ctxList)
            {
                foreach(Device dev_buff in ctx_buff.devices)
                {
                    // devices info
                    dev_info[dev_buff.id + "," + dev_buff.name] = dev_buff;

                    foreach(Channel ch_buff in dev_buff.channels)
                    {
                        // channels info
                        chls_info[ch_buff.id + "," + ch_buff.name] = ch_buff;

                        foreach(Attr attr in ch_buff.attrs)
                        {
                            // ch attributes
                            if (attr.name.CompareTo("frequency") == 0)
                            {
                                string ret = ("Attribute content: " + attr.read());
                            }
                        }
                    }

                    // dev attributes
                    foreach(Attr attr in dev_buff.attrs)
                    {
                        string ret = (attr.name);
                    }

                    /* If we find cf-ad9361-lpc, try to read a few bytes from the first channel */
                    if (dev_buff.name.CompareTo("cf-ad9361-lpc") == 0)
                    {
                        Channel chn = dev_buff.channels[0];
                        chn.enable();
                        IOBuffer buf = new IOBuffer(dev_buff, 0x8000);
                        buf.refill();

                        string ret = "Read " + chn.read(buf).Length + " bytes from hardware";
                        buf.Dispose();
                    }
                }
            }
        }
    }
}
