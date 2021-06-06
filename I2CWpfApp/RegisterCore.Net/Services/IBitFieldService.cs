using RegisterCore.Net.Models;
using System;
using System.Collections.ObjectModel;

namespace RegisterCore.Net.Services
{
    public interface IBitFieldService
    {
        ObservableCollection<BitFieldTemplate> GetBitFields();
        BitFieldTemplate GetBitField(UInt64 id);
        void DeleteBitField(UInt64 id);
        void AddBitField(BitFieldTemplate bitField);
        void UpdateBitField(BitFieldTemplate bitField);
    }
}
