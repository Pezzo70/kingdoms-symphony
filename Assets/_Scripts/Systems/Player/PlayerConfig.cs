using Assets.SimpleLocalization.Scripts;
using Newtonsoft.Json;

namespace Kingdom.Player
{
    public class PlayerConfig
    {
        [JsonProperty]
        private string _languagePreferred;

        [JsonProperty]
        private bool _sawDisclaimer;

        [JsonIgnore]
        public string LanguagePreferred
        {
            get => _languagePreferred;
        }

        [JsonIgnore]
        public bool SawDisclaimer
        {
            get => _sawDisclaimer;
        }

        public PlayerConfig() { }

        public void CreateNewPlayerConfig()
        {
            _sawDisclaimer = false;
            _languagePreferred = "Portuguese";
        }

        public void ApplyConfig()
        {
            LocalizationManager.Language = _languagePreferred;
        }

        public void SetLanguage(string language)
        {
            _languagePreferred = language;
            EventManager.Save.Invoke();
        }

        public void SetSawDisclaimer(bool saw)
        {
            _sawDisclaimer = saw;
            EventManager.Save.Invoke();
        }
    }
}
