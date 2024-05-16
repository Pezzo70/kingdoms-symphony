using Assets.SimpleLocalization.Scripts;
using Newtonsoft.Json;

namespace Kingdom.Player
{
    public class PlayerConfig
    {
        [JsonProperty]
        private string _languagePreferred;

        [JsonIgnore]
        public string LanguagePreferred
        {
            get => _languagePreferred;
        }

        public PlayerConfig() { }

        public void CreateNewPlayerConfig()
        {
            _languagePreferred = "Portuguese";
        }

        public void ApplyConfig()
        {
            LocalizationManager.Language = _languagePreferred;
        }

        public void SetLanguage(string language) => _languagePreferred = language;
    }
}
