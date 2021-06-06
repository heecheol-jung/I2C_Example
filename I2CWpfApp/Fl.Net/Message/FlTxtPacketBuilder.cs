using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Net.Message
{
    public static class FlTxtPacketBuilder
    {
        public static Dictionary<FlMessageId, string> MessageIdToStringTable = new Dictionary<FlMessageId, string>()
        {
            { FlMessageId.Unknown, FlConstant.STR_UNKNOWN },
            { FlMessageId.ReadHardwareVersion, FlConstant.STR_RHVER },
            { FlMessageId.ReadFirmwareVersion, FlConstant.STR_RFVER },
            { FlMessageId.ReadGpio, FlConstant.STR_RGPIO },
            { FlMessageId.WriteGpio, FlConstant.STR_WGPIO },
            { FlMessageId.ButtonEvent, FlConstant.STR_EBTN },
            { FlMessageId.ReadTemperature, FlConstant.STR_RTEMP },
            { FlMessageId.ReadHumidity, FlConstant.STR_RHUM },
            { FlMessageId.ReadTempAndHum, FlConstant.STR_RTAH },
            { FlMessageId.BootMode, FlConstant.STR_BMODE },
            { FlMessageId.Reset, FlConstant.STR_RESET },
            { FlMessageId.ReadWriteI2C, FlConstant.STR_RWI2C }
        };

        public static Dictionary<string, FlMessageId> StringToMessageIdTable = new Dictionary<string, FlMessageId>()
        {
            { FlConstant.STR_UNKNOWN, FlMessageId.Unknown },
            { FlConstant.STR_RHVER, FlMessageId.ReadHardwareVersion },
            { FlConstant.STR_RFVER, FlMessageId.ReadFirmwareVersion },
            { FlConstant.STR_RGPIO, FlMessageId.ReadGpio },
            { FlConstant.STR_WGPIO, FlMessageId.WriteGpio },
            { FlConstant.STR_EBTN, FlMessageId.ButtonEvent },
            { FlConstant.STR_RTEMP, FlMessageId.ReadTemperature },
            { FlConstant.STR_RHUM, FlMessageId.ReadHumidity },
            { FlConstant.STR_RTAH, FlMessageId.ReadTempAndHum },
            { FlConstant.STR_BMODE, FlMessageId.BootMode },
            { FlConstant.STR_RESET, FlMessageId.Reset },
            { FlConstant.STR_RWI2C, FlMessageId.ReadWriteI2C }
        };

        public static void BuildMessagePacket(ref IFlMessage txtMessage)
        {
            List<byte> packet = new List<byte>();

            // TODO : Check message type(text).
            // TODO : Check message category.

            // Command string packet.
            packet.AddRange(Encoding.ASCII.GetBytes(MessageIdToStringTable[txtMessage.MessageId]));

            // Arguments.
            if (txtMessage.Arguments?.Count > 0)
            {
                Type type;

                // Delimiter for a command and a device ID.
                packet.Add((byte)FlConstant.FL_TXT_MSG_ID_DEVICE_ID_DELIMITER);

                for (int i = 0; i < txtMessage.Arguments.Count; i++)
                {
                    type = txtMessage.Arguments[i].GetType();
                    if (InternalProtoUtil.IsPrimitiveTyps(type) == true)
                    {
                        packet.AddRange(InternalProtoUtil.BuildAsciiPacketForPrimitiveArgument(txtMessage.Arguments[i], Type.GetTypeCode(type)));
                    }

                    if (i != (txtMessage.Arguments.Count - 1))
                    {
                        // ','
                        packet.Add((byte)FlConstant.FL_TXT_MSG_ARG_DELIMITER);
                    }
                }
            }

            // '\n'
            packet.Add((byte)FlConstant.FL_TXT_MSG_TAIL);

            txtMessage.Buffer = packet.ToArray();
        }
    }
}
