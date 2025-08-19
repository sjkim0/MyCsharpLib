using AvaloniaApplication1.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Service
{
    public class SerialService : ISerialService
    {
        private SerialPort? _port;

        // 표준 이벤트 핸들러
        public event EventHandler<string>? DataReceivedString;
        public event EventHandler<byte[]>? DataReceivedByte;

        bool byte_read = true;

        public bool IsOpen
        {
            get
            {
                if(_port == null)
                {
                    return false;
                }
                return _port.IsOpen;
            }
        }

        public void Open(string port_name, int baud_rate = 115200)
        {
            if(IsOpen)
            {
                Close();
            }

            _port = new SerialPort(port_name, baud_rate);
            _port.Open();

            _port.DataReceived += SerialDataReceived;
        }

        private void SerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if(_port == null)
            {
                return;
            }
            if (byte_read)
            {
                int rx_length = _port.BytesToRead;
                byte[] buffer = new byte[rx_length];
                _port.Read(buffer, 0, rx_length);

                DataReceivedByte?.Invoke(this, buffer);
            }
            else
            {
                string data = _port.ReadExisting();
                DataReceivedString?.Invoke(this, data);
            }
        }

        public void Close()
        {
            if (_port != null)
            {
                _port.DataReceived -= SerialDataReceived;
                _port.Close();
                _port.Dispose();
                _port = null;
            }
        }

        public void Dispose()
        {
            Close();
        }

        public string[] scanPort()
        {
            return SerialPort.GetPortNames();
        }

        public async Task writeAsyncString(string data)
        {
            if (!IsOpen || _port == null)
            {
                return;
            }
            await Task.Run(() => _port.Write(data));
        }

        public async Task writeAsyncByte(byte[] data)
        {
            if (!IsOpen || _port == null)
            {
                return;
            }
            await Task.Run(() => _port.Write(data, 0, data.Length));
        }
    }
}
