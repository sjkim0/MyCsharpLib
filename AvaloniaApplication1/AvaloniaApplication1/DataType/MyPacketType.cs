using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.DataType
{
    public class MyPacketType
    {
        public enum PACKET_STATE
        {
            PACKET_STATE_NONE,
            PACKET_STATE_BINARY_PARSE_STARTED,
            PACKET_STATE_ASCII_PARSE_STARTED,
            PACKET_STATE_PARSE_END,
        }

        public enum ENUM_DATA_INDEX
        {
            ENUM_DATA_UP_CMD_HIGH,
            ENUM_DATA_UP_CMD_LOW,
            ENUM_DATA_DOWN_CMD_HIGH,
            ENUM_DATA_DOWN_CMD_LOW,
            ENUM_DATA_LENGTH_HIGH,
            ENUM_DATA_LENGTH_LOW,
            ENUM_DATA_DATA_START,
        }


        public PACKET_STATE packet_state;
        public const int whole_buffer_lenght = 1024;

        public ushort cmd_up { get; set; }
        public ushort cmd_down { get; set; }
        public ushort data_length { get; set; }

        public int data_buff_head { get; set; }

        public byte[] data_buff = new byte[whole_buffer_lenght];
        public byte check_sum { get; set; }


    }
}
