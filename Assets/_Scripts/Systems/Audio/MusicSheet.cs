using System.Collections.Generic;
using Kingdom.Audio.Procedural;
using UnityEngine;
using UnityEngine.UI;


namespace Kingdom.Audio
{
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


        private List<KeyPlayed> keysPlayed = new List<KeyPlayed>();
        private GameObject notationSpriteObject;
        private Stack<GameObject> actionStack;
        private int currentSpriteIndex = 0;

        void Start()
        {
            notationSpriteObject = GameObject.FindGameObjectWithTag("NotationSprite");
            actionStack = new Stack<GameObject>();

            GetActivePage();
        }

        void Update()
        {
            notationSpriteObject.SetActive(isHover);
            if (isHover)
                SpriteFollowMouse();
        }

        public void SetHover(bool val) => isHover = val;

        public static Vector2 GetSpriteRelativePivot(Image img)
        {

            Bounds bounds = img.sprite.bounds;
            var pivotX = -bounds.center.x / bounds.extents.x / 2 + 0.5f;
            var pivotY = -bounds.center.y / bounds.extents.y / 2 + 0.5f;
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
            while (actionStack.Count != 0)
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
            var key = new KeyPlayed() { Name = Frequencies.KeyName.A3, TimePlayed = 0, TimeReleased = 1 };
            keysPlayed.Add(key);
            instrument.QueueKey(keysPlayed);
        }

        public void InsertSprite()
        {
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPos.z = 0;


            GameObject newNote = new GameObject();
            newNote.transform.SetParent(GetActivePage().transform);
            newNote.transform.position = clickPos;

            Image renderer = newNote.AddComponent<Image>();
            renderer.raycastTarget = false;
            renderer.preserveAspect = true;
            renderer.sprite = notationSprites[currentSpriteIndex];
            newNote.GetComponent<RectTransform>().sizeDelta = notationSpriteObject.GetComponent<RectTransform>().sizeDelta;
            newNote.GetComponent<RectTransform>().localScale = Vector3.one;

            notationSpriteObject.GetComponent<Image>().sprite = notationSprites[currentSpriteIndex];
            newNote.GetComponent<RectTransform>().pivot = GetSpriteRelativePivot(notationSpriteObject.GetComponent<Image>());
            newNote.transform.position = new Vector3(newNote.transform.position.x, GetClosestLine(), newNote.transform.position.z);

            actionStack.Push(newNote);
        }

        public float GetClosestLine()
        {
            GameObject sheetLine = GameObject.Find("SheetLine");

            float mouseY = notationSpriteObject.transform.position.y;
            int childCount = sheetLine.transform.childCount;
            float yAux = 0;

            for (int i = 0; i < childCount - 1; i++)
            {
                GameObject line = sheetLine.transform.GetChild(i).gameObject;
                float lineY = line.GetComponent<RectTransform>().position.y;
                if (i == 0)
                    yAux = lineY;
                else if (Mathf.Abs(lineY - mouseY) < Mathf.Abs(yAux - mouseY))
                    yAux = lineY;
            }


            return yAux;
        }


        #region Pages
        private GameObject SetActivePage(int index)
        {
            var parentGo = GameObject.FindWithTag("Page");
            GameObject activeGo = parentGo.transform.GetChild(index).gameObject;

            foreach (Transform childTransform in parentGo.transform)
                childTransform.gameObject.SetActive(false);


            activeGo.SetActive(true);
            return activeGo;
        }

        public void CreatePage()
        {
            var pageParent = GameObject.FindWithTag("Page");
            var childCount = pageParent.transform.childCount + 1;

            if (childCount == GetMaxPage()) return;
            var page = new GameObject("Page" + childCount);
            page.transform.SetParent(pageParent.transform);
            page.AddComponent<RectTransform>();

            SetActivePage(pageParent.transform.childCount - 1);
        }

        public void RemovePage()
        {
            var page = GameObject.FindWithTag("Page");
            int childCount = page.transform.childCount;

            if (childCount <= 1)
                return;
            else
                Destroy(page.transform.GetChild(childCount - 1).gameObject);
        }

        public void NavigatePage(bool next)
        {
            var page = GameObject.FindWithTag("Page");
            int childCount = page.transform.childCount;
            int activeIndex = -1;

            for (int i = 0; i < childCount; i++)
            {
                var child = page.transform.GetChild(i);
                if (child.gameObject.activeSelf)
                {
                    activeIndex = i;
                    break;
                }
            }

            if (next)
            {
                activeIndex = (childCount - 1) == activeIndex ? 0 : activeIndex + 1;
                SetActivePage(activeIndex);
            }
            else
            {
                activeIndex = activeIndex <= 0 ? (childCount - 1) : activeIndex - 1;
                SetActivePage(activeIndex);
            }

        }

        private GameObject GetActivePage()
        {
            var parentGo = GameObject.FindWithTag("Page");
            foreach (Transform go in parentGo.transform)
                if (go.gameObject.activeSelf)
                    return go.gameObject;
            return null;
        }
        private int GetMaxPage() => Player.PlayerContainer.Instance.playerData.GetSheetPages(Kingdom.Enums.Player.CharacterID.Roddie);
        #endregion
    }
}