using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Kingdom.Audio.Procedural;
using UnityEngine;
using static Kingdom.Audio.Procedural.Frequencies;

namespace Kingdom.Audio.Procedural
{
    public abstract class Instrument : MonoBehaviour
    {
        [SerializeField]
        protected List<KeyPlayed> _keysPlayed;

        [SerializeField]
        protected ADSREnvelope ADSR;

        [SerializeField]
        protected float amplitude = 0.5f;

        [SerializeField]
        protected float _gain = 0.5f;

        [SerializeField]
        protected float _sampleRate;

        protected float _userVolume;

        public ReadOnlyCollection<KeyPlayed> KeysPlayed
        {
            get => _keysPlayed.AsReadOnly();
        }

        public virtual void KeyDown(KeyName keyName)
        {
            var key = _keysPlayed.FirstOrDefault(
                k =>
                    k.Name == keyName && k.TimeReleased != 0 && k.TimePlayed < AudioSettings.dspTime
            );

            if (key != null)
                key.TimePlayed = AudioSettings.dspTime;
            else
                _keysPlayed.Add(
                    new KeyPlayed { Name = keyName, TimePlayed = AudioSettings.dspTime }
                );
        }

        public virtual void KeyUp(KeyName keyName)
        {
            var keyToUpdate = _keysPlayed.FirstOrDefault(k => k.Name == keyName);
            if (keyToUpdate != null)
                keyToUpdate.TimeReleased = AudioSettings.dspTime;
        }

        public virtual void QueueKey(KeyPlayed key)
        {
            key.TimeReleased += AudioSettings.dspTime;
            key.TimePlayed += AudioSettings.dspTime;
            _keysPlayed.Add(key);
        }

        public virtual void QueueKey(IList<KeyPlayed> keys)
        {
            foreach (var key in keys)
            {
                key.TimeReleased += AudioSettings.dspTime;
                key.TimePlayed += AudioSettings.dspTime;
            }
            _keysPlayed.AddRange(keys);
        }

        public virtual void SetVolume(float volume)
        {
            _userVolume = volume;
            this.GetComponent<AudioSource>().volume = volume;
        }

        public abstract float WaveFunction(int dataIndex, double time, KeyName key);
    }

    [System.Serializable]
    public class KeyPlayed
    {
        public KeyName Name;
        public double TimePlayed;
        public double TimeReleased;
    }
}
