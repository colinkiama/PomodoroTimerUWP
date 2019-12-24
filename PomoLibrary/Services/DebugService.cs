using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomoLibrary.Services
{
    public class DebugService
    {
        static string logString = "";
        public static void AddToLog(string lineToAdd)
        {
            logString += lineToAdd + "\n";
        }

        public static string GetLogString()
        {
            return logString;
        }
    }
}
