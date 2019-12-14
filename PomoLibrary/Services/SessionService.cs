using PomoLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomoLibrary.Services
{
    public class SessionService
    {
        // Singleton Pattern with "Lazy"
        private static Lazy<SessionService> lazy =
            new Lazy<SessionService>(() => new SessionService());

        public static SessionService Instance => lazy.Value;

        private SessionService() { }

        public event EventHandler<PomoSessionState> SessionStateChanged;

        public void UpdateSessionState(PomoSessionState newState)
        {
            SessionStateChanged?.Invoke(this, newState);
        }
    }
}
