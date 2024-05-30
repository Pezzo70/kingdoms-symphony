using Kingdom.Audio;
using Kingdom.Audio.Procedural;
using UnityEngine;

public class IntrumentSettingsManager : MonoBehaviour 
{
    protected Instrument _currentInstrument;
    protected float[] _harmonics;
    protected ADSREnvelope _adsr;

    void OnEnable()
    {
        _currentInstrument = AudioSystem.Instance.instrument;
        _harmonics = _currentInstrument.GetHarmonics();
        _adsr = _currentInstrument.GetADSR();
    }

    void Commit()
    {
        
    }
}