using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Kingdom.Audio.Procedural;
using Kingdom.Enums.Audio.Procedural;
using UnityEngine;
using static Kingdom.Audio.Procedural.Frequencies;

namespace Kingdom.Audio.Procedural
{
    public abstract class Instrument : MonoBehaviour
    {
        public event EventHandler InstrumentEnd;

        [SerializeField]
        protected List<Key> _keysPlayed;

        [SerializeField]
        protected ADSREnvelope ADSR;

        [SerializeField]
        protected float amplitude = 0.5f;

        [SerializeField]
        protected float _gain = 0.5f;

        [SerializeField]
        protected float _sampleRate;

        protected float _userVolume;

        public ReadOnlyCollection<Key> KeysPlayed
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
                _keysPlayed.Add(new Key { Name = keyName, TimePlayed = AudioSettings.dspTime });
        }

        public virtual void KeyUp(KeyName keyName)
        {
            var keyToUpdate = _keysPlayed.FirstOrDefault(k => k.Name == keyName);
            if (keyToUpdate != null)
                keyToUpdate.TimeReleased = AudioSettings.dspTime;
        }

        public virtual void QueueKey(Key key)
        {
            key.TimeReleased += AudioSettings.dspTime;
            key.TimePlayed += AudioSettings.dspTime;
            _keysPlayed.Add(key);
        }

        public virtual void QueueKey(IList<Key> keys)
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

        protected void OnInstrumentEnd(object sender, EventArgs e)
        {
            InstrumentEnd?.Invoke(this, e);
        }

        public abstract float WaveFunction(int dataIndex, double time, KeyName key);
    }

    [Serializable]
    public class Key : INotifyPropertyChanged
    {
        [SerializeField]
        private KeyStatus status;

        public KeyStatus Status
        {
            get => status;
            set
            {
                if (status == value)
                    return;
                status = value;
                OnPropertyChanged(new KeyStatusEventArgs(status));
            }
        }

        [field: SerializeField]
        public KeyName Name { get; set; }

        [field: SerializeField]
        public double TimePlayed { get; set; }

        [field: SerializeField]
        public double TimeReleased { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(PropertyChangedEventArgs args) =>
            PropertyChanged?.Invoke(this, args);

        public Key()
        {
            Status = KeyStatus.Waiting;
        }
    }

    public class KeyStatusEventArgs : PropertyChangedEventArgs
    {
        public KeyStatusEventArgs(KeyStatus keyStatusEventArgs)
            : base("Status")
        {
            KeyStatus = keyStatusEventArgs;
        }

        public KeyStatus KeyStatus { get; }
    }
}
