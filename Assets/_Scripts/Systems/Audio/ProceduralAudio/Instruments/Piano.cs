using System;
using System.Collections.Generic;
using System.Linq;
using Kingdom.Enums.Audio.Procedural;
using UnityEngine;
using static Kingdom.Audio.Procedural.Frequencies;

namespace Kingdom.Audio.Procedural
{
    [System.Serializable]
    public class Piano : Instrument
    {
        [SerializeField, Min(0.0001f)]
        private float[] harmonics = new float[12];

        Queue<Action> events;

        public void Start()
        {
            this._sampleRate = AudioSettings.outputSampleRate;
            ADSR = new ADSREnvelope();
            _keysPlayed = new();
            events = new Queue<Action>();
        }

        public void Update() 
        { 
            while(events.Any())
              events.Dequeue()?.Invoke();
        }

        public void OnAudioFilterRead(float[] data, int channels)
        {
            for (int ikeys = 0; ikeys < _keysPlayed.Count; ikeys++)
            {
                var currentTime = AudioSettings.dspTime;
                var key = _keysPlayed[ikeys];
                if (key.TimePlayed > currentTime)
                    continue;

                var timeElapsed = (float)(currentTime - key.TimePlayed);

                float volumeModifier = ADSR.Sustain;
                var isReleased = key.TimeReleased != 0 && currentTime >= key.TimeReleased;
                if (timeElapsed <= ADSR.Attack) // It is in the Attack phase, the sound is still rising form 0 top to maximum (1)
                {
                        events.Enqueue(() => key.Status = KeyStatus.Attack);
                        volumeModifier = Mathf.InverseLerp(0.0f, ADSR.Attack, timeElapsed);
                }
                else if (timeElapsed < ADSR.Decay + ADSR.Attack) // The sound is in the decay phase meaning it is going from the maximum to the sustained level
                {
                    events.Enqueue(() =>key.Status = KeyStatus.Decay);
                    volumeModifier = Mathf.InverseLerp(
                        ADSR.Attack,
                        ADSR.Attack + ADSR.Decay,
                        timeElapsed
                    );
                    volumeModifier = Mathf.Lerp(1.0f, ADSR.Sustain, volumeModifier);
                }
                else if(!isReleased)
                    key.Status = KeyStatus.Sustain;

                if (isReleased) // The key is not being held any more, this is not a realistic piano as it can hold a note on sustain forever, it only goes to release when you release a key!
                {
                    events.Enqueue(() => key.Status = KeyStatus.Release);
                    timeElapsed = (float)(currentTime - key.TimeReleased);

                    if (timeElapsed > ADSR.Release)
                    {
                        this._keysPlayed.Remove(key);
                        if(_keysPlayed.Count == 0)
                           events.Enqueue(() => OnInstrumentEnd(this, new System.EventArgs()));
                        continue;
                    }

                    volumeModifier = Mathf.InverseLerp(0.0f, ADSR.Release, timeElapsed);
                    volumeModifier = Mathf.Lerp(ADSR.Sustain, 0.0f, volumeModifier);
                }

                for (int i = 0; i < data.Length; i += channels)
                {
                    float value =
                        WaveFunction(i / channels, key.TimePlayed, key.Name)
                        * (volumeModifier * _gain * _userVolume);
                    for (int channel = 0; channel < channels; channel++)
                        data[i + channel] += value;
                }
            }
        }

        public override float WaveFunction(int dataIndex, double time, KeyName key)
        {
            float superImposed = 0f;
            float frequency = key.GetFrequency();
            int count = harmonics.Length;
            double timeAux = AudioSettings.dspTime - time;
            for (int i = 1; i <= count; i++)
            {
                float harmonicFrequency = frequency * i;

                float timeAtTheBeginig = (float)(timeAux % (1.0 / (double)harmonicFrequency));

                float exactTime = timeAtTheBeginig + dataIndex / _sampleRate;

                superImposed +=
                    Mathf.Sin(exactTime * harmonicFrequency * 2f * Mathf.PI) * harmonics[i - 1];
            }

            return superImposed;
        }
    }
}
