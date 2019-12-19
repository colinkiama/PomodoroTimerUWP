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

        private PomoSessionState _currentState;
        // Singleton Pattern with "Lazy"
        private static Lazy<SessionService> lazy =
            new Lazy<SessionService>(() => new SessionService());

        public static SessionService Instance => lazy.Value;

        private SessionService()
        {
            _currentState = PomoSessionState.InProgress;
        }

        public event EventHandler<PomoSessionState> SessionStateChanged;

        public void UpdateSessionState(PomoSessionState newState)
        {
            _currentState = newState;
            SessionStateChanged?.Invoke(this, _currentState);
        }

        internal PomoSessionState GetSessionState()
        {
            return _currentState;
        }
    }
}
