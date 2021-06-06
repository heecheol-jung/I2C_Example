using RegisterCore.Net.Models;
using System;
using System.Collections.ObjectModel;

namespace RegisterCore.Net.Services
{
    public interface IRegisterService
    {
        ObservableCollection<Manufacturer> GetRegisters();
        RegisterTemplate GetRegister(UInt64 id);
        void DeleteRegister(UInt64 id);
        void AddRegister(RegisterTemplate register);
        void UpdateRegister(RegisterTemplate register);
    }
}
