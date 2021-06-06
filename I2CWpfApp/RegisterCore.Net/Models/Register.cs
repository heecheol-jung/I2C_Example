using System;
using System.Collections.ObjectModel;

namespace RegisterCore.Net.Models
{
    public class Register : RegisterTemplate
    {
        public Register(RegisterTemplate template)
        {
            Id = template.Id;
            Name = template.Name;
            FullName = template.FullName;
            Address = template.Address;
            Bits = template.Bits;
            ResetValue = template.ResetValue;
            ChipId = template.ChipId;
            Description = template.Description;
        }

        public UInt64 Value { get; set; }
        public new ObservableCollection<BitField> BitFields { get; set; } = new ObservableCollection<BitField>();
    }
}
