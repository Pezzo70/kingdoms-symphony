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

        public static IList<KeyPlayed> ToKeysPlayed(IList<Note> notes)
        {
            IList<KeyPlayed> keysPlayed = new List<KeyPlayed>();


            var orderedNotes = notes.OrderBy(n => n.page).ThenBy(n => n.xPos).AsReadOnlyList();
            float beatDuration = 60.0f / 60;

            for (int i = 0; i < orderedNotes.Count; i++)
            {
                var note = orderedNotes[i];
                KeyName name = note.note.NoteBehaviour is NotationBehaviour.Pause ? KeyName.Pause : FindNote(note.GetClef(), note.line, note.GetSignature());
                KeyPlayed key = new KeyPlayed()
                {
                    Name = name,
                    TimePlayed = i == 0 ? 0 : keysPlayed[i - 1].TimeReleased,
                };
                key.TimeReleased = key.TimePlayed + note.note.Tempo.ToFloat() * beatDuration;

                keysPlayed.Add(key);
            }

            return keysPlayed;
        }

        public static KeyName FindNote(Clef clef, int index, KeySignature signature)
        {
            //# -1, B +1
            Debug.Log(signature);
            KeyName[] notesOnLinesAndSpaces = null;
            index++;
            if (signature == KeySignature.Sharp) index--;
            else if (signature == KeySignature.Flat) index++;

            if (clef == Clef.G)
                notesOnLinesAndSpaces = new KeyName[]
                {
                    KeyName.B3,
                    KeyName.C4,
                    KeyName.CSharp4,
                    KeyName.D4,
                    KeyName.DSharp4,
                    KeyName.E4,
                    KeyName.F4,
                    KeyName.FSharp4,
                    KeyName.G4,
                    KeyName.GSharp4,
                    KeyName.A4,
                    KeyName.ASharp4,
                    KeyName.B4,
                    KeyName.C5,
                    KeyName.CSharp5,
                    KeyName.D5,
                    KeyName.DSharp5,
                    KeyName.E5,
                    KeyName.F5,
                    KeyName.FSharp5,
                    KeyName.G5,
                    KeyName.GSharp5,
                    KeyName.A5,
                    KeyName.ASharp5,
                };
            else if (clef == Clef.F)
                notesOnLinesAndSpaces = new KeyName[]
                {
                    KeyName.DSharp2,
                    KeyName.E2,
                    KeyName.F2,
                    KeyName.FSharp2,
                    KeyName.G2,
                    KeyName.GSharp2,
                    KeyName.A2,
                    KeyName.ASharp2,
                    KeyName.B2,
                    KeyName.C3,
                    KeyName.CSharp3,
                    KeyName.D3,
                    KeyName.DSharp3,
                    KeyName.E3,
                    KeyName.F3,
                    KeyName.FSharp3,
                    KeyName.G3,
                    KeyName.GSharp3,
                    KeyName.A3,
                    KeyName.ASharp3,
                    KeyName.B3,
                    KeyName.C4,
                    KeyName.CSharp4,
                    KeyName.D4
                };
            if (index >= 0 && index < notesOnLinesAndSpaces.Length)
                return notesOnLinesAndSpaces[index == notesOnLinesAndSpaces.Length - 1 ? --index : index];

            return KeyName.C3;
        }

        public Clef GetClef() => this.clef.Clef;
        public KeySignature GetSignature() => this.transform.parent.GetComponentsInChildren<MonoKeySignature>()?.FirstOrDefault(sign => sign.line == this.line)?.keySignature.KeySignature ?? KeySignature.Natural;
        public override string ToString()
        {
            return $"NOTE {note.Tempo} - LINE {line} / PAGE {page} / CLEF {clef.Clef}";
        }

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


            linhaRectTransform.sizeDelta = new Vector2(1.10f, 0.05f);
            linhaRectTransform.anchoredPosition = this.GetComponent<Image>().GetSpritePivotPosition();
        }
    }
}
