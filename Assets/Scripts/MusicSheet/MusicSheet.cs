using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MusicSheet : MonoBehaviour
{
    [SerializeField]Instrument instrument;
    [SerializeField]private Sprite[] notationSprites;
    [SerializeField]List<KeyPlayed> keysPlayed;
    // Start is called before the first frame update
    void Start()
    {
        ListCreated();
    }

    void ListCreated()
    {
        
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
        SpriteRenderer scaleSprite = GameObject.FindGameObjectWithTag("ChangeScale").GetComponent<SpriteRenderer>(); 
        scaleSprite.sprite = scaleSprite.sprite == notationSprites[0]? notationSprites[1]: notationSprites[0];
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
