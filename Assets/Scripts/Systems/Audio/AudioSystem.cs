using System.Linq;
using Audio;
using Enemies;
using Enums;
using Player;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioSystem : PersistentSingleton<AudioSystem>
{
    [SerializeField]
    private ScriptableContainer audioContainer;
    private AudioSource audioSource;
<<<<<<< HEAD
    [Range(0,1)]
    private float globalVolume;
=======

>>>>>>> df63ddb42072881081443f08982041736188fe47
    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
<<<<<<< HEAD
        globalVolume = 1.0f;
=======
>>>>>>> df63ddb42072881081443f08982041736188fe47
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => this.Play(scene);

    public void Play(UIAction action)
    {
        var audio = audioContainer.GetByType<UIAudio>().First(a => a.Action == action);
        audioSource.PlayOneShot(audio.AudioClip, audio.volume);
    }

<<<<<<< HEAD
    public void Play(UIAction action) 
 { 
    var audio = audioContainer.GetByType<UIAudio>().First(a => a.Action == action);
    audioSource.PlayOneShot(audio.AudioClip, audio.volume * globalVolume);
 }
    
=======
>>>>>>> df63ddb42072881081443f08982041736188fe47
    public void Play(Scene stage)
    {
        audioSource.Stop();
        var audio = audioContainer.GetByType<StageAudio>().First(a => a.StageName == stage.name);
<<<<<<< HEAD
            audioSource.clip = audio.AudioClip;	
            audioSource.volume *= audio.volume;
        audioSource.Play();
    }

    public void Play(EnemyID enemy, ActorAudioTypes actorAudioType) => audioSource.PlayOneShot(audioContainer.GetFirstByType<EnemyAudio>(so => so.Enemy == enemy && so.AudioType == actorAudioType).AudioClip);
    public void Play(CharacterID player, ActorAudioTypes actorAudioType) => audioSource.PlayOneShot(audioContainer.GetFirstByType<PlayerAudio>(so => so.Player == player && so.AudioType == actorAudioType).AudioClip);

    public void SetVolume(float volume)
    {
        this.audioSource.volume /= globalVolume;
        this.globalVolume = volume;
        this.audioSource.volume *= volume;
    }
}



public enum UIAction
{
    Hover, Submit, Cancel, PopUp, Pause, Return
=======
        audioSource.clip = audio.AudioClip;
        audioSource.volume = audio.volume;
        audioSource.Play();
    }

    public void Play(EnemyID enemy, ActorAudioTypes actorAudioType) =>
        audioSource.PlayOneShot(
            audioContainer
                .GetFirstByType<EnemyAudio>(
                    so => so.Enemy == enemy && so.AudioType == actorAudioType
                )
                .AudioClip
        );

    public void Play(CharacterID player, ActorAudioTypes actorAudioType) =>
        audioSource.PlayOneShot(
            audioContainer
                .GetFirstByType<PlayerAudio>(
                    so => so.Player == player && so.AudioType == actorAudioType
                )
                .AudioClip
        );
>>>>>>> df63ddb42072881081443f08982041736188fe47
}
