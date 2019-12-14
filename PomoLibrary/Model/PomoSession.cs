using PomoLibrary.Enums;
using PomoLibrary.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomoLibrary.Model
{
    public class PomoSession
    {
        public PomoSessionState CurrentSessionState { get; set; }
        public PomoSessionSettings SessionSettings { get; set; }
        public SessionTimer Timer { get; set; } 

        public event EventHandler<TimeSpan> TimerTicked;
        public event EventHandler SessionCompleted;
        public event EventHandler<PomoSessionState> StateChanged;

        public PomoSession()
        {

        }
    }
}
