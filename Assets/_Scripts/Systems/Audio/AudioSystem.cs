using System.Collections.Generic;
using System.Linq;
using Kingdom.Audio.Procedural;
using Kingdom.Enums;
using Kingdom.Enums.Enemies;
using Kingdom.Enums.FX;
using Kingdom.Enums.Player;
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
        private float globalVolume, ambienceVolume, effectVolume, musicVolume = 1.0f;
        private ScriptableAudio currentAudio;

        [SerializeField]
        private Instrument instrument;

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
                audio.volume * (audio.StageName == "Menu" ? musicVolume : ambienceVolume) * 100;
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

        public void Play(IList<Note> notes) => instrument.QueueKey(Note.ToKeysPlayed(notes));

        public void Play(FXID effect) =>
            effectSource.PlayOneShot(
                audioContainer.GetFirstByType<FXAudio>(so => so.fxID == effect).AudioClip,
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
            if (instrument is null)
                return;
            instrument.SetVolume(volume);
        }

        public void SetAmbienceVolume(float volume)
        {
            ambienceVolume = volume;
            audioSource.volume = currentAudio.volume * ambienceVolume * ambienceVolume;
        }

        public void SetMusicVolume(float volume)
        {
            musicVolume = volume;
            audioSource.volume = currentAudio.volume * musicVolume * ambienceVolume;
        }

        public void SetEffectVolume(float volume) => effectVolume = volume;
    }
}
