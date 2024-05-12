using UnityEngine;
using Kingdom.Audio.Procedural;
using System.Collections.Generic;
using Kingdom.Enums.MusicTheory;
using System.Linq;
using Kingdom.Enums;
using static Kingdom.Audio.Procedural.Frequencies;
using Unity.VisualScripting;


namespace Kingdom.Audio
{
    public class Note : MonoBehaviour
    {

        public NotationScriptable note;
        public ClefScriptable clef;
        public float xPos;
        public int line;
        public int page;

        public static IList<KeyPlayed> ToKeysPlayed(IList<Note> notes)
        {
            IList<KeyPlayed> keysPlayed = new List<KeyPlayed>();


            var orderedNotes = notes.OrderBy(n => n.page).ThenBy(n => n.xPos).AsReadOnlyList();
            float beatDuration = 60.0f / 30;

            for(int i = 0; i < orderedNotes.Count; i++)
            {
                var note = orderedNotes[i];
                KeyPlayed key = new KeyPlayed()
                {
                    Name = FindNote(note.clef.Clef, note.line),
                    TimePlayed = i == 0 ? 0 : keysPlayed[i - 1].TimeReleased,
                };
                key.TimeReleased = key.TimePlayed + note.note.Tempo.ToFloat() * beatDuration;

                keysPlayed.Add(key);
            }

            return keysPlayed;
        }

        public static KeyName FindNote(Clef clef, int index)
        {
            if (clef == Clef.G)
            {
                KeyName[] notesOnLinesAndSpaces = { KeyName.F4, KeyName.G4, KeyName.A4, KeyName.B4, KeyName.C5, KeyName.D5, KeyName.E5, KeyName.F5, KeyName.G5, KeyName.A5, KeyName.B5, KeyName.C6 };

                if (index >= 0 && index < notesOnLinesAndSpaces.Length)
                {
                    return notesOnLinesAndSpaces[index];
                }
            }
            else if (clef == Clef.F)
            {
                KeyName[] notesOnLinesAndSpaces = { KeyName.G2, KeyName.A2, KeyName.B2, KeyName.C3, KeyName.D3, KeyName.E3, KeyName.F3, KeyName.G3, KeyName.A3, KeyName.B3, KeyName.C4, KeyName.D4 };

                if (index >= 0 && index < notesOnLinesAndSpaces.Length)
                {
                    return notesOnLinesAndSpaces[index];
                }
            }
            return KeyName.C3;
        }
    }
}