using RegisterCore.Net.Models;
using System;
using System.Collections.ObjectModel;

namespace RegisterCore.Net.Services
{
    public interface IManufacturerService
    {
        ObservableCollection<Manufacturer> GetManufacturers();
        Manufacturer GetManufacturer(UInt64 id);
        void DeleteManufacturer(UInt64 id);
        void AddManufacturer(Manufacturer manufacturer);
        void UpdateManufacturer(Manufacturer manufacturer);
    }
}
