using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using LLStudy_Models.Models;

namespace IIStudyDESKTOP
{
    public class StatusBackgroundConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           
            string[] statusColors =
            {
                "#fbbf24",  // 0 Pending
                "#667eea",  // 1 Processing
                "#0ea5e9",  // 2 Shipped
                "#22c55e",  // 3 Delivered
                "#ef4444",  // 4 Canceled
                "#a855f7"   // 5 Refund
            };
            OrderStatus status = (OrderStatus)int.Parse(value.ToString());
            switch (status)
            {
                case OrderStatus.Pending:
                    return statusColors[0];
                    break;
                case OrderStatus.Processing:
                    return statusColors[1];
                    break;
                case OrderStatus.Shipped:
                    return statusColors[2];
                    break;
                case OrderStatus.Delivered:
                    return statusColors[3];
                    break;
                case OrderStatus.Canceled:
                    return statusColors[4];
                    break;
                case OrderStatus.Refund:
                    return statusColors[5];
                    break;
                default:
                    return statusColors[0];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
