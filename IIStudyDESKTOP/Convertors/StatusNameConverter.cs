using LLStudy_Models.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace IIStudyDESKTOP
{
    class StatusNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] statusNames =
            {
                "Pending",  // 0 Pending
                "Processing",  // 1 Processing
                "Shipped",  // 2 Shipped
                "Delivered",  // 3 Delivered
                "Canceled",  // 4 Canceled
                "Refund"   // 5 Refund
            };
            OrderStatus status = (OrderStatus)int.Parse(value.ToString());
            switch (status)
            {
                case OrderStatus.Pending:
                    return statusNames[0];
                    break;
                case OrderStatus.Processing:
                    return statusNames[1];
                    break;
                case OrderStatus.Shipped:
                    return statusNames[2];
                    break;
                case OrderStatus.Delivered:
                    return statusNames[3];
                    break;
                case OrderStatus.Canceled:
                    return statusNames[4];
                    break;
                case OrderStatus.Refund:
                    return statusNames[5];
                    break;
                default:
                    return statusNames[0];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
