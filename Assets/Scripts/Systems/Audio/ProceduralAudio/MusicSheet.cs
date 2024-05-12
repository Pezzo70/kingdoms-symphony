    using System.Collections.Generic;
    using System.Linq;
using Unity.VisualScripting;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.UI;

    public class MusicSheet : MonoBehaviour
    {
        [SerializeField] 
        Instrument instrument;
        [SerializeField] 
        private Sprite[] clefSprites;
        [SerializeField] 
        private Sprite[] notationSprites;
        [SerializeField] 
        private Vector2 offsetPosition;
        [SerializeField] 
        private bool isHover = false;
        [SerializeField] 
        public int pageNumber;
        private List<KeyPlayed> keysPlayed = new List<KeyPlayed>();
        private GameObject notationSpriteObject;
        private Stack<GameObject> actionStack;
        private int currentSpriteIndex = 0;

        void Start()
        {
            notationSpriteObject = GameObject.FindGameObjectWithTag("NotationSprite");
            actionStack = new Stack<GameObject>();
        }
        void Update()
        {
            notationSpriteObject.SetActive(isHover); 
            if (isHover) 
                SpriteFollowMouse();        

            SetActivePage(pageNumber);
        }

        public void SetHover(bool val) => isHover = val;

    public static Vector2 GetSpriteRelativePivot(Image img)
    {

        Bounds bounds = img.sprite.bounds;
		var pivotX = - bounds.center.x / bounds.extents.x / 2 + 0.5f;
		var pivotY = - bounds.center.y / bounds.extents.y / 2 + 0.5f;
        Vector2 pivotPixel = new Vector2(pivotX, pivotY);

 
        return pivotPixel;
    }

        void SpriteFollowMouse()
        {
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            notationSpriteObject.GetComponent<RectTransform>().pivot = GetSpriteRelativePivot(notationSpriteObject.GetComponent<Image>());
            notationSpriteObject.transform.position = new Vector2(cursorPos.x + offsetPosition.x, cursorPos.y + offsetPosition.y);

            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll > 0f)
            {
                currentSpriteIndex = (currentSpriteIndex + 1) % notationSprites.Length;
                UpdateSprite();
            }
            else if (scroll < 0f)
            {
                currentSpriteIndex = (currentSpriteIndex - 1 + notationSprites.Length) % notationSprites.Length;
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
            while(actionStack.Count != 0)
                Destroy(actionStack.Pop());
        }

        public void Undo()
        {
            if (actionStack.Count > 0)
                Destroy(actionStack.Pop());
        }

        public void ChangeScale()
        {
            Image scaleSprite = GameObject.FindGameObjectWithTag("ChangeScale").GetComponent<Image>();
            scaleSprite.sprite = scaleSprite.sprite == clefSprites[0] ? clefSprites[1] : clefSprites[0];
        }

        public void Play()
        {
            var key = new KeyPlayed(){Name=Frequencies.KeyName.A3, TimePlayed = 0, TimeReleased = 1};
            keysPlayed.Add(key);
            instrument.QueueKey(keysPlayed);
        }

        public void InsertSprite()
        {
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPos.z = 0;


            GameObject newNote = new GameObject();
            newNote.transform.SetParent(SetActivePage(pageNumber).transform);
            newNote.transform.position = clickPos;

            Image renderer = newNote.AddComponent<Image>();
            renderer.raycastTarget = false;
            renderer.preserveAspect = true;
            renderer.sprite = notationSprites[currentSpriteIndex];
            newNote.GetComponent<RectTransform>().sizeDelta = notationSpriteObject.GetComponent<RectTransform>().sizeDelta;
            newNote.GetComponent<RectTransform>().localScale = Vector3.one;
            newNote.GetComponent<RectTransform>().localPosition = new Vector3(newNote.transform.position.x, GetClosestLine(), newNote.transform.position.z);

            notationSpriteObject.GetComponent<Image>().sprite = notationSprites[currentSpriteIndex];

            actionStack.Push(newNote);
        }

        public float GetClosestLine()
        {
            GameObject sheetLine = GameObject.Find("SheetLine");

            float mouseY =  GetMousePosition().y;
            int childCount = sheetLine.transform.childCount;
            float yAux = 0;

            for(int i = 0; i < childCount - 1; i++){
                GameObject line = sheetLine.transform.GetChild(i).gameObject;
                float lineY = line.GetComponent<RectTransform>().anchoredPosition.y;
                Debug.Log(line.GetComponent<RectTransform>().gameObject);
                Debug.Log(line);
                Debug.Log(mouseY + "-" + lineY);
                Debug.Log(lineY - mouseY);
                if(i == 0) 
                    yAux = lineY;
                else if((lineY - mouseY) < (yAux - mouseY)) 
                    yAux = lineY;
            }

            Debug.Log(yAux);
            return yAux;
        }

        private Vector2 GetMousePosition()
        {
            var mPos = Input.mousePosition;
            var rect = (transform as RectTransform).rect;
            var relX = mPos.x - rect.x;
            var relY = mPos.y - rect.y;

            return new Vector2(relX, relY);
        }
        
        private GameObject SetActivePage(int index)
        {
            var parentGo = GameObject.FindWithTag("Page");
            GameObject activeGo = null;
            for(int i = 0; i < parentGo.transform.childCount; i++) 
            {
                var auxGo = parentGo.transform.GetChild(i);

                if(i == index) activeGo = auxGo.gameObject;
                else auxGo.transform.gameObject.SetActive(false);
            } 

            activeGo.gameObject.SetActive(true);
            return activeGo;
        }

        public void CreatePage()
        {
            var pageParent = GameObject.FindWithTag("Page");
            var childCount = pageParent.transform.childCount + 1;

            if(childCount == GetMaxPage()) return;
            var page = new GameObject("Page" + childCount);
            page.transform.SetParent(pageParent.transform);

        }

        private int GetMaxPage() => Player.PlayerContainer.Instance.playerData.GetSheetPages(Player.CharacterID.Roddie);
    }
