using PomoLibrary.Enums;
using PomoLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace PomoLibrary.Converters
{
    public class SessionTypeToString: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string typeAsString = "";
            if (value is PomoSessionType sessionType)
            {
                typeAsString = SessionStringHelper.GetSessionString(sessionType);
            }

            return typeAsString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
