using System;
using UnityEngine;

public class TipOrScrollContainerPauseHandler : MonoBehaviour
{
    public GameObject scrollsMenu;
    public bool wasOpen;

    void Awake()
    {
        EventManager.PauseGame += HandlePause;
        EventManager.UnpauseGame += HandleUnpause;
    }

    void OnDestroy()
    {
        EventManager.PauseGame -= HandlePause;
        EventManager.UnpauseGame -= HandleUnpause;
    }

    public void SetWasOpen(bool value) => wasOpen = value;

    private void HandlePause()
    {
        if (wasOpen)
        {
            scrollsMenu.SetActive(false);
        }
    }

    private void HandleUnpause()
    {
        if (wasOpen)
        {
            scrollsMenu.SetActive(true);
        }
    }
}
