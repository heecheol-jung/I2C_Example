using System;

namespace RegisterCore.Net.Models
{
    public class BitField : BitFieldTemplate
    {
        public BitField(BitFieldTemplate template)
        {
            Id = template.Id;
            Name = template.Name;
            Offset = template.Offset;
            Bits = template.Bits;
            AccessType = template.AccessType;
            ResetValue = template.ResetValue;
            RegisterTemplateId = template.RegisterTemplateId;
            Description = template.Description;
        }

        public UInt64 Value { get; set; }
    }
}
