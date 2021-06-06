using RegisterCore.Net.Models;
using System;
using System.Collections.ObjectModel;

namespace RegisterCore.Net.Services
{
    public interface IChipService
    {
        ObservableCollection<Chip> GetChips();
        Chip GetChip(UInt64 id);
        void DeleteChip(UInt64 id);
        void AddChip(Chip chip);
        void UpdateChip(Chip chip);
    }
}
