using Fl.Net.Message;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Net.Parser
{
    public class FlTxtParser
    {
        enum ReceiveState
        {
            MessageId,
            DeviceId,
            Data,
            Tail
        }

        #region Private Data
        private byte[] _buf = new byte[FlConstant.FL_TXT_MSG_MAX_LENGTH];
        private int _bufPos = 0;
        private byte[] _fullPacket = new byte[FlConstant.FL_TXT_MSG_MAX_LENGTH];
        private int _fullPacketLength = 0;
        private ReceiveState _receiveState;
        private StringBuilder _sb = new StringBuilder();
        private FlMessageId _msgId = FlMessageId.Unknown;
        private List<object> _arguments = new List<object>();
        #endregion

        #region Public Properties
        public object Context { get; set; } = null;
        public FlOnParseDone OnParseDone { get; set; } = null;
        #endregion

        #region Public Methods
        public FlParseState ParseCommand(byte data, out IFlMessage message)
        {
            FlParseState ret = FlParseState.Parsing;

            message = null;
            if (_fullPacketLength < FlConstant.FL_TXT_MSG_MAX_LENGTH)
            {
                _fullPacket[_fullPacketLength++] = data;
            }

            switch (_receiveState)
            {
                case ReceiveState.MessageId:
                    if (IsMsgIdChar(data) == true)
                    {
                        _buf[_bufPos++] = data;
                        if (_bufPos > FlConstant.TXT_MSG_ID_MAX_LEN)
                        {
                            ret = FlParseState.ParseFail;
                        }
                    }
                    else if (data == FlConstant.FL_TXT_MSG_ID_DEVICE_ID_DELIMITER)
                    {
                        _msgId = GetMessageId();
                        if (_msgId != FlMessageId.Unknown)
                        {
                            _receiveState = ReceiveState.DeviceId;
                            ClearReceiveBuffer();
                        }
                        else
                        {
                            ret = FlParseState.ParseFail;
                        }
                    }
                    else
                    {
                        ret = FlParseState.ParseFail;
                    }
                    break;

                case ReceiveState.DeviceId:
                    if (IsDeviceIdChar(data) == true)
                    {
                        _buf[_bufPos++] = data;
                        if (_bufPos > FlConstant.TXT_DEVICE_ID_MAX_LEN)
                        {
                            ret = FlParseState.ParseFail;
                        }
                    }
                    else if (data == FlConstant.FL_TXT_MSG_ARG_DELIMITER)
                    {
                        if (ProcessCommandData() == true)
                        {
                            if (IsCommandWithArgument(_msgId) == true)
                            {
                                _receiveState = ReceiveState.Data;
                                ClearReceiveBuffer();
                            }
                            else
                            {
                                ret = FlParseState.ParseFail;
                            }
                        }
                        else
                        {
                            ret = FlParseState.ParseFail;
                        }
                    }
                    else if (IsTail(data) == true)
                    {
                        if (ProcessCommandData() == true)
                        {
                            _receiveState = ReceiveState.Tail;
                            ret = FlParseState.ParseOk;
                        }
                        else
                        {
                            ret = FlParseState.ParseFail;
                        }
                    }
                    else
                    {
                        ret = FlParseState.ParseFail;
                    }
                    break;

                case ReceiveState.Data:
                    if (IsTail(data) != true)
                    {
                        if (data != FlConstant.FL_TXT_MSG_ARG_DELIMITER)
                        {
                            _buf[_bufPos++] = data;
                            if (_bufPos >= FlConstant.FL_TXT_MSG_MAX_LENGTH)
                            {
                                ret = FlParseState.ParseFail;
                            }
                        }
                        else
                        {
                            if (ProcessCommandData() == true)
                            {
                                ClearReceiveBuffer();
                            }
                            else
                            {
                                ret = FlParseState.ParseFail;
                            }
                        }
                    }
                    else
                    {
                        if (ProcessCommandData() == true)
                        {
                            _receiveState = ReceiveState.Tail;
                            ClearReceiveBuffer();
                            ret = FlParseState.ParseOk;
                        }
                        else
                        {
                            ret = FlParseState.ParseFail;
                        }
                    }
                    break;

                default:
                    ret = FlParseState.ParseFail;
                    break;
            }

            if (ret != FlParseState.Parsing)
            {
                if (ret == FlParseState.ParseOk)
                {
                    if (OnParseDone != null)
                    {
                        OnParseDone?.Invoke(this, null);
                    }
                    else
                    {
                        message = new FlTxtMessageCommand()
                        {
                            MessageId = _msgId,
                            Arguments = new List<object>()
                        };

                        while (_arguments.Count > 0)
                        {
                            message.Arguments.Add(_arguments[0]);
                            _arguments.RemoveAt(0);
                        }

                    }
                }

                Clear();
            }

            return ret;
        }

        public FlParseState ParseResponseEvent(byte data, out IFlMessage message)
        {
            FlParseState ret = FlParseState.Parsing;

            message = null;
            if (_fullPacketLength < FlConstant.FL_TXT_MSG_MAX_LENGTH)
            {
                _fullPacket[_fullPacketLength++] = data;
            }

            switch (_receiveState)
            {
                case ReceiveState.MessageId:
                    if (IsMsgIdChar(data) == true)
                    {
                        _buf[_bufPos++] = data;
                        if (_bufPos > FlConstant.TXT_MSG_ID_MAX_LEN)
                        {
                            ret = FlParseState.ParseFail;
                        }
                    }
                    else if (data == FlConstant.FL_TXT_MSG_ID_DEVICE_ID_DELIMITER)
                    {
                        _msgId = GetMessageId();
                        if (_msgId != FlMessageId.Unknown)
                        {
                            _receiveState = ReceiveState.DeviceId;
                            ClearReceiveBuffer();
                        }
                        else
                        {
                            ret = FlParseState.ParseFail;
                        }
                    }
                    else
                    {
                        ret = FlParseState.ParseFail;
                    }
                    break;

                case ReceiveState.DeviceId:
                    if (IsDeviceIdChar(data) == true)
                    {
                        _buf[_bufPos++] = data;
                        if (_bufPos > FlConstant.TXT_DEVICE_ID_MAX_LEN)
                        {
                            ret = FlParseState.ParseFail;
                        }
                    }
                    else if (data == FlConstant.FL_TXT_MSG_ARG_DELIMITER)
                    {
                        if (ProcessResponseEventData() == true)
                        {
                            _receiveState = ReceiveState.Data;
                            ClearReceiveBuffer();
                        }
                        else
                        {
                            ret = FlParseState.ParseFail;
                        }
                    }
                    else if (IsTail(data) == true)
                    {
                        if (ProcessResponseEventData() == true)
                        {
                            _receiveState = ReceiveState.Tail;
                            ret = FlParseState.ParseOk;
                        }
                        else
                        {
                            ret = FlParseState.ParseFail;
                        }
                    }
                    else
                    {
                        ret = FlParseState.ParseFail;
                    }
                    break;

                case ReceiveState.Data:
                    if (IsTail(data) != true)
                    {
                        if (data != FlConstant.FL_TXT_MSG_ARG_DELIMITER)
                        {
                            _buf[_bufPos++] = data;
                            if (_bufPos >= FlConstant.FL_TXT_MSG_MAX_LENGTH)
                            {
                                ret = FlParseState.ParseFail;
                            }
                        }
                        else
                        {
                            if (ProcessResponseEventData() == true)
                            {
                                ClearReceiveBuffer();
                            }
                            else
                            {
                                ret = FlParseState.ParseFail;
                            }
                        }
                    }
                    else
                    {
                        if (ProcessResponseEventData() == true)
                        {
                            _receiveState = ReceiveState.Tail;
                            ClearReceiveBuffer();
                            ret = FlParseState.ParseOk;
                        }
                        else
                        {
                            ret = FlParseState.ParseFail;
                        }
                    }
                    break;

                default:
                    ret = FlParseState.ParseFail;
                    break;
            }

            if (ret != FlParseState.Parsing)
            {
                if (ret == FlParseState.ParseOk)
                {
                    if (OnParseDone != null)
                    {
                        OnParseDone?.Invoke(this, null);
                    }
                    else
                    {
                        if (_msgId == FlMessageId.ButtonEvent)
                        {
                            message = new FlTxtMessageEvent()
                            {
                                MessageId = _msgId,
                                Arguments = new List<object>(),
                                ReceiveTime = DateTime.UtcNow
                            };
                        }
                        else
                        {
                            message = new FlTxtMessageResponse()
                            {
                                MessageId = _msgId,
                                Arguments = new List<object>(),
                                ReceiveTime = DateTime.UtcNow
                            };
                        }


                        while (_arguments.Count > 0)
                        {
                            message.Arguments.Add(_arguments[0]);
                            _arguments.RemoveAt(0);
                        }

                    }
                }

                Clear();
            }

            return ret;
        }

        public void Clear()
        {
            _msgId = FlMessageId.Unknown;
            //_deviceId = 0;
            _bufPos = 0;
            Array.Clear(_buf, 0, _buf.Length);
            _fullPacketLength = 0;
            Array.Clear(_fullPacket, 0, _fullPacket.Length);
            _receiveState = ReceiveState.MessageId;
            _arguments.Clear();
            _sb.Clear();
        }
        #endregion

        #region Private Methods
        private bool AddStringArgument()
        {
            if (GetStringData(out string stringData) == true)
            {
                _arguments.Add(stringData);
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool ProcessCommandData()
        {
            if (_arguments.Count >= FlConstant.TXT_MSG_MAX_ARG_COUNT)
            {
                return false;
            }

            if ((_msgId == FlMessageId.ReadHardwareVersion) ||
                (_msgId == FlMessageId.ReadFirmwareVersion) ||
                (_msgId == FlMessageId.Reset))
            {
                if (_arguments.Count < 1)
                {
                    return AddStringArgument();
                }
            }
            else if ((_msgId == FlMessageId.ReadGpio) ||
                     (_msgId == FlMessageId.BootMode) ||
                     (_msgId == FlMessageId.ReadTemperature) ||
                     (_msgId == FlMessageId.ReadHumidity) ||
                     (_msgId == FlMessageId.ReadTempAndHum))
            {
                if (_arguments.Count < 2)
                {
                    return AddStringArgument();
                }
            }
            else if (_msgId == FlMessageId.WriteGpio)
            {
                if (_arguments.Count < 3)
                {
                    return AddStringArgument();
                }
            }
            else if (_msgId == FlMessageId.ReadWriteI2C)
            {
                if (_arguments.Count < 7)
                {
                    return AddStringArgument();
                }
            }

            return false;
        }

        private bool ProcessResponseEventData()
        {
            if (_arguments.Count >= FlConstant.TXT_MSG_MAX_ARG_COUNT)
            {
                return false;
            }

            if ((_msgId == FlMessageId.ReadHardwareVersion) ||
                (_msgId == FlMessageId.ReadFirmwareVersion) ||
                (_msgId == FlMessageId.ButtonEvent))
            {
                if (_arguments.Count < 3)
                {
                    return AddStringArgument();
                }
            }
            else if ((_msgId == FlMessageId.ReadGpio) ||
                     (_msgId == FlMessageId.ReadTemperature) ||
                     (_msgId == FlMessageId.ReadHumidity))
            {
                if (_arguments.Count < 4)
                {
                    return AddStringArgument();
                }
            }
            else if ((_msgId == FlMessageId.WriteGpio) ||
                     (_msgId == FlMessageId.BootMode) ||
                     (_msgId == FlMessageId.Reset))
            {
                if (_arguments.Count < 2)
                {
                    return AddStringArgument();
                }
            }
            else if (_msgId == FlMessageId.ReadTempAndHum)
            {
                if (_arguments.Count < 5)
                {
                    return AddStringArgument();
                }
            }
            else if (_msgId == FlMessageId.ReadWriteI2C)
            {
                if (_arguments.Count < 7)
                {
                    return AddStringArgument();
                }
            }

            return false;
        }

        private bool GetStringData(out string stringData)
        {
            _sb.Clear();

            for (int i = 0; i < _bufPos; i++)
            {
                _sb.Append((char)_buf[i]);
            }

            stringData = _sb.ToString();

            return true;
        }

        private bool GetByteData(out byte byteData)
        {
            _sb.Clear();

            for (int i = 0; i < _bufPos; i++)
            {
                _sb.Append((char)_buf[i]);
            }

            return byte.TryParse(_sb.ToString(), out byteData);
        }

        private bool IsCommandWithArgument(FlMessageId msgId)
        {
            switch (msgId)
            {
                case FlMessageId.ReadGpio:
                case FlMessageId.WriteGpio:
                case FlMessageId.ReadTemperature:
                case FlMessageId.ReadHumidity:
                case FlMessageId.ReadTempAndHum:
                case FlMessageId.BootMode:
                case FlMessageId.ReadWriteI2C:
                    return true;
            }
            return false;
        }

        private bool GetUInt32Data(out UInt32 uint32Data)
        {
            _sb.Clear();

            for (int i = 0; i < _bufPos; i++)
            {
                _sb.Append((char)_buf[i]);
            }

            return UInt32.TryParse(_sb.ToString(), out uint32Data);
        }

        private bool SetDeviceId()
        {
            if (GetStringData(out string deviceId) == true)
            {
                //_deviceId = deviceId;
                _arguments.Add(deviceId);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsTail(byte data)
        {
            if (data == FlConstant.FL_TXT_MSG_TAIL)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsDeviceIdChar(byte data)
        {
            if ((data >= FlConstant.FL_TXT_DEVICE_ID_MIN_CHAR) &&
                (data <= FlConstant.FL_TXT_DEVICE_ID_MAX_CHAR))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ClearReceiveBuffer()
        {
            _bufPos = 0;
            Array.Clear(_buf, 0, _buf.Length);
        }

        private FlMessageId GetMessageId()
        {
            FlMessageId messageId = FlMessageId.Unknown;

            foreach (var item in FlTxtPacketBuilder.StringToMessageIdTable)
            {
                if (_bufPos == item.Key.Length)
                {
                    int i;

                    for (i = 0; i < _bufPos; i++)
                    {
                        if (_buf[i] != (byte)item.Key[i])
                        {
                            break;
                        }
                    }

                    if (i == _bufPos)
                    {
                        messageId = item.Value;
                        break;
                    }
                }
            }

            return messageId;
        }

        bool IsMsgIdChar(byte data)
        {
            if ((data >= FlConstant.FL_TXT_MSG_ID_MIN_CHAR) &&
                (data <= FlConstant.FL_TXT_MSG_ID_MAX_CHAR))
            {
                return true;
            }
            else
            {
                //return false;
                return IsDeviceIdChar(data);
            }
        }
        #endregion
    }
}
