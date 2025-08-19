using AvaloniaApplication1.DataType;
using AvaloniaApplication1.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AvaloniaApplication1.DataType.MyPacketType;

namespace AvaloniaApplication1.Service
{
    public class MyParserService : IMyParserService
    {
        private MyPacketType myPacket;

        public event EventHandler<MyPacketType> Parsed;

        public MyParserService()
        {
            myPacket = new MyPacketType();
        }

        public void ClearParser()
        {
            myPacket.data_buff_head = 0;
        }

        public void Push(byte[] data)
        {
            foreach(byte buff in data)
            {
                if (myPacket.data_buff_head < MyPacketType.whole_buffer_lenght)
                {
                    Loop(buff);
                }
            }
        }

        enum ENUM_SPECIAL_CHAR
        {
            ENUM_SPECIAL_CHAR_ASTERISK = '*',
            ENUM_SPECIAL_CHAR_LF = '\n',
            ENUM_SPECIAL_CHAR_CR = '\r'
        }

        enum ENUM_SPECIAL_BYTE
        {
            ENUM_SPECIAL_BYTE_START = 0,  // *, 0 -> start
            ENUM_SPECIAL_BYTE_END = 1,  // *, 1 -> end
            ENUM_SPECIAL_BYTE_ASTERISK = 2,  // *, 2 -> *
            ENUM_SPECIAL_BYTE_LF = 3,  // *, 3 -> \n
            ENUM_SPECIAL_BYTE_CR = 4   // *, 4 -> \r
        }

        void Loop(byte data)
        {
            switch (data)
            {
                case (byte)ENUM_SPECIAL_CHAR.ENUM_SPECIAL_CHAR_ASTERISK:
                    if(myPacket.packet_state != MyPacketType.PACKET_STATE.PACKET_STATE_BINARY_PARSE_STARTED)
                    {
                        myPacket.packet_state = MyPacketType.PACKET_STATE.PACKET_STATE_BINARY_PARSE_STARTED;
                    }
                    break;
                case (byte)ENUM_SPECIAL_CHAR.ENUM_SPECIAL_CHAR_LF:
                    myPacket.packet_state = MyPacketType.PACKET_STATE.PACKET_STATE_ASCII_PARSE_STARTED;
                    if (CheckSumCheck() == true)
                    {
                        CallAsciiParseDone();
                    }
                    resetDataBuff();
                    break;
                case (byte)ENUM_SPECIAL_CHAR.ENUM_SPECIAL_CHAR_CR:
                    myPacket.packet_state = MyPacketType.PACKET_STATE.PACKET_STATE_ASCII_PARSE_STARTED;
                    if (CheckSumCheck() == true)
                    {
                        CallAsciiParseDone();
                    }
                    resetDataBuff();
                    break;
                case (byte)ENUM_SPECIAL_BYTE.ENUM_SPECIAL_BYTE_START:
                    if (myPacket.packet_state == MyPacketType.PACKET_STATE.PACKET_STATE_BINARY_PARSE_STARTED)
                    {
                        resetDataBuff();
                    }
                    else
                    {
                        myPacket.data_buff[myPacket.data_buff_head] = data;
                        goToNextDataBuff();
                    }
                    break;
                case (byte)ENUM_SPECIAL_BYTE.ENUM_SPECIAL_BYTE_END:
                    if (myPacket.packet_state == MyPacketType.PACKET_STATE.PACKET_STATE_BINARY_PARSE_STARTED)
                    {
                        if(CheckSumCheck() == true)
                        {
                            CallBinaryParseDone();
                        }
                        resetDataBuff();
                    }
                    else
                    {
                        myPacket.data_buff[myPacket.data_buff_head] = data;
                        goToNextDataBuff();
                    }
                    break;
                case (byte)ENUM_SPECIAL_BYTE.ENUM_SPECIAL_BYTE_ASTERISK:
                    if (myPacket.packet_state == MyPacketType.PACKET_STATE.PACKET_STATE_BINARY_PARSE_STARTED)
                    {
                        myPacket.packet_state = MyPacketType.PACKET_STATE.PACKET_STATE_PARSE_END;
                        myPacket.data_buff[myPacket.data_buff_head] = (byte)ENUM_SPECIAL_CHAR.ENUM_SPECIAL_CHAR_ASTERISK;
                    }
                    else
                    {
                        myPacket.data_buff[myPacket.data_buff_head] = data;
                        goToNextDataBuff();
                    }
                    break;
                case (byte)ENUM_SPECIAL_BYTE.ENUM_SPECIAL_BYTE_LF:
                    if (myPacket.packet_state == MyPacketType.PACKET_STATE.PACKET_STATE_BINARY_PARSE_STARTED)
                    {
                        myPacket.packet_state = MyPacketType.PACKET_STATE.PACKET_STATE_PARSE_END;
                        myPacket.data_buff[myPacket.data_buff_head] = (byte)ENUM_SPECIAL_CHAR.ENUM_SPECIAL_CHAR_LF;
                    }
                    else
                    {
                        myPacket.data_buff[myPacket.data_buff_head] = data;
                        goToNextDataBuff();
                    }
                    break;
                case (byte)ENUM_SPECIAL_BYTE.ENUM_SPECIAL_BYTE_CR:
                    if (myPacket.packet_state == MyPacketType.PACKET_STATE.PACKET_STATE_BINARY_PARSE_STARTED)
                    {
                        myPacket.packet_state = MyPacketType.PACKET_STATE.PACKET_STATE_PARSE_END;
                        myPacket.data_buff[myPacket.data_buff_head] = (byte)ENUM_SPECIAL_CHAR.ENUM_SPECIAL_CHAR_CR;
                    }
                    else
                    {
                        myPacket.data_buff[myPacket.data_buff_head] = data;
                        goToNextDataBuff();
                    }
                    break;
                default:
                    myPacket.packet_state = MyPacketType.PACKET_STATE.PACKET_STATE_PARSE_END;
                    myPacket.data_buff[myPacket.data_buff_head] = data;
                    goToNextDataBuff();
                    break;
            }
        }

