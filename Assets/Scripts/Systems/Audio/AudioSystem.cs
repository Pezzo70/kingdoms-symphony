using System.Linq;
using Audio;
using Enemies;
using Enums;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;


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

    void OnEnable() {
    SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
    SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded (Scene scene, LoadSceneMode mode) => this.Play(scene);

    public void Play(UIAction action) => audioSource.PlayOneShot(audioContainer.GetByType<UIAudio>().First(a => a.Action == action).AudioClip);
    
    public void Play(Scene stage)
    {
        audioSource.Stop();
        //audioSource.clip = audioContainer.GetByType<StageAudio>().First(a => a.StageName == stage.name).AudioClip;
        audioSource.Play();
    }

    public void Play(EnemyID enemy, ActorAudioTypes actorAudioType) => audioSource.PlayOneShot(audioContainer.GetFirstByType<EnemyAudio>(so => so.Enemy == enemy && so.AudioType == actorAudioType).AudioClip);
    public void Play(CharacterID player, ActorAudioTypes actorAudioType) => audioSource.PlayOneShot(audioContainer.GetFirstByType<PlayerAudio>(so => so.Player == player && so.AudioType == actorAudioType).AudioClip);
}



public enum UIAction
{
    Hover, Submit, Cancel, PopUp, Pause, Return
}
