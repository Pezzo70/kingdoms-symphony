using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PauseHandler : MonoBehaviour
{
    public GameObject pauseMenu;
    public CanvasGroup playerHUDCanvasGroup;
    public InputSystemUIInputModule inputModule;
    private bool _onPause = false;

    void OnEnable()
    {
        inputModule.cancel.action.performed += DisplayPauseMenu;
    }

    void OnDisable()
    {
        inputModule.cancel.action.performed -= DisplayPauseMenu;
    }

    private void DisplayPauseMenu(InputAction.CallbackContext context)
    {
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
