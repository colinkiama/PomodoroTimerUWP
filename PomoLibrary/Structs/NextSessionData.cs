using PomoLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomoLibrary.Structs
{
    public struct NextSessionData
    {
        public PomoSessionType NextSessionType { get; set; }
        public PomoSessionState NextSessionState { get; set; }
        public TimeSpan NextSessionLength { get; set; }
    }
}
