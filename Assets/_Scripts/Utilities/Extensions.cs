using System.Collections.Generic;
using System.Linq;
using Kingdom.Audio;
using Kingdom.Audio.Procedural;
using Kingdom.Enums;
using Kingdom.Enums.MusicTheory;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Kingdom.Audio.Procedural.Frequencies;

namespace Kingdom.Extensions
{

    public static class NotationExtensions
    {
        private static KeyName[] GClefKeys = new KeyName[]
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
        private static KeyName[] FClefKeys = new KeyName[]
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
                    KeyName.D4
            };


        public static KeyName ToKey(this Note note)
        {
            int index = note.line;
            var array = note.clef.Clef == Clef.G ? GClefKeys : FClefKeys;
            var signature = note.GetSignature();

            index++;
            if (signature == KeySignature.Sharp) index--;
            else if (signature == KeySignature.Flat) index++;

            if (index >= 0 && index < array.Length)
                return array[index == array.Length - 1 ? --index : index];

            return KeyName.CSharp4;
        }

        public static IList<KeyPlayed> ToKeysPlayed(this IList<Note> notes)
        {
            IList<KeyPlayed> keysPlayed = new List<KeyPlayed>();

            var orderedNotes = notes.OrderBy(n => n.page).ThenBy(n => n.xPos).AsReadOnlyList();
            float beatDuration = 60.0f / 30;

            for (int i = 0; i < orderedNotes.Count; i++)
            {
                var note = orderedNotes[i];
                KeyName name;
                KeyPlayed key;
                var chords = note.GetChords(orderedNotes);
                if(chords.Count > 1)
                {
                    int iAux = i - 1;
                    KeyPlayed biggestRelease = i == 0 ? new KeyPlayed(){Name = KeyName.Pause, TimePlayed = 0, TimeReleased = 0} : keysPlayed[i - 1];
                    foreach(var chord in chords) 
                    {
                        var activePause = chord.GetActivePause(orderedNotes, keysPlayed) ?? biggestRelease;
                        biggestRelease = biggestRelease.TimeReleased > activePause.TimeReleased ? biggestRelease : activePause;
                    }

                    foreach(var chord in chords)
                    {
                        iAux++;
                        name = chord.ToKey();
                        key = new KeyPlayed
                        {
                            Name = name,
                            TimePlayed = biggestRelease.TimeReleased,
                        };
                        key.TimeReleased = key.TimePlayed + note.note.Tempo.ToFloat() * beatDuration;
                        keysPlayed.Add(key);
                    }
                    i = iAux;
                    continue;
                }
                else
                {
                    name = note.note.NoteBehaviour is NotationBehaviour.Pause ? KeyName.Pause : note.ToKey();
                    key = new KeyPlayed(){ Name = name };
                    key.TimePlayed = i == 0 ? 0 : keysPlayed[i - 1].TimeReleased;
                    key.TimeReleased = key.TimePlayed + note.note.Tempo.ToFloat() * beatDuration;
                    keysPlayed.Add(key);
                }
            }


            return keysPlayed;
        }

        public static KeyPlayed GetActivePause(this Note note, IList<Note> orderedNotes, IList<KeyPlayed> keysPlayed)
        {
            var key = orderedNotes.Where(x => x.note.NoteBehaviour is NotationBehaviour.Pause && x.line == note.line && note.xPos > x.xPos).OrderByDescending(x => x.xPos).FirstOrDefault();
            return key is null ? null : keysPlayed[orderedNotes.IndexOf(key)];
        }

        public static IList<Note> GetChords(this Note note, IList<Note> notes) => notes.Where(x => Mathf.Abs(x.xPos - note.xPos) <= 15f && x.page == note.page).AsReadOnlyList();



    }

    public static class SpriteExtensions
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


    }
}