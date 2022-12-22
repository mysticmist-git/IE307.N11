using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DoAn_IE307_N11.ValueConverters
{
    public class TypeNameConverter : BaseValueConverter<TypeNameConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string name = value as string;

            if (string.IsNullOrEmpty(name))
                return "Chọn nhóm";

            return name;

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
