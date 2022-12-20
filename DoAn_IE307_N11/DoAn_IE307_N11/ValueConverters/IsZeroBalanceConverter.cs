using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace DoAn_IE307_N11.ValueConverters
{
    public class IsZeroBalanceConverter : BaseValueConverter<IsZeroBalanceConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isZeroBalance = System.Convert.ToBoolean(value);

            if (isZeroBalance)
            {
                return Application.Current.Resources["LightForeground"];
            }

            return Application.Current.Resources["Foreground"];
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
