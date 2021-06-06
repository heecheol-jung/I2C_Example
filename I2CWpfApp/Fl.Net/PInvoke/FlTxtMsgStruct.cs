using System;
using System.Runtime.InteropServices;

namespace Fl.Net.PInvoke
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FlTxtMsgStruct
    {
        // Unique device ID(for RS-422, RS-485).
        public UInt32 device_id;

        // Message ID.
        public byte msg_id;

        // Error code.
        public byte error;

        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/unsafe-code-pointers/fixed-size-buffers#:~:text=In%20safe%20code%2C%20a%20C%23,in%20an%20unsafe%20code%20block.&text=A%20struct%20can%20contain%20an%20embedded%20array%20in%20unsafe%20code.
        // It is for maximum message buffer.
        public fixed byte payload[(int)FlConstant.FL_VER_STR_MAX_LEN];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FlTxtMsgParserStruct
    {
        // A buffer for message reception.
        public fixed byte buf[(int)FlConstant.FL_TXT_MSG_MAX_LENGTH];

        // The number of received bytes.
        public byte buf_pos;

        public byte receive_state;

        // Parsed message ID.
        public byte msg_id;

        // Parsed device ID.
        public UInt32 device_id;

        // Response error code.
        public byte error;

        // Payload buffer(fl_fw_ver_t is the longest payload).
        public fixed byte payload[(int)FlConstant.FL_VER_STR_MAX_LEN];

        public byte arg_count;

        public IntPtr context;

        public fl_msg_cb_on_parsed_t on_parsed_callback;
        public fl_msg_dbg_cb_on_parse_started_t on_parse_started_callback;
        public fl_msg_dbg_cb_on_parse_ended_t on_parse_ended_callback;
    }
}
