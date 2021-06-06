using System;
using System.Globalization;
using System.Windows.Data;

namespace I2CWpfApp.AppConverter
{
    public class HexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Int64)
            {
                Int64 input = (Int64)value;
                return string.Format("{0:X8}", input);
            }
            else if (value is UInt64)
            {
                UInt64 input = (UInt64)value;
                return string.Format("{0:X4}", input);
            }
            else if (value is Int32)
            {
                Int32 input = (Int32)value;
                if (((string)parameter) == "Offset")
                {
                    return string.Format("{0:X4}", input);
                }
                else
                {
                    return string.Format("{0:X4}", input);
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = (string)value;

            if (((string)parameter) == "Offset")
            {
                return Int32.Parse(input, NumberStyles.HexNumber);
            }
            else
            {
                return Int64.Parse(input, NumberStyles.HexNumber);
            }
        }
    }
}
