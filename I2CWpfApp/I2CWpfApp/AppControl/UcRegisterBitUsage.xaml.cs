using RegisterCore.Net.Models;
using I2CWpfApp.AppConverter;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace I2CWpfApp.AppControl
{
    /// <summary>
    /// Interaction logic for UcRegisterBitUsage.xaml
    /// </summary>
    public partial class UcRegisterBitUsage : UserControl
    {
        Grid _gridRegister = null;
        List<UIElement> _dynamicElements = new List<UIElement>();

        public UcRegisterBitUsage()
        {
            InitializeComponent();
        }

        internal void UpdateRegBitUsage(int bitFieldBits, List<BitFieldTemplate> bfTemplates)
        {
            if (gridMain.Children.Count > 0)
            {
                if (_dynamicElements?.Count > 0)
                {
                    foreach (UIElement element in _dynamicElements)
                    {
                        if (_gridRegister.Children.Contains(element) == true)
                        {
                            _gridRegister.Children.Remove(element);
                        }
                    }

                    _dynamicElements.Clear();
                }

                gridMain.Children.Clear();
            }

            int colIndex, rowIndex;
            int columnCount = 8, rowCount = 1;
            int i = 0;

            _gridRegister = new Grid();
            if (bitFieldBits >= 16)
            {
                columnCount = 16;
                if (bitFieldBits > 16)
                {
                    rowCount = 2;
                }
            }
            
            // Add columns.
            for (colIndex = 0; colIndex < columnCount; colIndex++)
            {
                _gridRegister.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // Add rows.
            for (rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                RowDefinition rd = new RowDefinition()
                {
                    //MinHeight = 10,
                    Height = new GridLength(0.2, GridUnitType.Star)
                };
                _gridRegister.RowDefinitions.Add(rd);

                //_gridRegister.RowDefinitions.Add(new RowDefinition());
                _gridRegister.RowDefinitions.Add(new RowDefinition());
                //_gridRegister.RowDefinitions.Add(new RowDefinition());

                rd = new RowDefinition()
                {
                    //MinHeight = 10,
                    Height = new GridLength(0.2, GridUnitType.Star)
                };
                _gridRegister.RowDefinitions.Add(rd);
            }

            // Add bit numbers.
            i = bitFieldBits;
            List<Label> labels = new List<Label>();

            for (rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                for (colIndex = 0; colIndex < columnCount; colIndex++)
                {
                    Label lbl = new Label()
                    {
                        Content = $"{i-1}",
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    Grid.SetRow(lbl, rowIndex*3);
                    Grid.SetColumn(lbl, colIndex);
                    labels.Add(lbl);

                    i--;
                }
            }

            // Add bit fields.
            foreach (var bf in bfTemplates)
            {
                UpdateBitFieldInfo(bf, columnCount);
            }

            // Add access type labels.
            foreach (var l in labels)
            {
                _gridRegister.Children.Add(l);
            }
            _gridRegister.Margin = new Thickness(5);

            gridMain.Children.Add(_gridRegister);
        }

        internal void UpdateRegBitUsage(int bitFieldBits, List<BitField> bfTemplates)
        {
            if (gridMain.Children.Count > 0)
            {
                if (_dynamicElements?.Count > 0)
                {
                    foreach (UIElement element in _dynamicElements)
                    {
                        if (_gridRegister.Children.Contains(element) == true)
                        {
                            _gridRegister.Children.Remove(element);
                        }
                    }

                    _dynamicElements.Clear();
                }

                gridMain.Children.Clear();
            }

            int colIndex, rowIndex;
            int columnCount = 8, rowCount = 1;
            int i = 0;

            _gridRegister = new Grid();
            if (bitFieldBits >= 16)
            {
                columnCount = 16;
                if (bitFieldBits > 16)
                {
                    rowCount = 2;
                }
            }

            // Add columns.
            for (colIndex = 0; colIndex < columnCount; colIndex++)
            {
                _gridRegister.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // Add rows.
            for (rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                RowDefinition rd = new RowDefinition()
                {
                    //MinHeight = 10,
                    Height = new GridLength(0.2, GridUnitType.Star)
                };
                _gridRegister.RowDefinitions.Add(rd);

                //_gridRegister.RowDefinitions.Add(new RowDefinition());
                _gridRegister.RowDefinitions.Add(new RowDefinition());
                //_gridRegister.RowDefinitions.Add(new RowDefinition());

                rd = new RowDefinition()
                {
                    //MinHeight = 10,
                    Height = new GridLength(0.2, GridUnitType.Star)
                };
                _gridRegister.RowDefinitions.Add(rd);
            }

            // Add bit numbers.
            i = bitFieldBits;
            List<Label> labels = new List<Label>();

            for (rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                for (colIndex = 0; colIndex < columnCount; colIndex++)
                {
                    Label lbl = new Label()
                    {
                        Content = $"{i - 1}",
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    Grid.SetRow(lbl, rowIndex * 3);
                    Grid.SetColumn(lbl, colIndex);
                    labels.Add(lbl);

                    i--;
                }
            }

            // Add bit fields.
            foreach (var bf in bfTemplates)
            {
                UpdateBitFieldInfo(bf, columnCount);
            }

            // Add access type labels.
            foreach (var l in labels)
            {
                _gridRegister.Children.Add(l);
            }
            _gridRegister.Margin = new Thickness(5);

            gridMain.Children.Add(_gridRegister);
        }

        private void UpdateBitFieldInfo(BitFieldTemplate bf, int columnCount)
        {
            TextBlock[] textBlocks = null;
            Label[] labels = null;
            int endIndex = bf.Offset + bf.Bits - 1;
            int columnStart = bf.Offset % columnCount;
            int row = (bf.Offset / columnCount);
            AccessTypeConverter atConverter = new AccessTypeConverter();
            Type stringType = typeof(string);
            Thickness defThick = new Thickness(1);

            if ((bf.Offset >= 0 && bf.Offset <= 15) &&
                (endIndex >= 16 && endIndex <= 31))
            {
                textBlocks = new TextBlock[2];
                labels = new Label[2];

                textBlocks[0] = new TextBlock();
                textBlocks[0].Text = bf.Name;
                textBlocks[0].Background = Brushes.Gray;
                textBlocks[0].Margin = defThick;

                Grid.SetRow(textBlocks[0], 1);
                Grid.SetColumn(textBlocks[0], bf.Offset);
                Grid.SetColumnSpan(textBlocks[0], 15 - columnStart + 1);

                labels[0] = new Label();
                labels[0].Content = atConverter.Convert(bf.AccessType, stringType, null, null);
                labels[0].Margin = defThick;
                labels[0].HorizontalContentAlignment = HorizontalAlignment.Center;
                labels[0].BorderBrush = Brushes.Gray;
                labels[0].BorderThickness = new Thickness(1);

                Grid.SetRow(labels[0], 2);
                Grid.SetColumn(labels[0], bf.Offset);
                Grid.SetColumnSpan(labels[0], 15 - columnStart + 1);

                textBlocks[1] = new TextBlock();
                textBlocks[1].Text = bf.Name;
                textBlocks[1].Background = Brushes.Gray;
                textBlocks[1].Margin = defThick;

                Grid.SetRow(textBlocks[1], 4);
                Grid.SetColumn(textBlocks[1], 16 - (endIndex - 15));
                Grid.SetColumnSpan(textBlocks[1], endIndex - 15);

                labels[1] = new Label();
                labels[1].Content = atConverter.Convert(bf.AccessType, stringType, null, null);
                labels[1].Margin = defThick;
                labels[1].HorizontalContentAlignment = HorizontalAlignment.Center;
                labels[1].BorderBrush = Brushes.Gray;
                labels[1].BorderThickness = defThick;

                Grid.SetRow(labels[1], 5);
                Grid.SetColumn(labels[1], bf.Offset);
                Grid.SetColumnSpan(labels[1], 15 - columnStart + 1);

                _gridRegister.Children.Add(textBlocks[0]);
                _gridRegister.Children.Add(textBlocks[1]);
                _gridRegister.Children.Add(labels[0]);
                _gridRegister.Children.Add(labels[1]);
            }
            else
            {
                int rowIndex = 1;
                textBlocks = new TextBlock[1];
                labels = new Label[1];

                textBlocks[0] = new TextBlock();
                textBlocks[0].Text = bf.Name;
                textBlocks[0].Background = Brushes.Gray;
                textBlocks[0].Margin = defThick;

                if (bf.Offset > 15)
                {
                    rowIndex = 4;
                }
                
                Grid.SetRow(textBlocks[0], rowIndex);
                Grid.SetColumn(textBlocks[0], columnCount - columnStart - bf.Bits);
                Grid.SetColumnSpan(textBlocks[0], bf.Bits);

                labels[0] = new Label();
                labels[0].Content = atConverter.Convert(bf.AccessType, stringType, null, null);
                labels[0].Margin = defThick;
                labels[0].HorizontalContentAlignment = HorizontalAlignment.Center;
                labels[0].BorderBrush = Brushes.Gray;
                labels[0].BorderThickness = defThick;

                Grid.SetRow(labels[0], rowIndex+1);
                Grid.SetColumn(labels[0], columnCount - columnStart - bf.Bits);
                Grid.SetColumnSpan(labels[0], bf.Bits);

                _gridRegister.Children.Add(textBlocks[0]);
                _gridRegister.Children.Add(labels[0]);
            }

            _dynamicElements.AddRange(textBlocks);
            _dynamicElements.AddRange(labels);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
