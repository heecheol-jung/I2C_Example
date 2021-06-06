using RegisterCore.Net.Models;
using System;
using System.Collections.Generic;

namespace RegisterCore.Net
{
    public static class GeneralUtil
    {
        public static UInt64 GetBitFieldValue(UInt64 regValue, BitFieldTemplate bitField)
        {
            UInt64 val = 0;
            UInt64 mask = 0;
            int i = 0;

            for (i = 0; i < bitField.Bits; i++)
            {
                mask |= ((UInt64)1 << i);
            }
            mask <<= bitField.Offset;

            val = (regValue & mask) >> bitField.Offset;
            
            return val;
        }

        public static UInt64 GetRegisterValueFromBitFields(IList<BitField> bitFields)
        {
            UInt64 val = 0;

            foreach (var bf in bitFields)
            {
                // TODO : Check offset.
                // Next bit offset = previous bit offset + previous bits
                val |= (bf.Value << bf.Offset);
            }

            return val;
        }

        public static ulong GetBitFieldMaxValue(BitFieldTemplate bfTemplate)
        {
            UInt64 val = 0;
            int i = 0;

            for (i = 0; i < bfTemplate.Bits; i++)
            {
                val |= ((UInt64)1 << i);
            }

            return val;
        }

        public static ulong GetMaxValueOfBits(int bits)
        {
            UInt64 val = 0;
            int i = 0;

            for (i = 0; i < bits; i++)
            {
                val |= ((UInt64)1 << i);
            }

            return val;
        }
    }
}
