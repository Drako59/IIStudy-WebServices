using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace IIStudyDESKTOP
{
    public class UrlConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string url = null;
            string mode = parameter as string;
            if (mode.ToLower() == "book")
                url = $"http://localhost:5049/Images/BooksImages/{value.ToString()}";
            else if (mode.ToLower() == "registered")
            {
                //string path = value.ToString().ToLower() == "none" ? "zoro2.jpg" : value.ToString();
                url = $"http://localhost:5049/api/Registered/GetProfileImage?registeredID={value.ToString()}";
            }
                return url;
            //string url = value as string;
            //if (string.IsNullOrEmpty(url))
            //    return null;
            //return new Uri(url);
        }
        

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Uri uri = value as Uri;
            if (uri == null)
                return null;
            return uri.ToString();
        }
    
    }
}
