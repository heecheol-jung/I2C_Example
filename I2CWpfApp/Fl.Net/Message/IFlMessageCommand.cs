using System;
using System.Collections.Generic;

namespace Fl.Net.Message
{
    public interface IFlMessageCommand : IFlMessage
    {
        int MaxTryCount { get; set; }
        int TryCount { get; set; }
        int TryInterval { get; set; }
        int ResponseWaitTimeout { get; set; }
        bool ResponseExpected { get; set; }
        List<DateTime> SendTimeHistory { get; set; }
    }
}
