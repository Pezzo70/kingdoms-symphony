using System.Linq;
using Audio;
using UnityEditor.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class AudioSystem : PersistentSingleton<AudioSystem>
{
    private const string AudioPath = "Assets/Audio";

    [SerializeField]
    private ScriptableContainer audioContainer;
    private AudioSource audioSource;
    private EventSystem eventSystem;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        eventSystem = EventSystem.current;
        Debug.Log(EventSystem.current);
        this.Subscribe();
    }

    private void Subscribe()
    {
        ((InputSystemUIInputModule)eventSystem.currentInputModule).submit.action.performed += delegate{ this.Play(UIAction.Submit); };
    }


    void Update()
    {

    }

    public void Play(UIAction action) => audioSource.PlayOneShot(audioContainer.GetByType<UIAudio>().First(a => a.Action == action).AudioClip);
    
}



public enum UIAction
{
    Hover, Submit, Cancel, PopUp, Pause, Return
}
