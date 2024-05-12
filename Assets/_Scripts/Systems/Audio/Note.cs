using UnityEngine;
using Kingdom.Audio.Procedural;
using System.Collections.Generic;
using Kingdom.Enums.MusicTheory;
using System.Linq;
using Kingdom.Enums;
using static Kingdom.Audio.Procedural.Frequencies;


namespace Kingdom.Audio
{
    public class Note : MonoBehaviour
    {

        public NotationScriptable note;
        public ClefScriptable clef;
        public float xPos;
        public int line;
        public int page;

        public IList<KeyPlayed> ToKeysPlayed(IList<Note> notes)
        {
            IList<KeyPlayed> keysPlayed = new List<KeyPlayed>();


            var orderedNotes = notes.OrderBy(n => n.page).ThenBy(n => n.xPos);

            foreach(var note in orderedNotes)
            {
                KeyPlayed key = new KeyPlayed()
                {
                    Name = FindNote(clef.Clef, note.line),
                    TimePlayed = 0f 
                };

                keysPlayed.Add(key);
            }

            return keysPlayed;
        }

        public static KeyName FindNote(Clef clef, int index)
        {
            if (clef == Clef.G)
            {
                // Notas correspondentes às linhas e espaços na clave de sol
                KeyName[] notesOnLinesAndSpaces = { KeyName.F4, KeyName.G4, KeyName.A4, KeyName.B4, KeyName.C5, KeyName.D5, KeyName.E5, KeyName.F5, KeyName.G5, KeyName.A5, KeyName.B5, KeyName.C6 };

                // Verifica se o índice está dentro do intervalo válido
                if (index >= 0 && index < notesOnLinesAndSpaces.Length)
                {
                    return notesOnLinesAndSpaces[index];
                }
            }
            else if (clef == Clef.F)
            {
                // Notas correspondentes às linhas e espaços na clave de fá
                KeyName[] notesOnLinesAndSpaces = { KeyName.G2, KeyName.A2, KeyName.B2, KeyName.C3, KeyName.D3, KeyName.E3, KeyName.F3, KeyName.G3, KeyName.A3, KeyName.B3, KeyName.C4, KeyName.D4 };

                // Verifica se o índice está dentro do intervalo válido
                if (index >= 0 && index < notesOnLinesAndSpaces.Length)
                {
                    return notesOnLinesAndSpaces[index];
                }
            }
            return KeyName.C3;
        }
    }
}