using UnityEngine;
using Kingdom.Audio.Procedural;
using System.Collections.Generic;
using Kingdom.Enums.MusicTheory;
using System.Linq;
using Kingdom.Enums;


namespace Kingdom.Audio
{
    public class Note : MonoBehaviour
    {

        public NotationScriptable note;
        public Scale scale;
        public float xPos;
        public int line;
        public int page;

        public static IList<KeyPlayed> ToKeysPlayed(IList<Note> notes)
        {
            IList<KeyPlayed> keysPlayed = new List<KeyPlayed>();


            var orderedNotes = notes.OrderBy(n => n.page).ThenBy(n => n.xPos);

            foreach(var note in orderedNotes)
            {
                KeyPlayed key = new KeyPlayed()
                {
                    Name = Frequencies.KeyName.G2,
                    TimePlayed = 0f 
                };

                keysPlayed.Add(key);
            }

            return keysPlayed;
        }

        private static Frequencies.KeyName GetKeyNameByScale(int line, Scale scale)
        {   
            return Frequencies.KeyName.C0;
        }
    }
}