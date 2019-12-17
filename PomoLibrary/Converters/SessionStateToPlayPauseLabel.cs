using PomoLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace PomoLibrary.Converters
{
    public class SessionStateToPlayPauseLabel:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string label = "Play";
            if (value is PomoSessionState sessionState)
            {
                switch (sessionState)
                {
                    case PomoSessionState.InProgress:
                        label = "Pause";
                        break;
                }
            }

            return label;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
