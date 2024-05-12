using UnityEngine;


namespace Kingdom.Audio
{
    public class Note : MonoBehaviour
    {

        NotationScriptable note;
        float xPos;
        int line;
        int page;

        /*public static IList<KeyPlayed> ToKeysPlayed(this IList<Note> notes)
        {
            IList<KeyPlayed> keysPlayed = new List<KeyPlayed>();


            var orderedNotes = notes.OrderBy(n => n.page).ThenBy(n => n.xPos);

            foreach(var note in orderedNotes)
            {
                KeyPlayed key = new KeyPlayed()
                {
                    Name = Frequencies.KeyName.G2,
                    TimePlayed = 
                };

                keysPlayed.Add(key);
            }

            return keysPlayed;
        }

        private Frequencies.KeyName GetKeyNameByChord(int line, int chord)*/
    }
}