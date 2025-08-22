using Avalonia.Threading;
using AvaloniaApplication1.DataType;
using AvaloniaApplication1.DataType.Base;
using AvaloniaApplication1.Service.Interface;
using iio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AvaloniaApplication1.Service
{
    public class ZedFmcomm3LibiioService : ILibIIOService
    {
        private Context? ctx;
        Int32 timeout = 10000;
        List<Context>? ctxList;
        public event EventHandler<ENUM_LIBIIO_SCAN_TASK_STATE>? taskStateCallBack;
        public event EventHandler<ObservableCollection<MyTreeNode>>? contextTreeReturn;

        bool timeout_retry_called = false;

        ObservableCollection<MyTreeNode> nodes;

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

            // node 0 -> count of ctx,
            // node 1 -> devices count,
            // node 2 -> 
            MyTreeNode node_info;

            ObservableCollection<string> ctx_names = new ObservableCollection<string>();
            ObservableCollection<string> dev_name_id = new ObservableCollection<string>();
            ObservableCollection<string> ch_name_id = new ObservableCollection<string>();
            ObservableCollection<string> ch_attr_names = new ObservableCollection<string>();
            ObservableCollection<string> dev_attr_names = new ObservableCollection<string>();

            node_info = new MyTreeNode(ctxList.Count.ToString(), new ObservableCollection<MyTreeNode>());  // fill node info
            foreach (var item in ctxList.Select((value, index) => (value, index)))
            {
                Context ctx_buff = item.value;
                int index_ctx = item.index;

                ctx_names.Add("name:" + ctx_buff.name);  // fill node

                if(node_info.SubNodes != null)  // fill node info
                {
                    node_info.SubNodes.Add(new MyTreeNode(ctx_buff.devices.Count.ToString(), new ObservableCollection<MyTreeNode>()));
                }

                foreach (var item_dev in ctx_buff.devices.Select((value, index) => (value, index)))
                {
                    Device dev_buff = item_dev.value;
                    int index_dev = item_dev.index;

                    dev_name_id.Add("name:" + dev_buff.name + ", id:" + dev_buff.id);  // fill node

                    foreach (Channel ch_buff in dev_buff.channels)
                    {
                        ch_name_id.Add("name:" + ch_buff.name + ", id:" + ch_buff.id);  // fill node

                        foreach(Attr attr in ch_buff.attrs)
                        {
                            ch_attr_names.Add(attr.name);  // fill node

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
                        dev_attr_names.Add("name:" + attr.name);  // fill node

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
            if(contextTreeReturn != null)
            {
                //nodes = new ObservableCollection<MyTreeNode>();

                //ObservableCollection<string> ctx_names = new ObservableCollection<string>();
                //ObservableCollection<string> dev_name_id = new ObservableCollection<string>();
                //ObservableCollection<string> ch_name_id = new ObservableCollection<string>();
                //ObservableCollection<string> ch_attr_names = new ObservableCollection<string>();
                //ObservableCollection<string> dev_attr_names = new ObservableCollection<string>();

                nodes = new ObservableCollection<MyTreeNode>();

                MyTreeNode ctxRoot = new MyTreeNode("ctxRoot", new ObservableCollection<MyTreeNode>());
                MyTreeNode devNameId = new MyTreeNode("devNameId", new ObservableCollection<MyTreeNode>());
                MyTreeNode chNameId = new MyTreeNode("chNameId", new ObservableCollection<MyTreeNode>());
                MyTreeNode chAttrNames = new MyTreeNode("chAttrNames", new ObservableCollection<MyTreeNode>());
                MyTreeNode devAttrNames = new MyTreeNode("devAttrNames", new ObservableCollection<MyTreeNode>());

                foreach (string buff in ctx_names)
                {
                    ctxRoot.SubNodes.Add(new MyTreeNode(buff, new ObservableCollection<MyTreeNode>()));
                }
                foreach (string buff in dev_name_id)
                {
                    ctxRoot.SubNodes[0].SubNodes.Add(new MyTreeNode(buff));
                }
                //foreach (string buff in ch_name_id)
                //{
                //    if (chNameId.SubNodes != null)
                //    {
                //        chNameId.SubNodes.Add(new MyTreeNode(buff, new ObservableCollection<MyTreeNode>()));
                //    }
                //}
                //foreach (string buff in ch_attr_names)
                //{
                //    if (chAttrNames.SubNodes != null)
                //    {
                //        chAttrNames.SubNodes.Add(new MyTreeNode(buff, new ObservableCollection<MyTreeNode>()));
                //    }
                //}
                //foreach (string buff in dev_attr_names)
                //{
                //    if (devAttrNames.SubNodes != null)
                //    {
                //        devAttrNames.SubNodes.Add(new MyTreeNode(buff));
                //    }
                //}

                nodes.Add(ctxRoot);

                contextTreeReturn.Invoke(this, nodes);
            }
        }
    }
}
