using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Fl.Net
{
    internal static class InternalProtoUtil
    {
        #region Internal Public Methods
        internal static bool IsPrimitiveTyps(Type type)
        {
            if (type.IsPrimitive == true || type.FullName == "System.String")
            {
                return true;
            }

            return false;
        }

        internal static IEnumerable<byte> BuildAsciiPacketForPrimitiveArgument(object argument, TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return BuildAsciiPacketForBoolean((bool)argument);

                //case TypeCode.Byte:
                //    return BuildAsciiPacketForByte((byte)argument);

                //case TypeCode.Char:
                //    return BuildAsciiPacketForChar((char)argument);

                //case TypeCode.DateTime:
                //case TypeCode.DBNull:
                //case TypeCode.Decimal:
                //    return BuildAsciiPacketForDecimal((float)argument);

                //case TypeCode.Double:
                //    return BuildAsciiPacketForDouble((double)argument)

                //case TypeCode.Int16:
                //    return BuildAsciiPacketForInt16((Int16)argument)

                case TypeCode.Int32:
                    return BuildAsciiPacketForInt32((Int32)argument);

                //case TypeCode.Int64:
                //    return BuildAsciiPacketForInt64((Int64)argument);

                //case TypeCode.SByte:
                //    return BuildAsciiPacketForSByte((sbyte)argument);

                case TypeCode.Single:
                    return BuildAsciiPacketForSingle((float)argument);

                case TypeCode.String:
                    return BuildAsciiPacketForString((string)argument);

                //case TypeCode.UInt16:
                //    return BuildAsciiPacketForUInt16((UInt16)argument);

                case TypeCode.UInt32:
                    return BuildAsciiPacketForUInt32((UInt32)argument);

                //case TypeCode.UInt64:
                //    return BuildAsciiPacketForUInt64((UInt64)argument);

                default:
                    throw new Exception("Invalid type");
            }

            throw new InvalidOperationException();
        }

        internal static IEnumerable<byte> BuildBinPacketForPrimitiveArgument(object argument, TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return BuildBinPacketForBoolean((bool)argument);

                case TypeCode.Byte:
                    return BuildBinPacketForByte((byte)argument);

                case TypeCode.SByte:
                    return BuildBinPacketForSByte((sbyte)argument);

                case TypeCode.Int16:
                    return BuildBinPacketForInt16((Int16)argument);

                case TypeCode.UInt16:
                    return BuildBinPacketForUInt16((UInt16)argument);

                case TypeCode.Int32:
                    return BuildBinPacketForInt32((Int32)argument);

                case TypeCode.UInt32:
                    return BuildBinPacketForUInt32((UInt32)argument);

                case TypeCode.Int64:
                    return BuildBinPacketForInt64((Int64)argument);

                case TypeCode.UInt64:
                    return BuildBinPacketForUInt64((UInt64)argument);

                case TypeCode.Single:
                    return BuildBinPacketForSingle((float)argument);

                case TypeCode.Double:
                    return BuildBinPacketForDouble((double)argument);

                case TypeCode.String:
                    return BuildBinPacketForString((string)argument);
            }

            throw new InvalidOperationException("Unsupported type code.");
        }
        #endregion

        #region Private Methods
        private static byte[] BuildAsciiPacketForString(string stringArg)
        {
            byte[] packet = new byte[stringArg.Length];

            for (int i = 0; i < stringArg.Length; i++)
            {
                packet[i] = (byte)stringArg[i];
            }

            return packet;
        }

        private static byte[] BuildAsciiPacketForSingle(float floatArg)
        {
            string strValue = floatArg.ToString("F2", CultureInfo.InvariantCulture);
            byte[] packet = new byte[strValue.Length];

            for (int i = 0; i < strValue.Length; i++)
            {
                packet[i] = (byte)strValue[i];
            }

            return packet;
        }

        private static byte[] BuildAsciiPacketForInt32(int int32Arg)
        {
            string strValue = string.Format("{0}", int32Arg);
            byte[] packet = new byte[strValue.Length];

            for (int i = 0; i < strValue.Length; i++)
            {
                packet[i] = (byte)strValue[i];
            }

            return packet;
        }

        private static byte[] BuildAsciiPacketForUInt32(UInt32 uint32Arg)
        {
            string strValue = string.Format("{0}", uint32Arg);
            byte[] packet = new byte[strValue.Length];

            for (int i = 0; i < strValue.Length; i++)
            {
                packet[i] = (byte)strValue[i];
            }

            return packet;
        }

        private static byte[] BuildAsciiPacketForBoolean(bool boolArg)
        {
            byte[] packet = new byte[1];

            if (boolArg == true)
            {
                packet[0] = (byte)'1';
            }
            else
            {
                packet[0] = (byte)'0';
            }

            return packet;
        }

        private static IEnumerable<byte> BuildBinPacketForString(string argument)
        {
            List<byte> packet = new List<byte>();

            // Data.
            packet.AddRange(Encoding.UTF8.GetBytes(argument));

            return packet.ToArray();
        }

        private static IEnumerable<byte> BuildBinPacketForDouble(double argument)
        {
            // Data.
            byte[] packet = BitConverter.GetBytes(argument);

            return packet;
        }

        private static IEnumerable<byte> BuildBinPacketForSingle(float argument)
        {
            // Data.
            byte[] packet = BitConverter.GetBytes(argument);

            return packet;
        }

        private static IEnumerable<byte> BuildBinPacketForUInt64(ulong argument)
        {
            // Data.
            byte[] packet = BitConverter.GetBytes(argument);

            return packet;
        }

        private static IEnumerable<byte> BuildBinPacketForInt64(long argument)
        {
            // Data.
            byte[] packet = BitConverter.GetBytes(argument);

            return packet;
        }

        private static IEnumerable<byte> BuildBinPacketForUInt32(uint argument)
        {
            // Data.
            byte[] packet = BitConverter.GetBytes(argument);

            // google search keyword : c# uint32 little endian to big endian
            // https://docs.microsoft.com/en-us/dotnet/api/system.bitconverter?view=netframework-4.8
            //if (BitConverter.IsLittleEndian == true)
            //{
            //    Array.Reverse(packet);
            //}

            return packet;
        }

        private static IEnumerable<byte> BuildBinPacketForInt32(int argument)
        {
            // Data.
            byte[] packet = BitConverter.GetBytes(argument);

            return packet;
        }

        private static IEnumerable<byte> BuildBinPacketForUInt16(ushort argument)
        {
            // Data.
            byte[] packet = BitConverter.GetBytes(argument);
            //if (BitConverter.IsLittleEndian == true)
            //{
            //    Array.Reverse(packet);
            //}

            return packet;
        }

        private static IEnumerable<byte> BuildBinPacketForInt16(short argument)
        {
            // Data.
            byte[] packet = BitConverter.GetBytes(argument);

            return packet;
        }

        private static IEnumerable<byte> BuildBinPacketForSByte(sbyte argument)
        {
            byte[] packet = new byte[1];

            packet[0] = (byte)argument;

            return packet;
        }

        private static IEnumerable<byte> BuildBinPacketForByte(byte argument)
        {
            byte[] packet = new byte[1];

            // Data.
            packet[0] = argument;

            return packet;
        }

        private static IEnumerable<byte> BuildBinPacketForBoolean(bool argument)
        {
            byte[] packet = new byte[1];

            // Data.
            packet[0] = argument == true ? (byte)1 : (byte)0;

            return packet;
        }
        #endregion Private Methods
    }
}
