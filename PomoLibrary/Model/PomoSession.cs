using PomoLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomoLibrary.Model
{
    public class PomoSession
    {
        PomoSessionState CurrentSessionState;

        public event EventHandler<TimeSpan> TimerTicked;
        public event EventHandler SessionCompleted;
        public event EventHandler<PomoSessionState> StateChanged;
    }
}
