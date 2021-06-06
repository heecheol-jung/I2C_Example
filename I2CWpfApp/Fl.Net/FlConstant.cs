using System;

namespace Fl.Net
{
    public enum FlMessageType
    {
        Unknown,

        Binary,

        Text
    }

    public enum FlMessageCategory
    {
        Command,

        Response,

        Event,

        Unknown
    }

    public enum FlMessageId
    {
        Unknown,
        ReadHardwareVersion = 1,
        ReadFirmwareVersion = 2,
        ReadGpio = 3,
        WriteGpio = 4,
        ButtonEvent = 5,
        ReadTemperature = 6,
        ReadHumidity = 7,
        ReadTempAndHum = 8,
        BootMode = 9,
        Reset = 10,
        ReadWriteI2C = 11
    }

    public enum FlParseState
    {
        Parsing,

        ParseOk,

        ParseFail
    }

    public enum FlParserRole
    {
        Host,

        Device
    }

    public delegate void FlOnParseDone(object sender, object evtArg);

    public class FlConstant
    {
        public const byte FL_FALSE = 0;
        public const byte FL_TRUE = 1;

        public const byte FL_OK = 0;
        public const byte FL_ERROR = 1;

        public const byte FL_MSG_ID_BASE = 0;
        public const byte FL_MSG_ID_UNKNOWN = 0;
        public const byte FL_MSG_ID_READ_HW_VERSION = (FL_MSG_ID_BASE + 1);
        public const byte FL_MSG_ID_READ_FW_VERSION = (FL_MSG_ID_BASE + 2);
        public const byte FL_MSG_ID_READ_GPIO = (FL_MSG_ID_BASE + 3);
        public const byte FL_MSG_ID_WRITE_GPIO = (FL_MSG_ID_BASE + 4);
        public const byte FL_MSG_ID_BUTTON_EVENT = (FL_MSG_ID_BASE + 5);
        public const byte FL_MSG_ID_READ_TEMPERATURE = (FL_MSG_ID_BASE + 6);
        public const byte FL_MSG_ID_READ_HUMIDITY = (FL_MSG_ID_BASE + 7);
        public const byte FL_MSG_ID_READ_TEMP_AND_HUM = (FL_MSG_ID_BASE + 8);
        public const byte FL_MSG_ID_BOOT_MODE = (FL_MSG_ID_BASE + 9);
        public const byte FL_MSG_ID_RESET = (FL_MSG_ID_BASE + 10);
        public const byte FL_MSG_ID_READ_WRITE_I2C = (FL_MSG_ID_BASE + 11);

        public const uint FL_MSG_MAX_STRING_LEN = 32;
        public const UInt32 FL_DEVICE_ID_UNKNOWN = 0;
        public const UInt32 FL_DEVICE_ID_ALL = 0xFFFFFFFF;
        public const byte FL_BUTTON_RELEASED = 0;
        public const byte FL_BUTTON_PRESSED = 1;

        public const byte FL_MSG_TYPE_COMMAND = 0;
        public const byte FL_MSG_TYPE_RESPONSE = 1;
        public const byte FL_MSG_TYPE_EVENT = 2;
        public const byte FL_MSG_TYPE_UNKNOWN = 0xff;

        public const byte FL_BMODE_APP = 0;
        public const byte FL_BMODE_BOOTLOADER = 1;

        public const uint FL_VER_STR_MAX_LEN = 32;

        public const byte FL_TXT_MSG_ID_MIN_CHAR = (byte)'A';
        public const byte FL_TXT_MSG_ID_MAX_CHAR = (byte)'Z';
        public const byte FL_TXT_DEVICE_ID_MIN_CHAR = (byte)'0';
        public const byte FL_TXT_DEVICE_ID_MAX_CHAR = (byte)'9';
        public const byte FL_TXT_MSG_TAIL = (byte)'\n';
        public const byte FL_TXT_MSG_ID_DEVICE_ID_DELIMITER = (byte)' ';
        public const byte FL_TXT_MSG_ARG_DELIMITER = (byte)',';

