using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
            notationSpriteObject.GetComponent<Image>().sprite = notationSprites[currentSpriteIndex];
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
        Image scaleSprite = GameObject.FindGameObjectWithTag("ChangeScale").GetComponent<Image>(); 
        scaleSprite.sprite = scaleSprite.sprite == clefSprites[0]? clefSprites[1]: clefSprites[0];
    }

    public void Play()
    {
        instrument.QueueKey(keysPlayed);
        ListCreated();
    }

   public void InsertSprite()
    {
        Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPos.z = 0;

        GameObject newNote = new GameObject();
        newNote.transform.SetParent(this.transform);
        newNote.transform.position = clickPos;

        Image renderer = newNote.AddComponent<Image>();
        renderer.raycastTarget = false;
        renderer.preserveAspect = true;
        renderer.sprite = notationSprites[currentSpriteIndex];
        newNote.GetComponent<RectTransform>().sizeDelta = new Vector2(1,1);
        newNote.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);

        notationSpriteObject.GetComponent<Image>().sprite = notationSprites[currentSpriteIndex];
    }
}
