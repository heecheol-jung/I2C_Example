using System.Collections.Generic;
using System.Windows.Controls;

namespace I2CWpfApp.AppControl
{
    /// <summary>
    /// Interaction logic for Uc16Bit.xaml
    /// </summary>
    public partial class Uc16Bit : UserControl
    {
        private List<UcBitFieldBit> _bits = new List<UcBitFieldBit>();

        public List<UcBitFieldBit> Bits { get => _bits; }

        public Uc16Bit()
        {
            InitializeComponent();

            _bits.Add(Bit15);
            _bits.Add(Bit14);
            _bits.Add(Bit13);
            _bits.Add(Bit12);
            _bits.Add(Bit11);
            _bits.Add(Bit10);
            _bits.Add(Bit9);
            _bits.Add(Bit8);
            _bits.Add(Bit7);
            _bits.Add(Bit6);
            _bits.Add(Bit5);
            _bits.Add(Bit4);
            _bits.Add(Bit3);
            _bits.Add(Bit2);
            _bits.Add(Bit1);
            _bits.Add(Bit0);
        }
    }
}
