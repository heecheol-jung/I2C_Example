using System;
using System.Collections.ObjectModel;

namespace RegisterCore.Net.Models
{
    public class RegisterTemplate : BaseModel
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public UInt64 Address { get; set; }
        public int Bits { get; set; }
        public string ResetValue { get; set; }
        public Int64 ChipId { get; set; }
        public ObservableCollection<BitFieldTemplate> BitFields { get; set; } = new ObservableCollection<BitFieldTemplate>();
    }
}
