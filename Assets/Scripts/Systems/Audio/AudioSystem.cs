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
    [Range(0,1)]
    private float globalVolume;
    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        globalVolume = 1.0f;
    }

    void OnEnable() {
    SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
    SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded (Scene scene, LoadSceneMode mode) => this.Play(scene);

    public void Play(UIAction action) 
 { 
    var audio = audioContainer.GetByType<UIAudio>().First(a => a.Action == action);
    audioSource.PlayOneShot(audio.AudioClip, audio.volume * globalVolume);
 }
    
    public void Play(Scene stage)
    {
        audioSource.Stop();
        var audio = audioContainer.GetByType<StageAudio>().FirstOrDefault(a => a.StageName == stage.name) ?? audioContainer.GetByType<StageAudio>().First();
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

