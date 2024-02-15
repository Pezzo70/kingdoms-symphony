using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSheet : MonoBehaviour
{
    [SerializeField]Instrument instrument;
    float time = 4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time < 6) 
        {    return;}
        else
         {   time = 0;}

        List<KeyPlayed> sheet = new List<KeyPlayed>();

        for(int i = 0; i < 4; i++)
        {
            var key = new KeyPlayed();
            key.Name = Frequencies.RandomKey();
            key.TimePlayed = Random.value + (i * 2);
            key.TimeReleased = Random.value + key.TimePlayed;
            sheet.Add(key);
        }

        instrument.QueueKey(sheet);
    }
}
