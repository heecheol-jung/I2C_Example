using Microsoft.Win32;
using RegisterCore.Net;
using RegisterCore.Net.Models;
using RegisterSqlite.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using I2CWpfApp.AppControl;

namespace I2CWpfApp.AppWnd
{
    /// <summary>
    /// Interaction logic for WndRegisterValueEdit.xaml
    /// </summary>
    public partial class WndRegisterValue : Window
    {
        long _currentChipId = -1;
        int _bitFieldBits = 0;
        ObservableCollection<Register> _registers = new ObservableCollection<Register>();
        List<BitField> _bfTemplates = new List<BitField>();
        UcRegisterBitUsage _regBitUsage = new UcRegisterBitUsage();

        public WndRegisterValue()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var ctx = new RegisterContext())
            {
                var chip = ctx.Chips.Where(c => c.Name == "VL6180x").SingleOrDefault();
                if (chip != null)
                {
                    _currentChipId = chip.Id;
                }
            }
        }

        private void BtnRegisterValueLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                TbFIleName.Text = openFileDialog.SafeFileName;
                var regValues = AppUtil.ReadRegisterValuesFromFile(openFileDialog.FileName);

                _registers.Clear();

                using (var ctx = new RegisterContext())
                {
                    foreach (var item in regValues)
                    {
                        var reg = ctx.RegisterTemplates.Where(r => r.Address == item.address).FirstOrDefault();

                        if (reg != null)
                        {
                            var bitFields = new ObservableCollection<BitFieldTemplate>(ctx.BitFieldTemplates.Where(bf => bf.RegisterTemplateId == reg.Id).OrderBy(bf => bf.Offset).ToList());
                            reg.BitFields = bitFields;
                            Register regValue = new Register(reg);
                            regValue.Value = item.value;

                            foreach (var bf in reg.BitFields)
                            {
                                BitField b = new BitField(bf);
                                var val = GeneralUtil.GetBitFieldValue(item.value, bf);

                                b.Value = val;
                                regValue.BitFields.Add(b);
                            }
                            _registers.Add(regValue);
                        }
                    }

                    DgRegister.ItemsSource = _registers;
                }
            }
        }

        private void BtnRegisterValueExport_Click(object sender, RoutedEventArgs e)
        {
            if ((_registers == null) ||
                (_registers.Count <= 0))
            {
                MessageBox.Show("No register values.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                SaveRegisterValues(saveFileDialog.FileName, _registers);
            }
        }

        private void DgRegister_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (DgRegister.SelectedIndex < 0)
            {
                DgRegisterBitField.ItemsSource = null;
                BitFieldPlaceHolder.Content = null;
                TbRegisterName.Text = "";
                TbRegisterValue.Text = "";
                TbRegisterAddress.Text = "";
                return;
            }

            Register reg = (Register)DgRegister.SelectedItem;
            if (reg != null)
            {
                DgRegisterBitField.ItemsSource = reg.BitFields;

                _bitFieldBits = reg.Bits;
                //_regBitUsage.UpdateRegBitUsage(_bitFieldBits, reg.BitFields.OrderBy(bf => bf.Offset).ToList());
                _regBitUsage.UpdateRegBitUsage(_bitFieldBits, reg.BitFields.ToList());
                BitFieldPlaceHolder.Content = _regBitUsage;

                TbRegisterName.Text = reg.Name;
                TbRegisterValue.Text = $"{reg.Value:X4}";
                TbRegisterAddress.Text = $"{reg.Address:X4}";
            }
            else
            {
                BitFieldPlaceHolder.Content = null;
                TbRegisterName.Text = "";
                TbRegisterValue.Text = "";
                TbRegisterAddress.Text = "";
            }
        }

        private void BtnAddRegisterValue_Click(object sender, RoutedEventArgs e)
        {
            List<RegisterTemplate> regTemplates = null;
            using (var ctx = new RegisterContext())
            {
                regTemplates = ctx.RegisterTemplates.Where(r => r.ChipId == _currentChipId).ToList();
            }

            if (_registers?.Count > 0)
            {
                foreach (var regValue in _registers)
                {
                    var regTemplate = regTemplates.Where(r => r.Id == regValue.Id).SingleOrDefault();
                    if (regTemplate != null)
                    {
                        regTemplates.Remove(regTemplate);
                    }
                }
            }

            WndRegisterValueAdd wndRegValueAdd = new WndRegisterValueAdd(regTemplates);
            if (wndRegValueAdd.ShowDialog() == true)
            {
                _registers.Add(wndRegValueAdd.RegisterValue);
                if (DgRegister.ItemsSource == null)
                {
                    DgRegister.ItemsSource = _registers;
                }
            }
        }

        private void BtnRemoveRegisterValue_Click(object sender, RoutedEventArgs e)
        {
            if (DgRegister.SelectedIndex < 0)
            {
                MessageBox.Show("No register selected.");
                return;
            }

            Register reg = (Register)DgRegister.SelectedItem;
            _registers.Remove(reg);
        }

        private void DgRegisterBitField_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (DgRegisterBitField.SelectedIndex < 0)
            {
                TbBitFieldName.Text = "";
                TbBitFieldValue.Text = "";
                TbBitFieldOffset.Text = "";
                TbBitFieldBits.Text = "";
                TbBitFieldDescription.Text = "";
                return;
            }

            BitField bf = (BitField)DgRegisterBitField.SelectedItem;
            if (bf != null)
            {
                TbBitFieldName.Text = $"{bf.Name}";
                TbBitFieldValue.Text = $"{bf.Value:X4}";
                TbBitFieldOffset.Text = $"{bf.Offset}";
                TbBitFieldBits.Text = $"{bf.Bits}";
                TbBitFieldDescription.Text = $"{bf.Description}";
            }
        }

        private void BtnRegisterValueApply_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = DgRegister.SelectedIndex;
            if (selectedIndex < 0)
            {
                MessageBox.Show("No selected register.");
                return;
            }

            if (string.IsNullOrEmpty(TbRegisterValue.Text) == true)
            {
                MessageBox.Show("No register value.");
                return;
            }

            Register reg = (Register)DgRegister.SelectedItem;
            if (ulong.TryParse(TbRegisterValue.Text, System.Globalization.NumberStyles.HexNumber, null, out ulong regValue) == true)
            {
                reg.Value = regValue;

                foreach (var bf in reg.BitFields)
                {
                    var val = GeneralUtil.GetBitFieldValue(reg.Value, bf);

                    bf.Value = val;
                }

                DgRegister.ItemsSource = null;
                DgRegister.ItemsSource = _registers;
                DgRegister.SelectedIndex = selectedIndex;
                DgRegisterBitField.ItemsSource = null;
                DgRegisterBitField.ItemsSource = reg.BitFields;
            }
        }

        private void BtnBitFieldValueApply_Click(object sender, RoutedEventArgs e)
        {
            int selectedRegIndex = DgRegister.SelectedIndex;
            int selectedBfIndex = DgRegisterBitField.SelectedIndex;
            if (selectedBfIndex < 0)
            {
                MessageBox.Show("No selected bit field.");
                return;
            }

            if (string.IsNullOrEmpty(TbBitFieldValue.Text) == true)
            {
                MessageBox.Show("No bit field value.");
                return;
            }

            Register reg = (Register)DgRegister.SelectedItem;
            BitField bf = (BitField)DgRegisterBitField.SelectedItem;
            if (ulong.TryParse(TbBitFieldValue.Text, System.Globalization.NumberStyles.HexNumber, null, out ulong bfValue) == true)
            {
                if (bfValue > GeneralUtil.GetBitFieldMaxValue(bf))
                {
                    MessageBox.Show("Check maximum value.");
                    return;
                }
                bf.Value = bfValue;

                UInt64 calculatedVal = GeneralUtil.GetRegisterValueFromBitFields(reg.BitFields);

                reg.Value = calculatedVal;

                DgRegister.ItemsSource = null;
                DgRegister.ItemsSource = _registers;
                DgRegister.SelectedIndex = selectedRegIndex;
                DgRegisterBitField.ItemsSource = null;
                DgRegisterBitField.ItemsSource = reg.BitFields;
                DgRegisterBitField.SelectedIndex = selectedBfIndex;
            }
        }

        private void SaveRegisterValues(string fileName, ObservableCollection<Register> registers)
        {
            try
            {
                StreamWriter sw = new StreamWriter(fileName);

                foreach (var reg in registers)
                {
                    if (reg.Bits <= 8)
                    {
                        sw.WriteLine($"{reg.Address:X8},{reg.Value:X2}");
                    }
                    else if (reg.Bits <= 16)
                    {
                        sw.WriteLine($"{reg.Address:X8},{reg.Value:X4}");
                    }
                    else if (reg.Bits <= 32)
                    {
                        sw.WriteLine($"{reg.Address:X8},{reg.Value:X8}");
                    }
                    else
                    {
                        sw.WriteLine($"{reg.Address:X8},{reg.Value:X}");
                    }
                }

                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
