using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DoAn_IE307_N11.ValueConverters
{
    public class TransactionNoteConverter : BaseValueConverter<TransactionNoteConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string note = value as string;

            if (string.IsNullOrEmpty(note))
                return "Thêm ghi chú";

            return note;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
