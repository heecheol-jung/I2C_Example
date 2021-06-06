using System.Collections.Generic;
using System.Windows.Controls;

namespace I2CWpfApp.AppControl
{
    /// <summary>
    /// Interaction logic for Uc32Bit.xaml
    /// </summary>
    public partial class Uc32Bit : UserControl
    {
        private List<UcBitFieldBit> _bits = new List<UcBitFieldBit>();

        public List<UcBitFieldBit> Bits { get => _bits; }

        public Uc32Bit()
        {
            InitializeComponent();

            _bits.Add(Bit31);
            _bits.Add(Bit30);
            _bits.Add(Bit29);
            _bits.Add(Bit28);
            _bits.Add(Bit27);
            _bits.Add(Bit26);
            _bits.Add(Bit25);
            _bits.Add(Bit24);
            _bits.Add(Bit23);
            _bits.Add(Bit22);
            _bits.Add(Bit21);
            _bits.Add(Bit20);
            _bits.Add(Bit19);
            _bits.Add(Bit18);
            _bits.Add(Bit17);
            _bits.Add(Bit16);

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
