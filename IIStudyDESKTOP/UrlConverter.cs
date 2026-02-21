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
            string placeHolder = "PlaceHolder.jpg";
            string url = null;
            string mode = parameter as string;
            switch(mode.ToLower()) {
                case "book":
                    {
                        if (value.ToString().ToLower() != "none")
                            url = $"http://localhost:5049/Images/BooksImages/{value.ToString()}";
                        else
                            url = $"http://localhost:5049/Images/BooksImages/{placeHolder}";
                        break;
                    }




                case "registered":
                    {
                        url = $"http://localhost:5049/api/Registered/GetProfileImage?registeredID={value.ToString()}";
                        break;
                    }
                    
            };
            
            //url = url + $"?v={Guid.NewGuid()}";
            return url ;
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
