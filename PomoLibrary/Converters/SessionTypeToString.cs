using PomoLibrary.Enums;
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
                switch (sessionType)
                {
                    case PomoSessionType.Work:
                        typeAsString = "Work";
                        break;
                    case PomoSessionType.Break:
                        typeAsString = "Break";
                        break;
                    case PomoSessionType.LongBreak:
                        typeAsString = "Long Break";
                        break;
                }
            }

            return typeAsString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
