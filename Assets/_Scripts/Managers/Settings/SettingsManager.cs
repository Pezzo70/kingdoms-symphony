using System;
using System.Collections.Generic;
using System.Linq;
using Kingdom.Audio;
using Kingdom.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private string[] supportedResolution;
    private string resolution;
    private bool fullscreen = true;
    private float musicVolume = 0.0f;
    private float instrumentVolume = 0.0f;
    private float effectVolume = 0.0f;
    private float ambienceVolume = 0.0f;

    public Slider musicVolumeSlider;
    public Slider instrumentVolumeSlider;
    public Slider ambienceVolumeSlider;
    public Slider effectVolumeSlider;
    public Toggle fullScreenToggle;
    public TMP_Dropdown resolutionDropdown;

    void OnEnable()
    {
        PlayerConfig playerConfig = PlayerContainer.Instance.playerConfig;
        musicVolumeSlider.value = playerConfig.MusicVolume * 100f;
        instrumentVolumeSlider.value = playerConfig.InstrumentVolume * 100f;
        effectVolumeSlider.value = playerConfig.EffectVolume * 100f;
        ambienceVolumeSlider.value = playerConfig.AmbienceVolume * 100f;
        fullScreenToggle.isOn = playerConfig.FullScreen;
        SetDropdown();
    }

    void Start()
    {
        SetDropdown();
    }

    private void SetDropdown()
    {
        if (resolutionDropdown.options.Count == 0)
        {
            supportedResolution = GetSupportedResolutions().Select(s => s.ToString()).ToArray();
            resolutionDropdown.options = supportedResolution
                .Select(res => new TMP_Dropdown.OptionData(res))
                .ToList();
        }

        resolutionDropdown.value = supportedResolution
            .ToList()
            .FindIndex(i => i == PlayerContainer.Instance.playerConfig.Resolution.ToString());
    }

    public IEnumerable<Resolution> GetSupportedResolutions() => Screen.resolutions;

    public static void ApplyResolution(string resolution, bool fullscreen)
    {
        string[] parts = resolution.Split(
            new string[] { "x", "@", "Hz" },
            StringSplitOptions.RemoveEmptyEntries
        );
        int width = int.Parse(parts[0].Trim());
        int height = int.Parse(parts[1].Trim());
        int refreshRate = (int)Math.Ceiling(double.Parse(parts[2].Trim()));
        Screen.SetResolution(width, height, fullscreen);
    }

    public void SetVolume(float volume) => AudioSystem.Instance.SetVolume(volume);

    public void Commit()
    {
        AudioSystem.Instance.SetMusicVolume(musicVolume);
        AudioSystem.Instance.SetInstrumentVolume(instrumentVolume);
        AudioSystem.Instance.SetEffectVolume(effectVolume);
        AudioSystem.Instance.SetAmbienceVolume(ambienceVolume);
        ApplyResolution(this.resolution, this.fullscreen);
        PlayerContainer
            .Instance
            .playerConfig
            .SetVolumeSettings(musicVolume, instrumentVolume, effectVolume, ambienceVolume);
        PlayerContainer.Instance.playerConfig.SetScreenSettings(resolution, fullscreen);
        EventManager.SavePlayerConfig();
    }

    public void SetEffectVolume(float volume) => this.effectVolume = volume / 100;

    public void SetMusicVolume(float volume) => this.musicVolume = volume / 100;

    public void SetAmbienceVolume(float volume) => this.ambienceVolume = volume / 100;

    public void SetInstrumentVolume(float volume) => this.instrumentVolume = volume / 100;

    public void SetFullscreen(bool fullscreen) => this.fullscreen = fullscreen;

    public void SetResolution(int resolutionIndex) =>
        this.resolution = supportedResolution[resolutionIndex];
}
