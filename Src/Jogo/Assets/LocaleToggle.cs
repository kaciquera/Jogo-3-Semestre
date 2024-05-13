using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using System.Linq;

namespace Game
{
    public class LocaleToggle : ToggleBehaviour
    {
        [SerializeField] private LocaleName language;

        private void Start()
        {
            if (LocalizationSettings.SelectedLocale.LocaleName.StartsWith(language.ToString()))
            {
                toggle.SetIsOnWithoutNotify(true);
            }
        }

        protected override void OnToggle(bool isOn)
        {
            if (!isOn) return;
            Locale locale = LocalizationSettings.AvailableLocales.Locales.FirstOrDefault(x => x.LocaleName.StartsWith(language.ToString()));
            LocalizationSettings.SelectedLocale = locale;
        }
    }
}
