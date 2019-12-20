using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace PomoLibrary.Converters
{
    public class ArrayToProgressFraction:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string valueToReturn = "0/0";
            if (value is int[] progressArray)
            {
                valueToReturn = $"{progressArray[0]} / {progressArray[1]}";
            }

            return valueToReturn;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
