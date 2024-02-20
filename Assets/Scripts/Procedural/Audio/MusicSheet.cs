using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MusicSheet : MonoBehaviour
{
    [SerializeField]Instrument instrument;
    [SerializeField]List<KeyPlayed> keysPlayed;
    // Start is called before the first frame update
    void Start()
    {
        ListCreated();
    }

    void ListCreated()
    {
        keysPlayed = new List<KeyPlayed>()
        {
            new KeyPlayed {Name = Frequencies.RandomKey(), TimePlayed = 2, TimeReleased = 2.4},
            new KeyPlayed {Name = Frequencies.KeyName.A2, TimePlayed = 2, TimeReleased = 2.4},
            new KeyPlayed {Name = Frequencies.KeyName.D2, TimePlayed = 2, TimeReleased = 2.4},
            new KeyPlayed {Name = Frequencies.RandomKey(), TimePlayed = 2, TimeReleased = 2.4},
            new KeyPlayed {Name = Frequencies.KeyName.B0, TimePlayed = 2, TimeReleased = 2.4},
            new KeyPlayed {Name = Frequencies.RandomKey(), TimePlayed = 2, TimeReleased = 2.4}
        };
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void Clear()
    {
        keysPlayed.Clear();
    }

    public void Undo()
    {
        if(keysPlayed.Count > 0) keysPlayed.RemoveAt(keysPlayed.Count - 1);
    }

    public void ChangeScale()
    {
        //Wait Pezzo create a new method with scales organized by index
        Debug.Log("Changed!");
    }

    public void Play()
    {
        instrument.QueueKey(keysPlayed);
        ListCreated();
    }

    public void OnMouseDown()
    {
        
    }
}
