using System.Collections.Generic;
using System.Windows.Controls;

namespace I2CWpfApp.AppControl
{
    /// <summary>
    /// Interaction logic for Uc8Bit.xaml
    /// </summary>
    public partial class Uc8Bit : UserControl
    {
        private List<UcBitFieldBit> _bits = new List<UcBitFieldBit>();

        public List<UcBitFieldBit> Bits { get => _bits; }

        public Uc8Bit()
        {
            InitializeComponent();

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
