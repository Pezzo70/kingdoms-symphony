using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PauseHandler : MonoBehaviour
{
    public GameObject pauseMenu;
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
    }

    public void Unpause() => _onPause = false;
    
}
