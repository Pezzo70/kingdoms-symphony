using Assets.SimpleLocalization.Scripts;
using Kingdom.Audio;
using Newtonsoft.Json;
using UnityEngine;

namespace Kingdom.Player
{
    public class PlayerConfig
    {
        [JsonProperty]
        private string _languagePreferred;

        [JsonProperty]
        private bool _sawDisclaimer;

        [JsonProperty]
        private float _musicVolume;

        [JsonProperty]
        private float _instrumentVolume;

        [JsonProperty]
        private float _effectVolume;

        [JsonProperty]
        private float _ambienceVolume;

        [JsonProperty]
        private string _resolution;

        [JsonProperty]
        private bool _fullscreen;

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

        [JsonIgnore]
        public float MusicVolume
        {
            get => _musicVolume;
        }

        [JsonIgnore]
        public float InstrumentVolume
        {
            get => _instrumentVolume;
        }

        [JsonIgnore]
        public float EffectVolume
        {
            get => _effectVolume;
        }

        [JsonIgnore]
        public float AmbienceVolume
        {
            get => _ambienceVolume;
        }

        [JsonIgnore]
        public string Resolution
        {
            get => _resolution;
        }

        [JsonIgnore]
        public bool FullScreen
        {
            get => _fullscreen;
        }

        public PlayerConfig() { }

        public void CreateNewPlayerConfig()
        {
            _sawDisclaimer = false;
            _languagePreferred = "Portuguese";
            _musicVolume = 0.5f;
            _instrumentVolume = 0.75f;
            _effectVolume = 0.25f;
            _ambienceVolume = 0.5f;
            _resolution = Screen.currentResolution.ToString();
            _fullscreen = true;
        }

        public void ApplyConfig()
        {
            AudioSystem instance = AudioSystem.Instance;
            LocalizationManager.Language = _languagePreferred;
            instance.SetMusicVolume(_musicVolume);
            instance.SetInstrumentVolume(_instrumentVolume);
            instance.SetEffectVolume(_effectVolume);
            instance.SetAmbienceVolume(_ambienceVolume);
            SettingsManager.ApplyResolution(_resolution, _fullscreen);
        }

        public void SetLanguage(string language)
        {
            _languagePreferred = language;
            EventManager.SavePlayerConfig.Invoke();
        }

        public void SetSawDisclaimer(bool saw)
        {
            _sawDisclaimer = saw;
            EventManager.SavePlayerConfig.Invoke();
        }

        public void SetVolumeSettings(
            float musicVolume,
            float instrumentVolume,
            float effectVolume,
            float ambienceVolume
        )
        {
            _musicVolume = musicVolume;
            _instrumentVolume = instrumentVolume;
            _effectVolume = effectVolume;
            _ambienceVolume = ambienceVolume;
        }

        public void SetScreenSettings(string resolution, bool fullscreen)
        {
            _resolution = resolution;
            _fullscreen = fullscreen;
        }
    }
}
