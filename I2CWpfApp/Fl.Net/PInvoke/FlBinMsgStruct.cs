using System;
using System.Runtime.InteropServices;

namespace Fl.Net.PInvoke
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FlBinMsgFlag1Struct
    {
        public byte BitField1;

        public byte sequence_num
        {
            get
            {
                return FlUtil.BitFieldGet(BitField1, FlConstant.FL_BIN_FLAG1_SEQUENCE_NUM_MASK, FlConstant.FL_BIN_FLAG1_SEQUENCE_NUM_POS);
            }
            set
            {
                BitField1 = FlUtil.BitFieldSet(BitField1, value, FlConstant.FL_BIN_FLAG1_SEQUENCE_NUM_MASK, FlConstant.FL_BIN_FLAG1_SEQUENCE_NUM_POS);
            }
        }

        public byte return_expected
        {
            get
            {
                return FlUtil.BitFieldGet(BitField1, FlConstant.FL_BIN_FLAG1_RETURN_EXPECTED_MASK, FlConstant.FL_BIN_FLAG1_RETURN_EXPECTED_POS);
            }
            set
            {
                BitField1 = FlUtil.BitFieldSet(BitField1, value, FlConstant.FL_BIN_FLAG1_RETURN_EXPECTED_MASK, FlConstant.FL_BIN_FLAG1_RETURN_EXPECTED_POS);
            }
        }

        public byte message_type
        {
            get
            {
                return FlUtil.BitFieldGet(BitField1, FlConstant.FL_BIN_FLAG1_MSG_TYPE_MASK, FlConstant.FL_BIN_FLAG1_MSG_TYPE_POS);
            }
            set
            {
                BitField1 = FlUtil.BitFieldSet(BitField1, value, FlConstant.FL_BIN_FLAG1_MSG_TYPE_MASK, FlConstant.FL_BIN_FLAG1_MSG_TYPE_POS);
            }
        }

        public byte reserved
        {
            get
            {
                return FlUtil.BitFieldGet(BitField1, FlConstant.FL_BIN_FLAG1_RESERVED_MASK, FlConstant.FL_BIN_FLAG1_RESERVED_POS);
            }
            set
            {
                BitField1 = FlUtil.BitFieldSet(BitField1, value, FlConstant.FL_BIN_FLAG1_RESERVED_MASK, FlConstant.FL_BIN_FLAG1_RESERVED_POS);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FlBinMsgFlag2Struct
    {
        public byte BitField1;

        public byte error
        {
            get
            {
                return FlUtil.BitFieldGet(BitField1, FlConstant.FL_BIN_FLAG2_ERROR_MASK, FlConstant.FL_BIN_FLAG2_ERROR_POS);
            }
            set
            {
                BitField1 = FlUtil.BitFieldSet(BitField1, value, FlConstant.FL_BIN_FLAG2_ERROR_MASK, FlConstant.FL_BIN_FLAG2_ERROR_POS);
            }
        }

        public byte reserved
        {
            get
            {
                return FlUtil.BitFieldGet(BitField1, FlConstant.FL_BIN_FLAG2_RESERVED_MASK, FlConstant.FL_BIN_FLAG2_RESERVED_POS);
            }
            set
            {
                BitField1 = FlUtil.BitFieldSet(BitField1, value, FlConstant.FL_BIN_FLAG2_RESERVED_MASK, FlConstant.FL_BIN_FLAG2_RESERVED_POS);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FlBinMsgHeaderStruct
    {
        // Unique device ID(for RS-422, RS-485).
        public UInt32 device_id;

        // Message length.
        public byte length;

        // Message(function) ID.
        public byte message_id;

        // Flag1
        public FlBinMsgFlag1Struct flag1;

        // Flag2
        public FlBinMsgFlag2Struct flag2;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FlBinMsgHeaderArgStruct
    {
        public UInt32 device_id;
        public byte message_id;
        public byte sequnece_num;
        public byte return_expected;
        public byte error;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FlBinMsgFullStruct
    {
        public byte stx;
        public FlBinMsgHeaderStruct header;
        public fixed byte payload[(int)FlConstant.FL_VER_STR_MAX_LEN];  // fl_hw_ver_t is the maximimum payload.
        public UInt16 crc;
        public byte etx;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FlBinMsgParserStruct
    {
        // A buffer for message reception.
        public fixed byte buf[(int)FlConstant.FL_BIN_MSG_MAX_LENGTH];

        // The number of received bytes.
        public byte buf_pos;

        public byte count;

        public byte receive_state;

        public IntPtr context;

        public fl_msg_cb_on_parsed_t on_parsed_callback;
        public fl_msg_dbg_cb_on_parse_started_t on_parse_started_callback;
        public fl_msg_dbg_cb_on_parse_ended_t on_parse_ended_callback;
    }
}
