using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    void Start()
    {
        foreach (var setting in Screen.resolutions)
            Debug.Log(setting.ToString());
    }

    public IEnumerable<Resolution> GetSupportedResolutions() => Screen.resolutions;

    public void SetResolution(int width, int height, bool fullscreen) =>
        Screen.SetResolution(width, height, fullscreen);
}
