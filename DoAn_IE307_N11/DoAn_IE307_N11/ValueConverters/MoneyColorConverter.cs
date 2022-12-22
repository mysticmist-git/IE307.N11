using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Forms;

namespace DoAn_IE307_N11.ValueConverters
{
    public class MoneyColorConverter : BaseValueConverter<MoneyColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isExpense = System.Convert.ToBoolean(value);

            if (isExpense)
                return Color.Red;
            else
                return Color.Green;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
