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

    [SerializeField]
    private ScriptableContainer audioContainer;
    private AudioSource audioSource;
    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(UIAction action) => audioSource.PlayOneShot(audioContainer.GetByType<UIAudio>().First(a => a.Action == action).AudioClip);
    
}



public enum UIAction
{
    Hover, Submit, Cancel, PopUp, Pause, Return
}
