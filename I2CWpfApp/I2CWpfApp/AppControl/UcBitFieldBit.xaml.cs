using System.Windows;
using System.Windows.Controls;

namespace I2CWpfApp.AppControl
{
    /// <summary>
    /// Interaction logic for UcBitFieldBit.xaml
    /// </summary>
    public partial class UcBitFieldBit : UserControl
    {
        bool _occupied = false;

        public string BitName
        {
            get
            {
                return (string)LblBitName.Content;
            }
            set
            {
                LblBitName.Content = value;
            }
        }

        public bool Occupied
        {
            get
            {
                return _occupied;
            }
            set
            {
                if (_occupied != value)
                {
                    if (value == true)
                    {
                        ChkBitUsage.BorderThickness = new Thickness(5);
                    }
                    else
                    {
                        ChkBitUsage.BorderThickness = new Thickness(1);
                    }
                    _occupied = value;
                }
            }
        }

        public UcBitFieldBit()
        {
            InitializeComponent();
        }
    }
}
