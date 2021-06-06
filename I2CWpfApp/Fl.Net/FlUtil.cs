using System;
using System.Collections.Generic;

namespace Fl.Net
{
    public class FlUtil
    {
        private const UInt16 CRC_POLY_SICK = 0x8005;
        private const UInt16 CRC_START_SICK = 0x0000;

        public static byte BitFieldSet(byte bitField, byte value, byte mask, byte pos)
        {
            return (byte)((bitField & ~mask) | ((value << pos) & mask));
        }

        public static byte BitFieldGet(byte bitField, byte mask, byte pos)
        {
            return (byte)((bitField & mask) >> pos);
        }

        public static Int32 BitFieldGet(Int32 bitField, Int32 mask, Int32 pos)
        {
            return (Int32)((bitField & mask) >> pos);
        }

        public static Int32 BitFieldSet(Int32 bitField, Int32 value, Int32 mask, Int32 pos)
        {
            return (Int32)((bitField & ~mask) | ((value << pos) & mask));
        }

        public static UInt16 CRC16(byte[] packet, int startIndex, int numOfBytes)
        {
            UInt16 crc = CRC_START_SICK;
            UInt16 short_c = 0;
            UInt16 short_p = 0;
            int i, j;

            for (j = 0, i = startIndex; j < numOfBytes; i++, j++)
            {
                short_c = (UInt16)(0x00FF & (UInt16)packet[i]);
                if ((crc & 0x8000) != 0)
                {
                    crc = (UInt16)((crc << 1) ^ CRC_POLY_SICK);
                }
                else
                {
                    crc = (UInt16)(crc << 1);
                }

                crc ^= (UInt16)(short_c | short_p);
                short_p = (UInt16)(short_c << 8);
            }

            //UInt16 low_byte = 0;
            //UInt16 high_byte = 0;
            //low_byte = (UInt16)((crc & 0xFF00) >> 8);
            //high_byte = (UInt16)((crc & 0x00FF) << 8);
            //crc = (UInt16)(low_byte | high_byte);

            return crc;
        }

        public static UInt16 CRC16(List<byte> packet, int startIndex, int numOfBytes)
        {
            UInt16 crc = CRC_START_SICK;
            UInt16 short_c = 0;
            UInt16 short_p = 0;
            int i, j;

            for (j = 0, i = startIndex; j < numOfBytes; i++, j++)
            {
                short_c = (UInt16)(0x00FF & (UInt16)packet[i]);
                if ((crc & 0x8000) != 0)
                {
                    crc = (UInt16)((crc << 1) ^ CRC_POLY_SICK);
                }
                else
                {
                    crc = (UInt16)(crc << 1);
                }

                crc ^= (UInt16)(short_c | short_p);
                short_p = (UInt16)(short_c << 8);
            }

            //UInt16 low_byte = 0;
            //UInt16 high_byte = 0;
            //low_byte = (UInt16)((crc & 0xFF00) >> 8);
            //high_byte = (UInt16)((crc & 0x00FF) << 8);
            //crc = (UInt16)(low_byte | high_byte);

            return crc;
        }

        // google search keyword : c# uint32 little endian to big endian
        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/c878e72e-d42e-417d-b4f6-1935ad96d8ae/converting-small-endian-to-big-endian-using-clong-value?forum=csharpgeneral
        public static short SwapInt16(short v)
        {
            return (short)(((v & 0xff) << 8) | ((v >> 8) & 0xff));
        }

        public static ushort SwapUInt16(ushort v)
        {
            return (ushort)(((v & 0xff) << 8) | ((v >> 8) & 0xff));
        }

        public static int SwapInt32(int v)
        {
            return (int)(((SwapInt16((short)v) & 0xffff) << 0x10) |
                          (SwapInt16((short)(v >> 0x10)) & 0xffff));
        }

        public static uint SwapUInt32(uint v)
        {
            return (uint)(((SwapUInt16((ushort)v) & 0xffff) << 0x10) |
                           (SwapUInt16((ushort)(v >> 0x10)) & 0xffff));
        }

        public static long SwapInt64(long v)
        {
            return (long)(((SwapInt32((int)v) & 0xffffffffL) << 0x20) |
                           (SwapInt32((int)(v >> 0x20)) & 0xffffffffL));
        }

        public static ulong SwapUInt64(ulong v)
        {
            return (ulong)(((SwapUInt32((uint)v) & 0xffffffffL) << 0x20) |
                            (SwapUInt32((uint)(v >> 0x20)) & 0xffffffffL));
        }
    }
}
