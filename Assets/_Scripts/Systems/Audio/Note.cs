using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Kingdom.Audio.Procedural;
using Kingdom.Enums;
using Kingdom.Enums.Audio.Procedural;
using Kingdom.Enums.MusicTheory;
using Kingdom.Extensions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kingdom.Audio
{
    public class Note : UIEventTrigger
    {
        public MusicSheet musicSheet;
        public NotationScriptable note;
        public ClefScriptable clef;
        public KeySignatureScriptable signature;
        public float xPos;
        public int line;
        public int page;
        private Image imageComponent;

        private Queue<Action> mainThreadActions;

        public void Start()
        {
            imageComponent = GetComponent<Image>();
            mainThreadActions = new Queue<Action>();
            musicSheet = GameObject.FindAnyObjectByType<MusicSheet>();
            this.AddComponent<Selectable>();
            supportedActionsAudio = new UIAction[] { UIAction.Submit};
        }

        public void Update()
        {
            while (mainThreadActions.Count > 0)
            {
                mainThreadActions.Dequeue().Invoke();
            }
        }

        public void OnKeyStatusChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e is KeyStatusEventArgs statusArg)
            {
                switch (statusArg.KeyStatus)
                {
                    case KeyStatus.Waiting:
                        break;
                    case KeyStatus.Attack:
                        mainThreadActions.Enqueue(() =>
                        {
                            EventManager.NoteCurrentlyPlaying?.Invoke(this);
                            SetColor(new Color32(0, 0, 0, 100));
                        });
                        break;
                    case KeyStatus.Decay:
                        mainThreadActions.Enqueue(() => SetColor(new Color32(210, 238, 130, 100)));
                        break;
                    case KeyStatus.Sustain:
                        mainThreadActions.Enqueue(() => SetColor(new Color32(210, 238, 130, 100)));
                        break;
                    case KeyStatus.Release:
                        mainThreadActions.Enqueue(() => SetColor(new Color32(255, 255, 225, 100)));
                        break;
                }
            }
        }

        public void SetColor(Color32 color)
        {
            imageComponent.color = color;
        }

        public void HighlightNote()
        {
            var imageComponent = GetComponent<Image>();

            if (imageComponent != null)
            {
                imageComponent.color = new Color32(255, 255, 225, 100);
            }
        }

        public Clef GetClef() => this.clef.Clef;

        public KeySignature GetSignature() =>
            this.transform
                .parent
                .GetComponentsInChildren<MonoKeySignature>()
                ?.FirstOrDefault(sign => sign.line == this.line)
                ?.keySignature
                .KeySignature ?? KeySignature.Natural;

        public void ApplyInLine()
        {
            if (this.line != 12 && this.line != 0)
                return;

            GameObject linha = new GameObject("Linha");
            linha.transform.SetParent(this.transform);
            linha.transform.localScale = Vector3.one;
            Image linhaImage = linha.AddComponent<Image>();
            linhaImage.color = Color.black;

            RectTransform linhaRectTransform = linha.GetComponent<RectTransform>();
            linhaRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            linhaRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            linhaRectTransform.pivot = new Vector2(0.5f, 0.5f);

            linhaRectTransform.sizeDelta = new Vector2(50f, 2.5f);
            linhaRectTransform.anchoredPosition = this.GetComponent<Image>()
                .GetSpritePivotPosition();
        }

        public override string ToString()
        {
            return $"NOTE {note.Tempo} - LINE {line} / PAGE {page} / CLEF {clef.Clef}";
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.button  == PointerEventData.InputButton.Left)
               musicSheet.ChangeNote(this);
            else if(eventData.button == PointerEventData.InputButton.Right)
               musicSheet.RemoveNote(this);

            base.OnPointerClick(eventData);
        }

        public override void OnPointerEnter(PointerEventData data) => musicSheet.SetHover(0);
        
    }
}
