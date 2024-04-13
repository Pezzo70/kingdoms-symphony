using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MusicSheet : MonoBehaviour
{
    [SerializeField]Instrument instrument;
    [SerializeField]private Sprite[] clefSprites;
    [SerializeField]List<KeyPlayed> keysPlayed;

    [SerializeField]
    private Vector2 _offsetPosition;

    [SerializeField]
    private Sprite[] notationSprites;

    private GameObject notationSpriteObject;
    private int currentSpriteIndex = 0;

    [SerializeField]
    private bool isHover = false;

    // Start is called before the first frame update
    void Start()
    {
        ListCreated();
        notationSpriteObject = GameObject.FindGameObjectWithTag("NotationSprite");
    }

    void ListCreated()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        notationSpriteObject.SetActive(isHover);
         if(isHover) SpriteFollowMouse();
    }

    public void SetHover(bool val) => isHover = val;

    void SpriteFollowMouse()
    {
         Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         notationSpriteObject.transform.position = new Vector2(cursorPos.x + _offsetPosition.x, cursorPos.y + _offsetPosition.y);

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            currentSpriteIndex++;
            if (currentSpriteIndex >= notationSprites.Length)
            {
                currentSpriteIndex = 0;
            }

            UpdateSprite();
        }
        else if (scroll < 0f)
        {
            currentSpriteIndex--;
            if (currentSpriteIndex < 0)
            {
                currentSpriteIndex = notationSprites.Length - 1;
            }
            UpdateSprite();
        }
    }

     void UpdateSprite()
    {
        if (currentSpriteIndex >= 0 && currentSpriteIndex < notationSprites.Length)
        {
            notationSpriteObject.GetComponent<SpriteRenderer>().sprite = notationSprites[currentSpriteIndex];
        }
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
        scaleSprite.sprite = scaleSprite.sprite == clefSprites[0]? clefSprites[1]: clefSprites[0];
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
