
using System;

namespace RegisterCore.Net.Models
{
    // STM32L0x3 Reference manual
    public enum BitAccessType
    {
        Unknown,

        /// <summary>
        /// Reserved bit, must be kept at reset value(Res.).
        /// </summary>
        Reserved,

        /// <summary>
        /// Software can read and write to this bit(rw).
        /// </summary>
        ReadWrite,

        /// <summary>
        /// Software can only read this bit(r).
        /// </summary>
        ReadOnly,

        /// <summary>
        /// Software can only write to this bit. 
        /// Reading this bit returns the reset value(w).
        /// </summary>
        WriteOnly,

        /// <summary>
        /// Software can read as well as clear this bit by writing 0. 
        /// Writing 1 has no effect on the bit value(rc_w0).
        /// </summary>
        ReadClear0,

        /// <summary>
        /// Software can read as well as clear this bit by writing 1. 
        /// Writing 0 has no effect on the bit value(rc_w1).
        /// </summary>
        ReadClear1,

        /// <summary>
        /// Software can read this bit. 
        /// Reading this bit automatically clears it to 0. 
        /// Writing this bit has no effect on the bit value(rc_r).
        /// </summary>
        ReadClearByRead,

        /// <summary>
        /// Software can read as well as set this bit. 
        /// Writing 0 has no effect on the bit value(rs).
        /// </summary>
        ReadSetByRead
    }

    public class BitFieldTemplate : BaseModel
    {
        public string Name { get; set; }
        public int Offset { get; set; }
        public int Bits { get; set; }
        public BitAccessType AccessType { get; set; }
        public string ResetValue { get; set; }
        public Int64 RegisterTemplateId { get; set; }
    }
}
