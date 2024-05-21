using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PauseHandler : MonoBehaviour
{
    public GameObject pauseMenu;
    public CanvasGroup playerHUDCanvasGroup;
    public InputSystemUIInputModule inputModule;
    private bool _onPause = false;
    private bool _cantPause = false;

    void OnEnable()
    {
        inputModule.cancel.action.performed += DisplayPauseMenu;
        EventManager.EndGameVictory += HandleEndGame;
        EventManager.EndGameDefeat += HandleEndGame;
        EventManager.PhaseVictory += HandleEndGame;
        EventManager.CantPause += HandleCanPause;
    }

    private void HandleCanPause(bool cantPause) => _cantPause = cantPause;

    void OnDisable()
    {
        inputModule.cancel.action.performed -= DisplayPauseMenu;
        EventManager.EndGameVictory -= HandleEndGame;
        EventManager.EndGameDefeat -= HandleEndGame;
        EventManager.PhaseVictory -= HandleEndGame;
        EventManager.CantPause -= HandleCanPause;
    }

    private void HandleEndGame()
    {
        _cantPause = true;
        pauseMenu.SetActive(false);
    }

    private void DisplayPauseMenu(InputAction.CallbackContext context)
    {
        if (_cantPause)
        {
            pauseMenu.SetActive(false);
            return;
        }

        if (_onPause && !pauseMenu.activeInHierarchy)
            return;

        pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        _onPause = pauseMenu.activeInHierarchy;
        if (_onPause)
        {
            playerHUDCanvasGroup.alpha = 0;
            EventManager.PauseGame?.Invoke();
        }
        else
        {
            playerHUDCanvasGroup.alpha = 1;
            EventManager.UnpauseGame?.Invoke();
        }
    }

    public void Unpause()
    {
        _onPause = false;
        EventManager.UnpauseGame?.Invoke();
        playerHUDCanvasGroup.alpha = 1;
    }
}
