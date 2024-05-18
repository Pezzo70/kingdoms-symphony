using System;
using System.Collections.Generic;
using System.Linq;
using Kingdom.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{

    private string[] supportedResolution;
    private string resolution;
    private bool fullscreen = true;
    private float effectVolume = 0.0f;
    private float ambienceVolume = 0.0f;
    private float musicVolume = 0.0f;
    private float instrumentVolume = 0.0f;



    void Start()
    {
        var dropdown = GameObject.Find("Resolution").GetComponentInChildren<TMP_Dropdown>();
        supportedResolution = GetSupportedResolutions().Select(s => s.ToString()).ToArray();
        dropdown.options = supportedResolution.Select(res => new TMP_Dropdown.OptionData(res)).ToList();
        dropdown.value = supportedResolution.ToList().FindIndex(i => i == Screen.currentResolution.ToString());
    }

    public IEnumerable<Resolution> GetSupportedResolutions() => Screen.resolutions;

    public void ApplyResolution()
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
        AudioSystem.Instance.SetAmbienceVolume(ambienceVolume);
        AudioSystem.Instance.SetInstrumentVolume(instrumentVolume);
        AudioSystem.Instance.SetMusicVolume(musicVolume);
        AudioSystem.Instance.SetEffectVolume(effectVolume);
        ApplyResolution();
    }

    public void SetEffectVolume(float volume) => this.effectVolume = volume / 100;
    public void SetMusicVolume(float volume) => this.musicVolume = volume / 100;
    public void SetAmbienceVolume(float volume) => this.ambienceVolume = volume / 100;
    public void SetInstrumentVolume(float volume) => this.instrumentVolume = volume / 100;

    public void SetFullscreen(bool fullscreen) => this.fullscreen = fullscreen;
    public void SetResolution(int resolutionIndex) => this.resolution = supportedResolution[resolutionIndex];
}
