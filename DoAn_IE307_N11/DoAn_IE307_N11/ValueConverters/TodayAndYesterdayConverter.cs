using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DoAn_IE307_N11.ValueConverters
{
    public class TodayAndYesterdayConverter : BaseValueConverter<TodayAndYesterdayConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTime = value as DateTime?;

            if (dateTime == null)
                return new DateTime();

            if (dateTime.Value.Date == DateTime.Now.Date)
                return "Hôm nay";
            
            if (dateTime.Value.Date == DateTime.Now.AddDays(-1).Date)
                return "Hôm qua";

            return dateTime;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
