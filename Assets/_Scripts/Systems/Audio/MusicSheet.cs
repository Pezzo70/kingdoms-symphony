using System.Collections.Generic;
using System.Linq;
using Kingdom.Audio.Procedural;
using Kingdom.Enums;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


namespace Kingdom.Audio
{
    public class MusicSheet : MonoBehaviour
    {
        [SerializeField]
        private ScriptableContainer notationContainer;
        [SerializeField]
        private Vector2 offsetPosition;
        [SerializeField]
        private bool isHover = false;
        [SerializeField]
        private ScriptableContainer clefContainer;
        [SerializeField]
        private Object pagePrefab;

        private GameObject notationSpriteObject;
        private Stack<Note> actionStack;
        private int currentSpriteIndex = 0;

        void Start()
        {
            notationSpriteObject = GameObject.FindGameObjectWithTag("NotationSprite");
            actionStack = new Stack<Note>();

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
                currentSpriteIndex = (currentSpriteIndex + 1) % notationContainer.Count();
                UpdateSprite();
            }
            else if (scroll < 0f)
            {
                currentSpriteIndex = (currentSpriteIndex - 1 + notationContainer.Count()) % notationContainer.Count();
                UpdateSprite();
            }
        }

        void UpdateSprite()
        {
            if (currentSpriteIndex >= 0 && currentSpriteIndex < notationContainer.Count())
                notationSpriteObject.GetComponent<Image>().sprite = notationContainer.GetByType<NotationScriptable>().ToArray()[currentSpriteIndex].Sprite;
        }

        public void Clear()
        {
            while (actionStack.Count != 0)
                Destroy(actionStack.Pop().gameObject);
        }

        public void Undo()
        {
            if (actionStack.Count > 0)
                Destroy(actionStack.Pop().gameObject);
        }

        public void ChangeScale()
        {
            var clefSprites = clefContainer.GetByType<ClefScriptable>().ToArray();
            var scaleObject = GameObject.FindGameObjectWithTag("ChangeScale");
            var scaleSprite = scaleObject.GetComponent<Image>();
            scaleSprite.sprite = scaleSprite.sprite == clefSprites[0].Sprite ? clefSprites[1].Sprite : clefSprites[0].Sprite;

            foreach (Transform noteT in scaleObject.transform.parent)
            {
                Note note = null;
                noteT.TryGetComponent<Note>(out note);
                if(note != null)
                  note.clef = clefContainer.GetFirstByType<ClefScriptable>(a => a.Sprite == scaleSprite.sprite);
            }
        }

        public void Play()
        {
            AudioSystem.Instance.PlayMusicSheet(this.actionStack.AsReadOnlyList());
        }

        public void InsertSprite()
        {
            Image scaleSprite = GameObject.FindGameObjectWithTag("ChangeScale").GetComponent<Image>();
            NotationScriptable notationSprite = notationContainer.GetByType<NotationScriptable>().ToArray()[currentSpriteIndex];
            var line = GetClosestLine();
            var activePageIndex = GetActivePageIndex();

            if(actionStack.Where(a => a.line == line.index && a.page == activePageIndex).Sum(a => a.note.Tempo.ToFloat()) >= 1)
                return;

            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
            clickPos.z = 0;


            GameObject newNote = new GameObject();
            newNote.transform.SetParent(GetActivePage().transform);
            newNote.transform.position = clickPos;

            Image renderer = newNote.AddComponent<Image>();
            renderer.raycastTarget = false;
            renderer.preserveAspect = true;
            renderer.sprite = notationSprite.Sprite;
            newNote.GetComponent<RectTransform>().sizeDelta = notationSpriteObject.GetComponent<RectTransform>().sizeDelta;
            newNote.GetComponent<RectTransform>().localScale = Vector3.one;

            notationSpriteObject.GetComponent<Image>().sprite = notationSprite.Sprite;
            newNote.GetComponent<RectTransform>().pivot = GetSpriteRelativePivot(notationSpriteObject.GetComponent<Image>());
            newNote.transform.position = new Vector3(newNote.transform.position.x, line.yPos, newNote.transform.position.z);


            Note note = newNote.AddComponent<Note>();
            note.clef = clefContainer.GetFirstByType<ClefScriptable>(a => a.Sprite == scaleSprite.sprite);
            note.line = line.index;
            note.xPos = clickPos.x;
            note.page = activePageIndex;
            note.note = notationSprite;

            newNote.name = $"Tempo: {notationSprite.Tempo} - Line: {note.line} - Page: {note.page} - Clef: {note.clef}";

            actionStack.Push(note);
            //this.Play();
        }

        public (float yPos, int index) GetClosestLine()
        {
            GameObject sheetLine = GameObject.Find("SheetLine");

            float mouseY = notationSpriteObject.transform.position.y;
            int childCount = sheetLine.transform.childCount;
            float yAux = 0;
            int closestIndex = -1;

            for (int i = 0; i < childCount - 1; i++)
            {
                GameObject line = sheetLine.transform.GetChild(i).gameObject;
                float lineY = line.GetComponent<RectTransform>().position.y;
                if (i == 0)
                {
                    yAux = lineY;
                    closestIndex = i;
                }
                else if (Mathf.Abs(lineY - mouseY) < Mathf.Abs(yAux - mouseY))
                {
                    yAux = lineY;
                    closestIndex = i;
                }
            }

            return (yAux, closestIndex);
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
            var page = Instantiate(pagePrefab, pageParent.transform);


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

        private int GetActivePageIndex()
        {
            var parentGo = GameObject.FindWithTag("Page");
            for (int i = 0; i < parentGo.transform.childCount; i++)
            {
                if (parentGo.transform.GetChild(i).gameObject.activeSelf)
                {
                    return i;
                }
            }
            return -1; // Retorna -1 se nenhum filho estiver ativo
        }
        private int GetMaxPage() => Player.PlayerContainer.Instance.playerData.GetSheetPages(Kingdom.Enums.Player.CharacterID.Roddie);
        #endregion
    }
}