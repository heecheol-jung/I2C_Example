using RegisterCore.Net;
using RegisterCore.Net.Models;
using RegisterSqlite.Net;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace I2CWpfApp.AppWnd
{
    /// <summary>
    /// Interaction logic for WndRegisterValueAdd.xaml
    /// </summary>
    public partial class WndRegisterValueAdd : Window
    {
        private Register _registerValue = null;
        private List<RegisterTemplate> _regTemplates = null;

        public Register RegisterValue
        {
            get
            {
                return _registerValue;
            }
        }

        public WndRegisterValueAdd(List<RegisterTemplate> registerTemplates)
        {
            _regTemplates = registerTemplates;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DgRegister.ItemsSource = _regTemplates;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (DgRegister.SelectedIndex < 0)
            {
                MessageBox.Show("No register selected.");
                return;
            }

            if (ulong.TryParse(TbRegisterValue.Text, System.Globalization.NumberStyles.HexNumber, null, out ulong regValue) != true)
            {
                MessageBox.Show("No hexadecimal number.");
                return;
            }

            RegisterTemplate regTemplate = (RegisterTemplate)DgRegister.SelectedItem;
            if (regValue > GeneralUtil.GetMaxValueOfBits(regTemplate.Bits))
            {
                MessageBox.Show("Input value is greater than maximum value.");
                return;
            }

            _registerValue = new Register(regTemplate);
            _registerValue.Value = regValue;
            using (var ctx = new RegisterContext())
            {
                var bitFields = new ObservableCollection<BitFieldTemplate>(ctx.BitFieldTemplates.Where(bf => bf.RegisterTemplateId == _registerValue.Id).OrderBy(bf => bf.Offset).ToList());

                foreach (var bf in bitFields)
                {
                    BitField b = new BitField(bf);
                    var val = GeneralUtil.GetBitFieldValue(_registerValue.Value, bf);

                    b.Value = val;
                    _registerValue.BitFields.Add(b);
                }
            }

            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
