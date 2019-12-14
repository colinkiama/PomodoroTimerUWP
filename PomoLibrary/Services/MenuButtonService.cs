using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomoLibrary.Services
{
    public class MenuButtonService
    {
        // Singleton Pattern with "Lazy"
        private static Lazy<MenuButtonService> lazy =
            new Lazy<MenuButtonService>(() => new MenuButtonService());

        public static MenuButtonService Instance => lazy.Value;

        private MenuButtonService() { }

        public event EventHandler MenuClosed;

        public void CloseMenu()
        {
            MenuClosed?.Invoke(this, EventArgs.Empty);
        }
    }
}
