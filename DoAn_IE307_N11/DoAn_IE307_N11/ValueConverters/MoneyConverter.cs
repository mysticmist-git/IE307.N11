using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DoAn_IE307_N11.ValueConverters
{
    public class MoneyConverter : BaseValueConverter<MoneyConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var money = (int)value;

            if (money < 0)
                return -money;

            return money;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
