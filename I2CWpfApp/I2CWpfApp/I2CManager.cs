using Fl.Net;
using Fl.Net.Message;
using Fl.Net.Parser;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace I2CWpfApp
{
    public class I2CManager
    {
        const int MAX_BUF_LEN = 2048;

        #region Private Fields
        byte[] _rx_buf = new byte[MAX_BUF_LEN];
        int _rx_len;
        SerialPort _serialPort = new SerialPort();
        bool _isStarted = false;
        Thread _messageThread = null;
        bool _messageLoop = false;
        EventWaitHandle _serialEvent;
        FlTxtParser _appTxtParser = new FlTxtParser();
        IFlMessage _response = null;
        uint _deviceId = 1;
        #endregion

        #region Public Properties
        public UInt32 DeviceId { get; set; }
        public bool ResponseReceived { get; set; }
        public string ComPortName { get; set; }
        public FlTxtParser AppTxtParser => _appTxtParser;
        #endregion

        public void Start(string strComPortName)
        {
            _serialEvent = new EventWaitHandle(false, EventResetMode.AutoReset);

            _serialPort.PortName = strComPortName;
            _serialPort.BaudRate = 115200;
            _serialPort.DataBits = 8;
            _serialPort.Parity = Parity.None;
            _serialPort.StopBits = StopBits.One;

            _serialPort.DataReceived += new SerialDataReceivedEventHandler(OnSerialPortDataReceived);

            _serialPort.Open();

            _messageLoop = true;
            _messageThread = new Thread(new ThreadStart(InternalMessageProc))
            {
                Priority = ThreadPriority.Highest
            };
            _messageThread.Start();

            _isStarted = true;
        }

        public void Stop()
        {
            _serialEvent.Set();
            _messageLoop = false;
            _messageThread.Join();

            CloseSerialPort();

            _serialEvent.Dispose();

            _isStarted = false;
        }

        public IFlMessage ReadRegister(ushort address, ushort regAddr)
        {
            IFlMessage message = null;

            message = new FlTxtMessageCommand()
            {
                MessageId = FlMessageId.ReadWriteI2C,
                Arguments = new List<object>()
                {
                    _deviceId.ToString(),   // DeviceID
                    "0",                    // Read mode
                    "1",                    // I2C number
                    $"{address}",           // Target I2C device address
                    $"{regAddr}"            // Register address
                }
            };
            FlTxtPacketBuilder.BuildMessagePacket(ref message);

            ResponseReceived = false;
            SendPacket(message.Buffer);

            if (WaitForResponse() == true)
            {
                return _response;
            }

            return null;
        }

        public IFlMessage WriteRegister(ushort address, ushort regAddr, UInt32 regValue)
        {
            IFlMessage message = null;

            message = new FlTxtMessageCommand()
            {
                MessageId = FlMessageId.ReadWriteI2C,
                Arguments = new List<object>()
                {
                    _deviceId.ToString(),   // DeviceID
                    "1",                    // Write mode
                    "1",                    // I2C number
                    $"{address}",           // Target I2C device address
                    $"{regAddr}",           // Register address
                    $"{regValue}"           // Register value
                }
            };
            FlTxtPacketBuilder.BuildMessagePacket(ref message);

            ResponseReceived = false;
            SendPacket(message.Buffer);

            if (WaitForResponse() == true)
            {
                return _response;
            }

            return null;
        }

        public bool IsStarted()
        {
            return _isStarted;
        }

        private void OnSerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            _serialEvent.Set();

            switch (e.EventType)
            {
                case SerialData.Chars:
                    break;

                case SerialData.Eof:
                    Log.Debug("EOF received");
                    break;
            }
        }

        private void CloseSerialPort()
        {
            if (_serialPort.IsOpen == true)
            {
                _serialPort.DataReceived -= OnSerialPortDataReceived;

                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();
                _serialPort.Close();

                Log.Debug("Serial port closed.");
            }
        }

        private void InternalMessageProc()
        {
            while (_messageLoop)
            {
                _serialEvent.WaitOne();

                if (!_messageLoop)
                {
                    break;
                }

                _rx_len = _serialPort.BytesToRead;
                if (_rx_len > 0)
                {
                    if (_rx_len > _rx_buf.Length)
                    {
                        _rx_len = _rx_buf.Length;
                    }

                    _serialPort.Read(_rx_buf, 0, _rx_len);

                    ProcessAppTxtMessage(_rx_len);
                }
            }
        }

        private void ProcessAppTxtMessage(int bytesToRead)
        {
            for (int i = 0; i < bytesToRead; i++)
            {
                FlParseState ret = _appTxtParser.ParseResponseEvent(_rx_buf[i], out _response);
                if (ret == FlParseState.ParseOk)
                {
                    ResponseReceived = true;

                    switch (_response.MessageId)
                    {
                        case FlMessageId.ReadFirmwareVersion:
                            ProcessAppTxtFwVerResponse(_response);
                            break;
                    }
                }
                else if (ret == FlParseState.ParseFail)
                {
                    Log.Debug("Application text response parser fail");
                }
            }
        }

        private void ProcessAppTxtFwVerResponse(IFlMessage response)
        {
            if (response.Arguments?.Count == 3)
            {
                Log.Information($"FW version : {(string)response.Arguments[2]}");
            }
            else
            {
                Log.Warning("Read firmware version failed");
            }
        }

        public void SendPacket(byte[] buf)
        {
            if (_isStarted != true)
            {
                Log.Warning("I2C manager is not started");
                return;
            }

            _serialPort.Write(buf, 0, buf.Length);
            Log.Information("Message sent");
        }

        private bool WaitForResponse()
        {
            int i = 0;

            while (true)
            {
                if (ResponseReceived == true)
                {
                    Log.Information("Response received");
                    return true;
                }
                Thread.Sleep(100);
                i++;
                if (i == 10)
                {
                    Log.Information("No response");
                    break;
                }
            }

            return false;
        }
    }
}
