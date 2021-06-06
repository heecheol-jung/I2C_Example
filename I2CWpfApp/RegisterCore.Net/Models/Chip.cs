using System;
using System.Collections.ObjectModel;

namespace RegisterCore.Net.Models
{
    public class Chip : BaseModel
    {
        public string Name { get; set; }
        public string ChipType { get; set; }    // Sensor, MCU, ...
        public string Comment { get; set; }

        public Int64 ManufacturerId { get; set; }

        public ObservableCollection<RegisterTemplate> Registers { get; set; } = new ObservableCollection<RegisterTemplate>();
    }
}
