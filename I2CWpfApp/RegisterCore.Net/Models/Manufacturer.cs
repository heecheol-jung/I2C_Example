using System.Collections.ObjectModel;

namespace RegisterCore.Net.Models
{
    public class Manufacturer : BaseModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string HomePage { get; set; }
        public string Comment { get; set; }
        public ObservableCollection<Chip> Chips { get; set; } = new ObservableCollection<Chip>();
    }
}
