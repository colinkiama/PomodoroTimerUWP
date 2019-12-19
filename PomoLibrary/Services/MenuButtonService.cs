using PomoLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomoLibrary.Services
{
    public class MenuButtonService : Notifier
    {
        const string MenuGlyphString = "\xE700";
        const string BackGlyphString = "\xE830";

        public event EventHandler MenuClosed;
        public event EventHandler BackButtonClicked;


        // Singleton Pattern with "Lazy"
        private static Lazy<MenuButtonService> lazy =
            new Lazy<MenuButtonService>(() => new MenuButtonService());

        public static MenuButtonService Instance => lazy.Value;

        private string _currentButtonGlyph;

        public string CurrentButtonGlyph
        {
            get { return _currentButtonGlyph; }
            set
            {
                _currentButtonGlyph = value;
                NotifyPropertyChanged();
            }
        }

        private MenuButtonService()
        {
            CurrentButtonGlyph = MenuGlyphString;
        }


        public void CloseMenu()
        {
            MenuClosed?.Invoke(this, EventArgs.Empty);
        }

        public void GoBack()
        {
            BackButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        public void NavigatedToMenu()
        {
            CurrentButtonGlyph = MenuGlyphString;
        }

        public void NavigatedToMenuChild()
        {
            CurrentButtonGlyph = BackGlyphString;
        }


        public bool CheckIfAtMenuRoot()
        {
            return _currentButtonGlyph == MenuGlyphString;
        }
    }
}
