using System.Collections.Generic;
using System.Linq;
using Kingdom.Audio;
using Kingdom.Audio.Procedural;
using Kingdom.Enums;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Kingdom.Audio.Procedural.Frequencies;

namespace Kingdom.Extensions
{

    public static class KingdomExtensions
    {
        public static Vector2 GetSpriteRelativePivot(this Image img)
        {

            Bounds bounds = img.sprite.bounds;
            var pivotX = -bounds.center.x / bounds.extents.x / 2 + 0.5f;
            var pivotY = -bounds.center.y / bounds.extents.y / 2 + 0.5f;
            Vector2 pivotPixel = new Vector2(pivotX, pivotY);


            return pivotPixel;
        }

        public static Vector2 GetSpritePivotPosition(this Image img)
        {
            Vector2 pivotRelative = img.GetSpriteRelativePivot();

            float xPos = (pivotRelative.x - 0.5f) * img.rectTransform.rect.width;
            float yPos = (pivotRelative.y - 0.5f) * img.rectTransform.rect.height;

            return new Vector2(xPos, yPos);
        }

        
        public static IList<KeyPlayed> ToKeysPlayed(this IList<Note> notes)
        {
            IList<KeyPlayed> keysPlayed = new List<KeyPlayed>();

            var orderedNotes = notes.OrderBy(n => n.page).ThenBy(n => n.xPos).AsReadOnlyList();
            float beatDuration = 60.0f / 60;

            for (int i = 0; i < orderedNotes.Count; i++)
            {
                var note = orderedNotes[i];
                KeyName name = note.note.NoteBehaviour is NotationBehaviour.Pause ? KeyName.Pause : Note.FindNote(note.GetClef(), note.line, note.GetSignature());
                KeyPlayed key = new KeyPlayed()
                {
                    Name = name,
                };
                
                if(i != 0 && orderedNotes[i - 1].xPos == note.xPos)
                {
                    key.TimePlayed = keysPlayed[i - 1].TimePlayed;
                    key.TimeReleased = keysPlayed[i - 1].TimeReleased;
                }
                else
                {
                    key.TimePlayed = i == 0 ? 0 : keysPlayed[i - 1].TimeReleased;
                    key.TimeReleased = key.TimePlayed + note.note.Tempo.ToFloat() * beatDuration;
                }

                keysPlayed.Add(key);
            }

            return keysPlayed;
        }
    }
}