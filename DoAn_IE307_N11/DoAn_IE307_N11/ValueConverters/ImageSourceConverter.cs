using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace DoAn_IE307_N11.ValueConverters
{
    public class ImageSourceConverter : BaseValueConverter<ImageSourceConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = value as string;

            if (path is null)
                return null;

            return ImageSource.FromFile(path);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
