using Fl.Net.Message;
using I2CWpfApp.AppControl;
using Microsoft.Win32;
using RegisterCore.Net;
using RegisterCore.Net.Models;
using RegisterSqlite.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace I2CWpfApp.AppWnd
{
    /// <summary>
    /// Interaction logic for WndI2CExample.xaml
    /// </summary>
    public partial class WndI2CExample : Window
    {
        private const string STR_VL6180X_REG_VALUE_FILE_NAME = "def_vl6180x_reg_values.txt";

        long _currentChipId = -1;
        int _bitFieldBits = 0;
        ObservableCollection<Register> _registers = new ObservableCollection<Register>();
        UcRegisterBitUsage _regBitUsage = new UcRegisterBitUsage();
        I2CManager _i2cMgr = new I2CManager();
        ushort _i2cAddr = 0x52;

        public WndI2CExample()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Use VL6180x chip for I2C read/write test.
            using (var ctx = new RegisterContext())
            {
                var chip = ctx.Chips.Where(c => c.Name == "VL6180x").SingleOrDefault();
                if (chip != null)
                {
                    _currentChipId = chip.Id;
                }
            }

            BtnRegisterRead.IsEnabled = false;
            BtnRegisterWrite.IsEnabled = false;

            // Load VL6180x register values and update register data grid.
            var regValues = AppUtil.ReadRegisterValuesFromFile(STR_VL6180X_REG_VALUE_FILE_NAME);
            UpdateRegisterDataGrid(regValues);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_i2cMgr.IsStarted())
            {
                _i2cMgr.Stop();
            }
        }

        // Load register values from a file.
        // File format
        // - Each line has register address(hex) and value(hex) pair.
        private void BtnRegisterValueLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                TbFIleName.Text = openFileDialog.SafeFileName;
                var regValues = AppUtil.ReadRegisterValuesFromFile(openFileDialog.FileName);

                UpdateRegisterDataGrid(regValues);
            }
        }

        // Export register values to a file.
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
                try
                {
                    AppUtil.SaveRegisterValues(saveFileDialog.FileName, _registers);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void DgRegister_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgRegister.SelectedIndex < 0)
            {
                DgRegisterBitField.ItemsSource = null;
                BitFieldPlaceHolder.Content = null;
                TbRegisterName.Text = "";
                TbRegisterValue.Text = "";
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
            }
            else
            {
                BitFieldPlaceHolder.Content = null;
                TbRegisterName.Text = "";
                TbRegisterValue.Text = "";
            }
        }

        // Add a new register value.
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

        // Remove a selected register value.
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

        private void DgRegisterBitField_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgRegisterBitField.SelectedIndex < 0)
            {
                TbBitFieldName.Text = "";
                TbBitFieldValue.Text = "";
                return;
            }

            BitField bf = (BitField)DgRegisterBitField.SelectedItem;
            if (bf != null)
            {
                TbBitFieldName.Text = $"{bf.Name}";
                TbBitFieldValue.Text = $"{bf.Value:X4}";
            }
        }

        // Apply a new value for a selected registr.
        // - Child bit field values will be updated.
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

        // Apply a new bit field value for a selected bit field of the selected register.
        // - Parent register value will be updated.
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

        // Open/close COM port.
        private void BtnComPortOpenClose_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TbComPortName.Text))
            {
                MessageBox.Show("Port name empty.");
                return;
            }

            try
            {
                string strOpenClose = (string)BtnComPortOpenClose.Content;
                switch (strOpenClose)
                {
                    case "Open":
                        _i2cMgr.Start(TbComPortName.Text);
                        break;

                    case "Close":
                        _i2cMgr.Stop();
                        break;
                }

                if (_i2cMgr.IsStarted() == true)
                {
                    BtnComPortOpenClose.Content = "Close";
                    BtnRegisterRead.IsEnabled = true;
                    BtnRegisterWrite.IsEnabled = true;
                }
                else
                {
                    BtnComPortOpenClose.Content = "Open";
                    BtnRegisterRead.IsEnabled = false;
                    BtnRegisterWrite.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // Read a value from the selected VL6180x register.
        private void BtnRegisterRead_Click(object sender, RoutedEventArgs e)
        {
            if (DgRegister.SelectedIndex < 0)
            {
                MessageBox.Show("No register selected.");
                return;
            }

            Register reg = (Register)DgRegister.SelectedItem;
            IFlMessage response = _i2cMgr.ReadRegister(_i2cAddr, (ushort)reg.Address);
            LbHistory.Items.Insert(0, "ReadRegister command sent");

            string strResp = "No response";
            if (response != null)
            {
                if ((response.Arguments?.Count > 0) &&
                    (response.Arguments?.Count == 7))
                {
                    strResp = "Invalid response(register value error)";
                    if (reg.Bits <= 8)
                    {
                        if (byte.TryParse((string)response.Arguments[6], out byte regValue))
                        {
                            strResp = string.Format("Resp : 0x{0:X4}, 0x{1:X2}", reg.Address, regValue);
                        }
                    }
                    else if (reg.Bits <= 16)
                    {
                        if (ushort.TryParse((string)response.Arguments[6], out ushort regValue))
                        {
                            strResp = string.Format("Resp : 0x{0:X4}, 0x{1:X4}", reg.Address, regValue);
                        }
                    }
                    else if (reg.Bits <= 32)
                    {
                        if (uint.TryParse((string)response.Arguments[6], out uint regValue))
                        {
                            strResp = string.Format("Resp : 0x{0:X4}, 0x{1:X8}", reg.Address, regValue);
                        }
                    }
                }
                else
                {
                    strResp = "Invalid response(argument error)";
                }
            }

            LbHistory.Items.Insert(0, strResp);
        }

        // Write a value to the selected VL6180x register.
        private void BtnRegisterWrite_Click(object sender, RoutedEventArgs e)
        {
            if (DgRegister.SelectedIndex < 0)
            {
                MessageBox.Show("No register selected.");
                return;
            }
            if (string.IsNullOrEmpty(TbRegisterValue.Text))
            {
                MessageBox.Show("Empty register value");
                return;
            }
            if (!ushort.TryParse(TbRegisterValue.Text, NumberStyles.HexNumber, null,  out ushort regValue))
            {
                MessageBox.Show("Invalid register value");
                return;
            }

            Register reg = (Register)DgRegister.SelectedItem;
            IFlMessage response = _i2cMgr.WriteRegister(_i2cAddr, (ushort)reg.Address, regValue);
            LbHistory.Items.Insert(0, "WriteRegister command sent");

            string strResp = "No response";
            if (response != null)
            {
                if ((response.Arguments?.Count > 0) &&
                    (response.Arguments?.Count == 2))
                {
                    strResp = "Register write OK";
                    if ((string)response.Arguments[1] == "1")
                    {
                        strResp = "Register write fail";
                    }
                }
                else
                {
                    strResp = "Invalid response(argument error)";
                }
            }

            LbHistory.Items.Insert(0, strResp);
        }

        private void BtnHistoryClear_Click(object sender, RoutedEventArgs e)
        {
            LbHistory.Items.Clear();
        }

        private void UpdateRegisterDataGrid(List<(ulong address, uint value)> regValues)
        {
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
}
