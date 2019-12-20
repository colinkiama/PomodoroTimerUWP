using PomoLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomoLibrary.Helpers
{
    public static class SessionStringHelper
    {
        public static string GetSessionString(PomoSessionType sessionType)
        {
            string typeAsString = "";
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

            return typeAsString;
        }
    }
}
