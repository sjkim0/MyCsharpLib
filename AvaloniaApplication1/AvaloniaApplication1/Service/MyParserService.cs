using AvaloniaApplication1.DataType;
using AvaloniaApplication1.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Service
{
    public class MyParserService : IMyParser
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

        public void Loop(byte data)
        {
            byte now_data = myPacket.data_buff[myPacket.data_buff_head];

            switch (now_data)
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
                        Parsed.Invoke(this, myPacket);  // Return event to viewmodel for parse done
                    }
                    resetDataBuff();
                    break;
                case (byte)ENUM_SPECIAL_CHAR.ENUM_SPECIAL_CHAR_CR:
                    myPacket.packet_state = MyPacketType.PACKET_STATE.PACKET_STATE_ASCII_PARSE_STARTED;
                    if (CheckSumCheck() == true)
                    {
                        Parsed.Invoke(this, myPacket);  // Return event to viewmodel for parse done
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
                        myPacket.data_buff[myPacket.data_buff_head] = now_data;
                    }
                    break;
                case (byte)ENUM_SPECIAL_BYTE.ENUM_SPECIAL_BYTE_END:
                    if (myPacket.packet_state == MyPacketType.PACKET_STATE.PACKET_STATE_BINARY_PARSE_STARTED)
                    {
                        if(CheckSumCheck() == true)
                        {
                            Parsed.Invoke(this, myPacket);  // Return event to viewmodel for parse done
                        }
                        resetDataBuff();
                    }
                    else
                    {
                        myPacket.data_buff[myPacket.data_buff_head] = now_data;
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
                        myPacket.data_buff[myPacket.data_buff_head] = now_data;
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
                        myPacket.data_buff[myPacket.data_buff_head] = now_data;
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
                        myPacket.data_buff[myPacket.data_buff_head] = now_data;
                    }
                    break;
                default:
                    myPacket.packet_state = MyPacketType.PACKET_STATE.PACKET_STATE_PARSE_END;
                    myPacket.data_buff[myPacket.data_buff_head] = now_data;
                    break;
            }
            goToNextDataBuff();
        }

        public bool CheckSumCheck()
        {
            throw new NotImplementedException();
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
