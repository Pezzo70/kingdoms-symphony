using System.Collections.Generic;
using System.Linq;
using Assets.SimpleLocalization.Scripts;
using Kingdom.Audio.Procedural;
using Kingdom.Effects;
using Kingdom.Enums;
using Kingdom.Enums.MusicTheory;
using Kingdom.Extensions;
using Kingdom.Level;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Kingdom.Audio
{
    public class MusicSheet : MonoBehaviour
    {
        private const float DEFAULT_SPRITE_SIZE = 70f;

        [SerializeField]
        private Object pagePrefab;
        private GameObject notationSpriteObject;
        private Stack<MonoBehaviour> actionStack;
        public GameObject musicSheetCanvas;
        public GameObject playerOptions;
        public bool wasOpen;
        private Queue<System.Action> mainThreadActions;

        /********************Notation Arrays***************************/
        private ISprite[] currentSpriteArray;
        private int currentIndex;

        [SerializeField]
        private ScriptableContainer notationContainer;

        [SerializeField]
        private ScriptableContainer clefContainer;
        private NotationScriptable[] upNotations;
        private NotationScriptable[] downNotations;
        private KeySignatureScriptable[] keySignatures;

        /*************************************************************/

        [SerializeField]
        private Vector2 offsetPosition;
        private bool isHover = false;
        private string currentHoverTag;
        public TextMeshProUGUI pageCounterText;
        public TMP_InputField pageLabelInput;
        public LocalizedTextMeshProUGUI currentNoteText;

        void Awake()
        {
            EventManager.PauseGame += HandlePause;
            EventManager.UnpauseGame += HandleUnpause;
            EventManager.NoteCurrentlyPlaying += HandleCurrentNotePlaying;
        }

        private void HandleCurrentNotePlaying(Note note)
        {
            if (note == null)
            {
                currentNoteText.Clear();
                return;
            }

            if (note.GetChord(EffectsAndScrollsManager.Instance.playedNotes).Count() > 1)
            {
                currentNoteText.LocalizationKey = "Theory.Chord";
                return;
            }

            currentNoteText.LocalizationKey = "Theory.Notes." + note.ToKeyText();
        }

        void Start()
        {
            Canvas parentCanvas = transform.parent.gameObject.GetComponent<Canvas>();
            parentCanvas.worldCamera = Camera.main;
            parentCanvas.planeDistance = 15;
            parentCanvas.sortingLayerName = "UI";
            parentCanvas.sortingOrder = 100;

            actionStack = new Stack<MonoBehaviour>();

            notationSpriteObject = GameObject.FindGameObjectWithTag("NotationSprite");
            upNotations = notationContainer
                .GetByType<NotationScriptable>()
                .Where(
                    nc =>
                        nc.NoteOrientation is NotationOrientation.Up
                        || nc.NoteOrientation is NotationOrientation.Center
                )
                .ToArray();
            downNotations = notationContainer
                .GetByType<NotationScriptable>()
                .Where(
                    nc =>
                        nc.NoteOrientation is NotationOrientation.Down
                        || nc.NoteOrientation is NotationOrientation.Center
                )
                .ToArray();
            keySignatures = notationContainer.GetByType<KeySignatureScriptable>().ToArray();

            currentSpriteArray = upNotations;
            UpdatePageCounter();
            mainThreadActions = new Queue<System.Action>();
        }

        void OnDestroy()
        {
            EventManager.PauseGame -= HandlePause;
            EventManager.UnpauseGame -= HandleUnpause;
            EventManager.NoteCurrentlyPlaying -= HandleCurrentNotePlaying;
        }

        private void HandlePause()
        {
            if (wasOpen)
            {
                musicSheetCanvas.SetActive(false);
            }
        }

        private void HandleUnpause()
        {
            if (wasOpen)
            {
                musicSheetCanvas.SetActive(true);
            }
        }

        void Update()
        {
            notationSpriteObject.SetActive(isHover);
            if (isHover)
                SpriteFollowMouse();
            UpdatePageCounter();

            while (mainThreadActions.Count > 0)
            {
                mainThreadActions.Dequeue().Invoke();
            }
        }

        public void SetHover(bool val, string tag = "MusicSheet")
        {
            isHover = val;
            currentHoverTag = tag;

            switch (currentHoverTag)
            {
                case "MusicSheet":
                    currentSpriteArray = upNotations;
                    break;
                case "KeySignatureArea":
                    currentSpriteArray = keySignatures;
                    break;
            }
            if (currentIndex >= currentSpriteArray.Length)
                currentIndex = 0;

            notationSpriteObject.GetComponent<Image>().sprite = currentSpriteArray[
                currentIndex
            ].Sprite;
        }

        public void Exit()
        {
            this.playerOptions.SetActive(true);
            this.musicSheetCanvas.SetActive(false);
            this.wasOpen = false;
            EventManager.MusicSheetOpen?.Invoke(false);
        }

        void SpriteFollowMouse()
        {
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            notationSpriteObject.GetComponent<RectTransform>().pivot = notationSpriteObject
                .GetComponent<Image>()
                .GetSpriteRelativePivot();
            notationSpriteObject.transform.position = new Vector2(
                cursorPos.x + offsetPosition.x,
                cursorPos.y + offsetPosition.y
            );

            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll > 0f)
            {
                currentIndex = (currentIndex + 1) % currentSpriteArray.Length;
                UpdateSprite();
            }
            else if (scroll < 0f)
            {
                currentIndex =
                    (currentIndex - 1 + currentSpriteArray.Length) % currentSpriteArray.Length;
                UpdateSprite();
            }
        }

        void UpdateSprite()
        {
            if (currentIndex >= 0 && currentIndex < currentSpriteArray.Length)
            {
                notationSpriteObject.GetComponent<Image>().sprite = currentSpriteArray[
                    currentIndex
                ].Sprite;
            }
        }

        public void Clear()
        {
            while (actionStack.Count != 0)
            {
                if (actionStack.Peek().IsDestroyed())
                    actionStack.Pop();
                else
                    Destroy(actionStack.Pop().gameObject);
            }
            SetActivePage(0);
        }

        public void Undo()
        {
            if (actionStack.Count > 0)
            {
                MonoBehaviour lastAction = null;

                while (actionStack.Peek().IsDestroyed() && actionStack.Count > 0)
                {
                    lastAction = actionStack.Pop();
                }

                if (actionStack.Count > 0)
                    lastAction = actionStack.Pop();

                if (!lastAction?.IsDestroyed() ?? false)
                {
                    var lastActionPage = lastAction.transform.parent;
                    var lastActionPageIndex = lastActionPage.GetSiblingIndex();
                    Destroy(lastAction.gameObject);

                    UpdateActivePageAfterUndo(lastActionPageIndex);
                }
            }
        }

        private void UpdateActivePageAfterUndo(int previousPageIndex)
        {
            var pageParent = GameObject.FindWithTag("Page");
            int childCount = pageParent.transform.childCount;

            if (
                previousPageIndex >= 0
                && previousPageIndex < childCount
                && pageParent.transform.GetChild(previousPageIndex).childCount > 0
            )
            {
                SetActivePage(previousPageIndex);
                return;
            }

            for (int i = childCount - 1; i >= 0; i--)
            {
                if (pageParent.transform.GetChild(i).childCount > 0)
                {
                    SetActivePage(i);
                    return;
                }
            }

            if (childCount > 0)
            {
                SetActivePage(0);
            }
        }

        public void ChangeScale()
        {
            var clefSprites = clefContainer.GetByType<ClefScriptable>().ToArray();
            var scaleObject = GameObject.FindGameObjectWithTag("ChangeScale");
            var scaleSprite = scaleObject.GetComponent<Image>();
            scaleSprite.sprite =
                scaleSprite.sprite == clefSprites[0].Sprite
                    ? clefSprites[1].Sprite
                    : clefSprites[0].Sprite;

            foreach (Transform noteT in scaleObject.transform.parent)
            {
                Note note = null;
                noteT.TryGetComponent<Note>(out note);
                if (note != null)
                    note.clef = clefContainer.GetFirstByType<ClefScriptable>(
                        a => a.Sprite == scaleSprite.sprite
                    );
            }
        }

        //Also using this method on PlayerTurnManager
        public void Play()
        {
            var pageParent = GameObject.FindWithTag("Page");
            int pagesCount = pageParent.transform.childCount;

            var notesToPlay = new List<Note>();

            EffectsAndScrollsManager.Instance.playedNotes.Clear();

            for (int pageIndex = 0; pageIndex < pagesCount; pageIndex++)
            {
                var page = pageParent.transform.GetChild(pageIndex);
                foreach (Transform noteTransform in page)
                {
                    var noteComponent = noteTransform.GetComponent<Note>();
                    if (noteComponent != null)
                    {
                        notesToPlay.Add(noteComponent);
                        EffectsAndScrollsManager.Instance.playedNotes.Add(noteComponent);
                    }
                }
            }
            AudioSystem.Instance.Play(notesToPlay.AsReadOnlyList());
            GameObject.FindWithTag("Play").GetComponent<Selectable>().interactable = false;
            AudioSystem.Instance.instrument.InstrumentEnd += delegate
            {
                mainThreadActions.Enqueue(() =>
                {
                    foreach (Transform page in pageParent.transform)
                    {
                        foreach (Transform noteT in page)
                        {
                            noteT.TryGetComponent<Note>(out Note note);
                            if (note != null)
                                note.setColor(new Color32(255, 255, 255, 255));
                        }
                    }
                    GameObject.FindWithTag("Play").GetComponent<Selectable>().interactable = true;
                    EventManager.NoteCurrentlyPlaying?.Invoke(null);
                });
            };
        }

        public void InsertNote()
        {
            var cLine = GetClosestLine();
            var aPage = GetActivePageIndex();
            var spriteArray = cLine.index > 6 ? upNotations : downNotations;

            float tempo = spriteArray[currentIndex].Tempo.ToFloat();
            if (
                actionStack
                    .OfType<Note>()
                    .Where(a => a.line == cLine.index && a.page == aPage)
                    .Sum(a => a.note.Tempo.ToFloat()) > (1f - tempo)
            )
                return;

            Sprite sprite = spriteArray[currentIndex].Sprite;
            Image scaleSprite = GameObject
                .FindGameObjectWithTag("ChangeScale")
                .GetComponent<Image>();
            var newNote = CreateObjectInLine(cLine.yPos, sprite);

            Note noteOnNearX = actionStack
                .OfType<Note>()
                .FirstOrDefault(
                    n => n.page == aPage && Mathf.Abs(n.xPos - newNote.transform.position.x) <= 1f
                );
            if (noteOnNearX != null)
            {
                newNote.transform.position = new Vector3(
                    noteOnNearX.xPos,
                    newNote.transform.position.y,
                    newNote.transform.position.z
                );
                newNote.GetComponent<Image>().sprite = noteOnNearX.note.Sprite;
                newNote.GetComponent<RectTransform>().pivot = noteOnNearX
                    .GetComponent<RectTransform>()
                    .pivot;
            }

            Note note = newNote.AddComponent<Note>();
            note.clef = clefContainer.GetFirstByType<ClefScriptable>(
                a => a.Sprite == scaleSprite.sprite
            );
            note.line = cLine.index;
            note.xPos = newNote.transform.position.x;
            note.page = aPage;
            note.note = notationContainer.GetFirstByType<NotationScriptable>(
                n => n.Sprite == newNote.GetComponent<Image>().sprite
            );
            newNote.name = note.ToString();
            newNote.tag = "Note";
            note.ApplyInLine();

            //@TODO: Setar size e raycast padding baseado no tamanho do sprite(de forma dinamica).
            switch (note.note.NoteOrientation)
            {
                case NotationOrientation.Center:
                    if (
                        note.note.NoteBehaviour == NotationBehaviour.Pause
                        && (
                            note.note.Tempo == Tempo.Quarter
                            || note.note.Tempo == Tempo.Sixteenth
                            || note.note.Tempo == Tempo.Eighth
                        )
                    )
                        break;
                    newNote.GetComponent<RectTransform>().sizeDelta = new Vector2(
                        newNote.GetComponent<RectTransform>().sizeDelta.x,
                        50
                    );
                    break;
                case NotationOrientation.Up:
                    newNote.GetComponent<Image>().raycastPadding = new Vector4(0, 3, 0, 68);
                    break;
                case NotationOrientation.Down:
                    newNote.GetComponent<Image>().raycastPadding = new Vector4(0, 68, 0, 3);
                    break;
            }

            actionStack.Push(note);
        }

        public void InsertKeySignature()
        {
            var line = GetClosestLine();
            var aPage = GetActivePageIndex();

            if (
                actionStack
                    .OfType<MonoKeySignature>()
                    .Any(a => a.line == line.index && a.page == aPage)
            )
                return;

            Sprite sprite = currentSpriteArray[currentIndex].Sprite;
            var newNote = CreateObjectInLine(line.yPos, sprite);
            Image scaleSprite = GameObject
                .FindGameObjectWithTag("ChangeScale")
                .GetComponent<Image>();

            MonoKeySignature key = newNote.AddComponent<MonoKeySignature>();
            key.line = line.index;
            key.page = aPage;
            key.keySignature = notationContainer.GetFirstByType<KeySignatureScriptable>(
                n => n.Sprite == newNote.GetComponent<Image>().sprite
            );
            newNote.name = key.ToString();
            newNote.tag = "KeySignature";

            actionStack.Push(key);
        }

        public GameObject CreateObjectInLine(float lineY, Sprite sprite)
        {
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPos.z = 0;

            GameObject newNote = new GameObject();
            newNote.transform.SetParent(GetActivePage().transform);
            newNote.transform.position = clickPos;

            Image renderer = newNote.AddComponent<Image>();
            renderer.raycastTarget = true;
            renderer.preserveAspect = true;
            renderer.sprite = sprite;
            newNote.GetComponent<RectTransform>().sizeDelta = notationSpriteObject
                .GetComponent<RectTransform>()
                .sizeDelta;
            newNote.GetComponent<RectTransform>().localScale = Vector3.one;

            notationSpriteObject.GetComponent<Image>().sprite = sprite;
            newNote.GetComponent<RectTransform>().pivot = notationSpriteObject
                .GetComponent<Image>()
                .GetSpriteRelativePivot();
            newNote.transform.position = new Vector3(
                newNote.transform.position.x,
                lineY,
                newNote.transform.position.z
            );

            return newNote;
        }

        public (float yPos, int index) GetClosestLine()
        {
            GameObject sheetLine = GameObject.Find("SheetLines");

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
            UpdatePageCounter();
            return activeGo;
        }

        public void CreatePage()
        {
            var pageParent = GameObject.FindGameObjectWithTag("Page");
            var childCount = pageParent.transform.childCount + 1;

            if (childCount == GetMaxPage())
                return;

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

            SetActivePage(childCount - 2);
            UpdatePageCounter();
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
                if (parentGo.transform.GetChild(i).gameObject.activeSelf)
                    return i;

            return -1;
        }

        private int GetMaxPage() =>
            PlaythroughContainer.Instance.PlayerStats.AvailableSheetBars + 1;

        private void UpdatePageCounter()
        {
            int activePageIndex = GetActivePageIndex() + 1;
            int totalPageCount = GameObject.FindWithTag("Page").transform.childCount;

            GameObject pageCounterObject = GameObject.FindWithTag("PageCounter");

            if (pageCounterObject != null)
            {
                TextMeshProUGUI textAux = pageCounterObject.GetComponent<TextMeshProUGUI>();

                if (textAux != null)
                    textAux.text = $"{activePageIndex}-{totalPageCount}";
            }
        }

        #endregion

        public void ChangeNote(Note note)
        {
            var line = GetClosestLine();
            var spriteArray = line.index > 6 ? upNotations : downNotations;
            NotationScriptable currentNote = spriteArray[currentIndex];

            if (note.note.Tempo > currentNote.Tempo)
            {
                return;
            }

            if (
                note.note.Tempo != currentNote.Tempo
                || note.note.NoteBehaviour != currentNote.NoteBehaviour
            )
            {
                note.note = currentNote;
                note.GetComponent<Image>().sprite = currentNote.Sprite;
                note.GetComponent<RectTransform>().pivot = note.GetComponent<Image>()
                    .GetSpriteRelativePivot();
            }
        }

        public void RemoveNote(Note note)
        {
            var newActionStack = new Stack<MonoBehaviour>();
            foreach (MonoBehaviour action in actionStack.Reverse())
            {
                if (action != note)
                    newActionStack.Push(action);
            }
            actionStack = newActionStack;
            Destroy(note.gameObject);
        }
    }
}
