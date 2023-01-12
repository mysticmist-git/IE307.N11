using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DoAn_IE307_N11.ValueConverters
{
    public class EventNameConverter : BaseValueConverter<EventNameConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value as string))
                return "Chọn sự kiện";

            return value as string;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
