using UnityEngine;
using Kingdom.Audio.Procedural;
using System.Collections.Generic;
using Kingdom.Enums.MusicTheory;
using System.Linq;
using Kingdom.Enums;
using static Kingdom.Audio.Procedural.Frequencies;
using Unity.VisualScripting;
using UnityEngine.UI;
using Kingdom.Extensions;


namespace Kingdom.Audio
{
    public class Note : MonoBehaviour
    {

        public NotationScriptable note;
        public ClefScriptable clef;
        public KeySignatureScriptable signature;
        public float xPos;
        public int line;
        public int page;


        public Clef GetClef() => this.clef.Clef;
        public KeySignature GetSignature() => this.transform.parent.GetComponentsInChildren<MonoKeySignature>()?.FirstOrDefault(sign => sign.line == this.line)?.keySignature.KeySignature ?? KeySignature.Natural;

        public void ApplyInLine()
        {
            if (this.line != 12 && this.line != 0) return;


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
            linhaRectTransform.anchoredPosition = this.GetComponent<Image>().GetSpritePivotPosition();
        }

        public override string ToString()
        {
            return $"NOTE {note.Tempo} - LINE {line} / PAGE {page} / CLEF {clef.Clef}";
        }

    }
}
