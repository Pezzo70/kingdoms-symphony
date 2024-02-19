using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSheet : MonoBehaviour
{
    [SerializeField]Instrument instrument;
    [SerializeField]List<KeyPlayed> keysPlayed;
    // Start is called before the first frame update
    void Start()
    {
        keysPlayed = new List<KeyPlayed>();
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

    public void OnMouseDown(){
        Debug.Log(gameObject.name);
    }
}
