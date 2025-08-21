using AvaloniaApplication1.DataType.Base;
using AvaloniaApplication1.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iio;
using System.Diagnostics;

namespace AvaloniaApplication1.Service
{
    public class ZedFmcomm3LibiioService : ILibIIOService
    {
        private Context? ctx;

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

        public void start()
        {
            ScanContext scanContext = new ScanContext();

            // get usb but not working now
            Dictionary<string, string> usb = scanContext.get_usb_backend_contexts();
            foreach (string key in usb.Keys)
            {
            }

            // get network?
            Dictionary<string, string> dns_sd = scanContext.get_dns_sd_backend_contexts();
            foreach (string key in dns_sd.Keys)
            {
            }

            //?
            //Dictionary<string, string> local = scanContext.get_local_backend_contexts();
            //foreach (string key in local.Keys)
            //{
            //}

        }
    }
}