        public const UInt32 FL_TXT_MSG_MAX_LENGTH = 64;

        public const byte FL_BIN_MSG_STX = 0x02;
        public const byte FL_BIN_MSG_ETX = 0x03;

        public const byte FL_BIN_MSG_STX_LENGTH = 1;
        public const byte FL_BIN_MSG_HEADER_LENGTH = 8;
        public const byte FL_BIN_MSG_MAX_PAYLOAD_LENGTH = 37;
        public const byte FL_BIN_MSG_CRC_LENGTH = 2;
        public const byte FL_BIN_MSG_ETX_LENGTH = 1;

        public const byte FL_BIN_MSG_MAX_DATA_LENGTH = 41;
        public const byte FL_BIN_MSG_MIN_LENGTH = 12;
        public const byte FL_BIN_MSG_MAX_LENGTH = 49;

        public const byte FL_BIN_MSG_DEVICE_ID_LENGTH = 4;

        public const byte FL_BIN_MSG_MIN_SEQUENCE = 0;
        public const byte FL_BIN_MSG_MAX_SEQUENCE = 0xf;

        public const byte FL_BIN_FLAG1_RESERVED_MASK = 0b00000001;
        public const byte FL_BIN_FLAG1_RESERVED_POS = 0;
        public const byte FL_BIN_FLAG1_MSG_TYPE_MASK = 0b00000110;
        public const byte FL_BIN_FLAG1_MSG_TYPE_POS = 1;
        public const byte FL_BIN_FLAG1_RETURN_EXPECTED_MASK = 0b00001000;
        public const byte FL_BIN_FLAG1_RETURN_EXPECTED_POS = 3;
        public const byte FL_BIN_FLAG1_SEQUENCE_NUM_MASK = 0b11110000;
        public const byte FL_BIN_FLAG1_SEQUENCE_NUM_POS = 4;

        public const byte FL_BIN_FLAG2_RESERVED_MASK = 0b00111111;
        public const byte FL_BIN_FLAG2_RESERVED_POS = 0;
        public const byte FL_BIN_FLAG2_ERROR_MASK = 0b11000000;
        public const byte FL_BIN_FLAG2_ERROR_POS = 6;

        public const byte HeaderDeviceIdFieldIndex = 1;
        public const byte HeaderLengthFieldIndex = 5;
        public const byte HeaderMessageIdFieldIndex = 6;
        public const byte HeaderFlag1FieldIndex = 7;
        public const byte HeaderFlag2FieldIndex = 8;

        public const int FL_DEF_CMD_MAX_TRY_COUNT = 3;
        public const int FL_DEF_CMD_TRY_INTERVAL = 100; // millisecond
        public const int FL_DEF_CMD_RESPONSE_TIMEOUT = 50; // millisecond

        public const uint TXT_MSG_ID_MAX_LEN = 5;
        public const uint TXT_DEVICE_ID_MAX_LEN = 2;
        public const uint TXT_MSG_MAX_ARG_COUNT = 8;

        public const string STR_RHVER = "RHVER";    // Read hardware version.
        public const string STR_RFVER = "RFVER";    // Read firmware version.
        public const string STR_RGPIO = "RGPIO";    // Read a value from a GPIO input pin.
        public const string STR_WGPIO = "WGPIO";    // Write a value to a GPIO output pin.
        public const string STR_EBTN = "EBTN";     // Button event.
        public const string STR_RTEMP = "RTEMP";    // Read temperature.
        public const string STR_RHUM = "RHUM";     // Read humidity.
        public const string STR_RTAH = "RTAH";     // Read temperature and humidity.
        public const string STR_BMODE = "BMODE";    // Set boot mode.
        public const string STR_RESET = "RESET";    // Reset a target device.
        public const string STR_RWI2C = "RWI2C";    // Read/write I2C.
        public const string STR_UNKNOWN = "UNKNOWN";
    }
}
