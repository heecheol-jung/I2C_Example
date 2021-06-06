using System;
using System.Collections.Generic;

namespace Fl.Net.Message
{
    public class FlTxtMessageResponse : IFlMessage
    {
        public FlMessageType MessageType => FlMessageType.Text;

        public FlMessageCategory MessageCategory => FlMessageCategory.Response;

        private FlMessageId _messageId = FlMessageId.Unknown;
        public FlMessageId MessageId { get => _messageId; set => _messageId = value; }

        private List<object> _arguments = null;
        public List<object> Arguments { get => _arguments; set => _arguments = value; }

        private byte[] _buffer = null;
        public byte[] Buffer { get => _buffer; set => _buffer = value; }

        private DateTime _receiveTime = DateTime.MinValue;
        public DateTime ReceiveTime { get => _receiveTime; set => _receiveTime = value; }

        public FlTxtMessageResponse()
        {
        }
    }
}
