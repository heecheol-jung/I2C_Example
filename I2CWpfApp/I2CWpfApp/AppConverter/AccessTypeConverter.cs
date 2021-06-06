using RegisterCore.Net.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace I2CWpfApp.AppConverter
{
    public class AccessTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BitAccessType bat)
            {
                switch (bat)
                {
                    case BitAccessType.Reserved:
                        return AppConstant.STR_RES;

                    case BitAccessType.ReadWrite:
                        return AppConstant.STR_RW;

                    case BitAccessType.ReadOnly:
                        return AppConstant.STR_R;

                    case BitAccessType.WriteOnly:
                        return AppConstant.STR_W;

                    case BitAccessType.ReadClear0:
                        return AppConstant.STR_RC_W0;

                    case BitAccessType.ReadClear1:
                        return AppConstant.STR_RC_W1;

                    case BitAccessType.ReadClearByRead:
                        return AppConstant.STR_RC_R;

                    case BitAccessType.ReadSetByRead:
                        return AppConstant.STR_RS;
                }

                return AppConstant.STR_UNKNOWN;
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string strValue)
            {
                switch (strValue)
                {
                    case AppConstant.STR_RES:
                        return BitAccessType.Reserved;

                    case AppConstant.STR_RW:
                        return BitAccessType.ReadWrite;

                    case AppConstant.STR_R:
                        return BitAccessType.ReadOnly;

                    case AppConstant.STR_W:
                        return BitAccessType.WriteOnly;

                    case AppConstant.STR_RC_W0:
                        return BitAccessType.ReadClear0;

                    case AppConstant.STR_RC_W1:
                        return BitAccessType.ReadClear1;

                    case AppConstant.STR_RC_R:
                        return BitAccessType.ReadClearByRead;

                    case AppConstant.STR_RS:
                        return BitAccessType.ReadSetByRead;
                }
            }

            return BitAccessType.Unknown;
        }
    }
}
