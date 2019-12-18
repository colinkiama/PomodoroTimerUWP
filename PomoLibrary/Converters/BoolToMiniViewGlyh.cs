using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace PomoLibrary.Converters
{
    class BoolToMiniViewGlyh: IValueConverter
    {
        const string TurnOnMiniViewGlyph = "\xE2B3";
        const string TurnOffMiniViewGlyph = "\xE2B4";
        public object Convert(object value, Type targetType, object parameter, string language)
        {

            bool isMiniViewOn = false;
            if (value is bool hasMiniViewTurnedOn)
            {
                isMiniViewOn = hasMiniViewTurnedOn;
            }

            if (isMiniViewOn)
            {
                return TurnOffMiniViewGlyph;
            }
            else
            {
                return TurnOnMiniViewGlyph;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
