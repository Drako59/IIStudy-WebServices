using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace IIStudyDESKTOP
{
    public class StringToDateConverter : IValueConverter
    {
        private const string Format = "yyyy-MM-dd";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string dateText = value as string;

            if (string.IsNullOrWhiteSpace(dateText))
                return null;

            if (DateTime.TryParseExact(
                    dateText,
                    Format,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime date))
            {
                return date;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
                return date.ToString(Format);

            return null;
        }
    }
}
