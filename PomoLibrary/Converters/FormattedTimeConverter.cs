using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace PomoLibrary.Converters
{
    public class FormattedTimeConverter : IValueConverter
    {
        int secondsInMinute = 60;
        int secondsInHour = 3600;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            TimeSpan timeToPrint = TimeSpan.FromSeconds(0);
            if (value is TimeSpan timeFromBinding)
            {
                timeToPrint = timeFromBinding;
            }

            StringBuilder sb = new StringBuilder();
            double timeInSeconds = timeToPrint.TotalSeconds;

            // Get Hours
            int hoursInTime = (int)(timeInSeconds / secondsInHour);

            if (hoursInTime > 0)
            {
                sb.Append($"{hoursInTime}:");
            }
            timeInSeconds -= hoursInTime * secondsInHour;


            // Get Minutes
            int minutesInTime = (int)(timeInSeconds / secondsInMinute);
            

            timeInSeconds -= minutesInTime * secondsInMinute;

            // Get seconds and leave the rest
            int wholeSecondsLeft = (int)timeInSeconds;

            // Now outputs the values as a string in the correct format
            string timeString;
            if (hoursInTime > 0)
            {
                timeString = string.Format("{0:D2}:{1:D2}:{2:D2}", hoursInTime, minutesInTime, wholeSecondsLeft);
            }
            else
            {
                timeString = string.Format("{0:D2}:{1:D2}", minutesInTime, wholeSecondsLeft);
            }
            
            return timeString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