        void CallBinaryParseDone()
        {
            // TODO: 패킷 정보 처리

            ushort packet_buff = myPacket.data_buff[(int)ENUM_DATA_INDEX.ENUM_DATA_UP_CMD_HIGH];
            packet_buff = (ushort)(packet_buff << 8);
            packet_buff += myPacket.data_buff[(int)ENUM_DATA_INDEX.ENUM_DATA_UP_CMD_LOW];
            myPacket.cmd_up = packet_buff;

            packet_buff = myPacket.data_buff[(int)ENUM_DATA_INDEX.ENUM_DATA_DOWN_CMD_HIGH];
            packet_buff = (ushort)(packet_buff << 8);
            packet_buff += myPacket.data_buff[(int)ENUM_DATA_INDEX.ENUM_DATA_DOWN_CMD_LOW];
            myPacket.cmd_down = packet_buff;

            packet_buff = myPacket.data_buff[(int)ENUM_DATA_INDEX.ENUM_DATA_LENGTH_HIGH];
            packet_buff = (ushort)(packet_buff << 8);
            packet_buff += myPacket.data_buff[(int)ENUM_DATA_INDEX.ENUM_DATA_LENGTH_LOW];
            myPacket.data_length = packet_buff;

            Parsed.Invoke(this, myPacket);  // Return event to viewmodel for parse done
        }

        void CallAsciiParseDone()
        {
            Parsed.Invoke(this, myPacket);  // Return event to viewmodel for parse done
        }

        bool CheckSumCheck()
        {
            byte checksum_buff = myPacket.data_buff[0];
            byte received_checksum = myPacket.data_buff[myPacket.data_buff_head - 1];
            int end_of_data_index = myPacket.data_buff_head - 2;

            for (int i = 1; i <= end_of_data_index; i++)
            {
                checksum_buff ^= myPacket.data_buff[i];
            }
            if(checksum_buff == received_checksum)
            {
                myPacket.check_sum = checksum_buff;
                return true;
            }
            else
            {
                return false;
            }
        }

        void goToNextDataBuff()
        {
            myPacket.data_buff_head += 1;
        }

        void resetDataBuff()
        {
            myPacket.packet_state = MyPacketType.PACKET_STATE.PACKET_STATE_PARSE_END;
            myPacket.data_buff_head = 0;
        }
    }
}
