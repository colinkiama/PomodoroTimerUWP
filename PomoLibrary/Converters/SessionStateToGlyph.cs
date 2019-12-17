using PomoLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace PomoLibrary.Converters
{
    public class SessionStateToGlyph:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {

            string glpyhValue = "\xE768";
            if (value is PomoSessionState sessionState)
            {
                switch (sessionState)
                {
                    case PomoSessionState.InProgress:
                        glpyhValue = "\xE769"; 
                        break;
                }
            }

            return glpyhValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
