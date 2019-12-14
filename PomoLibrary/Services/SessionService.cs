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
        private SessionService _settingsService = null;
        private static Lazy<SessionService> lazy =
            new Lazy<SessionService>(() => new SessionService());

        public static SessionService Instance => lazy.Value;

        private PomoSessionState _currentSessionState = PomoSessionState.Stopped;

        private SessionService() { }

        public event EventHandler<PomoSessionState> SessionStateChanged;

        public void UpdateSessionState(PomoSessionState newState)
        {
            SessionStateChanged?.Invoke(this, newState);
        }
    }
}
