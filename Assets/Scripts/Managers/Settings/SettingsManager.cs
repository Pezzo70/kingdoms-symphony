using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private bool fullscreen = true;
    void Start()
    {

    }
    
    void Update()
    {
        foreach (var setting in Screen.resolutions)
            SetResolution(setting.ToString());
    }
    public IEnumerable<Resolution> GetSupportedResolutions() => Screen.resolutions;

    public void SetResolution(string resolution)
    {
        string[] parts = resolution.Split(new string[] { "x", "@", "Hz" }, StringSplitOptions.RemoveEmptyEntries);
        int width = int.Parse(parts[0].Trim());
        int height = int.Parse(parts[1].Trim());
        int refreshRate = (int)Math.Ceiling(double.Parse(parts[2].Trim()));
        Screen.SetResolution(width, height, fullscreen);
    }

    public void SetVolume(float volume) => AudioSystem.Instance.SetVolume(volume);
}