using System.Collections.Generic;

namespace Fl.Net.Message
{
    public interface IFlMessage
    {
        FlMessageType MessageType { get; }
        FlMessageCategory MessageCategory { get; }
        FlMessageId MessageId { get; set; }
        List<object> Arguments { get; set; }
        byte[] Buffer { get; set; }
    }
}
