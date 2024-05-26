using System.Collections.Generic;
using System.Linq;
using Kingdom.Audio.Procedural;
using Kingdom.Enums;
using Kingdom.Enums.Enemies;
using Kingdom.Enums.FX;
using Kingdom.Enums.Player;
using Kingdom.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kingdom.Audio
{
    public class AudioSystem : PersistentSingleton<AudioSystem>
    {
        [SerializeField]
        private ScriptableContainer audioContainer;
        private AudioSource audioSource;

        [Range(0, 1)]
        private float globalVolume,
            ambienceVolume,
            effectVolume,
            musicVolume = 1.0f;
        private ScriptableAudio currentAudio;

        [SerializeField]
        public Instrument instrument;

        [SerializeField]
        private AudioSource effectSource;

        protected override void Awake()
        {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
            globalVolume = 1.0f;
            OnSceneLoaded(SceneManager.GetActiveScene());
        }

        public void OnSceneLoaded(Scene scene) => this.Play(scene);

        public void OnSceneUnloaded() => audioSource.Stop();

        public void Play(UIAction action)
        {
            var audio = audioContainer.GetByType<UIAudio>().First(a => a.Action == action);
            effectSource.PlayOneShot(audio.AudioClip, audio.volume * effectVolume);
        }

        public void Play(Scene stage)
        {
            audioSource.Stop();
            var audio =
                audioContainer
                    .GetByType<StageAudio>()
                    .FirstOrDefault(a => a.StageName == stage.name)
                ?? audioContainer.GetByType<StageAudio>().First();
            currentAudio = audio;
            audioSource.clip = audio.AudioClip;
            audioSource.volume =
                audio.volume * (audio.StageName == "MenuScene" ? musicVolume : ambienceVolume);
            audioSource.Play();
        }

        public void Play(EnemyID enemy, ActorAudioTypes actorAudioType) =>
            effectSource.PlayOneShot(
                audioContainer
                    .GetFirstByType<EnemyAudio>(
                        so => so.Enemy == enemy && so.AudioType == actorAudioType
                    )
                    .AudioClip
            );

        public void Play(CharacterID player, ActorAudioTypes actorAudioType) =>
            effectSource.PlayOneShot(
                audioContainer
                    .GetFirstByType<PlayerAudio>(
                        so => so.Player == player && so.AudioType == actorAudioType
                    )
                    .AudioClip,
                effectVolume
            );

        public void Play(IList<Note> notes) => instrument.QueueKey(notes.ToKeysPlayed());

        public void Play(FXID effect, FXState state) =>
            effectSource.PlayOneShot(
                audioContainer
                    .GetFirstByType<FXAudio>(so => so.fxID == effect && so.fxState == state)
                    .AudioClip,
                effectVolume
            );

        public void Play(EndState endState) =>
            effectSource.PlayOneShot(
                audioContainer
                    .GetFirstByType<EndGameAudio>(so => so.endState == endState)
                    .AudioClip,
                effectVolume
            );

        public void SetVolume(float volume)
        {
            this.audioSource.volume /= globalVolume;
            this.globalVolume = volume;
            this.audioSource.volume *= volume;
        }

        public void SetInstrumentVolume(float volume)
        {
            instrument.SetVolume(volume);
        }

        public void SetAmbienceVolume(float volume)
        {
            ambienceVolume = volume;
            if ((currentAudio is StageAudio stage) && stage.StageName != "MenuScene")
            {
                audioSource.volume = currentAudio.volume * ambienceVolume * ambienceVolume;
            }
        }

        public void SetMusicVolume(float volume)
        {
            musicVolume = volume;
            if ((currentAudio is StageAudio stage) && stage.StageName == "MenuScene")
                audioSource.volume = currentAudio.volume * musicVolume;
        }

        public void SetEffectVolume(float volume) => effectVolume = volume;
    }
}
