using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private static KeyName[] GClefKeysNatural = new KeyName[]
        {
            KeyName.C4,
            KeyName.D4,
            KeyName.E4,
            KeyName.F4,
            KeyName.G4,
            KeyName.A4,
            KeyName.B4,
            KeyName.C5,
            KeyName.D5,
            KeyName.E5,
            KeyName.F5,
            KeyName.G5,
            KeyName.A5,
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

        private static KeyName[] FCLefKeysNatural = new KeyName[]
        {
            KeyName.E2,
            KeyName.F2,
            KeyName.G2,
            KeyName.A2,
            KeyName.B2,
            KeyName.C3,
            KeyName.D3,
            KeyName.E3,
            KeyName.F3,
            KeyName.G3,
            KeyName.A3,
            KeyName.B3,
            KeyName.C4
        };

        public enum ModeOrderKey
        {
            Tone = 0,
            Semitone = 1
        }

        public static Dictionary<Chords, Chords[]> ChordsProgressionIivV = new Dictionary<
            Chords,
            Chords[]
        >
        {
            { Chords.CMajor, new Chords[] { Chords.CMajor, Chords.FMajor, Chords.GMajor } },
            { Chords.DMajor, new Chords[] { Chords.DMajor, Chords.GMajor, Chords.AMajor } },
            { Chords.EMajor, new Chords[] { Chords.EMajor, Chords.AMajor, Chords.BMajor } },
            { Chords.FMajor, new Chords[] { Chords.FMajor, Chords.BbMinor, Chords.CMajor } },
            { Chords.GMajor, new Chords[] { Chords.GMajor, Chords.CMajor, Chords.DMajor } },
            { Chords.AMajor, new Chords[] { Chords.AMajor, Chords.DMajor, Chords.EMajor } },
            { Chords.BMajor, new Chords[] { Chords.BMajor, Chords.EMajor, Chords.FSharpMajor } }
        };

        public static Dictionary<Chords, SimpleNotes[]> ChordsNote = new Dictionary<
            Chords,
            SimpleNotes[]
        >()
        {
            { Chords.CMajor, new SimpleNotes[] { SimpleNotes.C, SimpleNotes.E, SimpleNotes.G } },
            {
                Chords.CSharpMajor,
                new SimpleNotes[] { SimpleNotes.CSharp, SimpleNotes.F, SimpleNotes.GSharp }
            },
            {
                Chords.DMajor,
                new SimpleNotes[] { SimpleNotes.D, SimpleNotes.FSharp, SimpleNotes.A }
            },
            {
                Chords.EbMajor,
                new SimpleNotes[] { SimpleNotes.DSharp, SimpleNotes.G, SimpleNotes.ASharp }
            },
            {
                Chords.EMajor,
                new SimpleNotes[] { SimpleNotes.E, SimpleNotes.GSharp, SimpleNotes.B }
            },
            { Chords.FMajor, new SimpleNotes[] { SimpleNotes.F, SimpleNotes.A, SimpleNotes.C } },
            {
                Chords.FSharpMajor,
                new SimpleNotes[] { SimpleNotes.FSharp, SimpleNotes.ASharp, SimpleNotes.CSharp }
            },
            { Chords.GMajor, new SimpleNotes[] { SimpleNotes.G, SimpleNotes.B, SimpleNotes.D } },
            {
                Chords.AbMajor,
                new SimpleNotes[] { SimpleNotes.GSharp, SimpleNotes.C, SimpleNotes.DSharp }
            },
            {
                Chords.AMajor,
                new SimpleNotes[] { SimpleNotes.A, SimpleNotes.CSharp, SimpleNotes.E }
            },
            {
                Chords.BbMajor,
                new SimpleNotes[] { SimpleNotes.ASharp, SimpleNotes.D, SimpleNotes.F }
            },
            {
                Chords.BMajor,
                new SimpleNotes[] { SimpleNotes.B, SimpleNotes.DSharp, SimpleNotes.FSharp }
            },
            {
                Chords.CMinor,
                new SimpleNotes[] { SimpleNotes.C, SimpleNotes.DSharp, SimpleNotes.G }
            },
            {
                Chords.CSharpMinor,
                new SimpleNotes[] { SimpleNotes.CSharp, SimpleNotes.E, SimpleNotes.GSharp }
            },
            { Chords.DMinor, new SimpleNotes[] { SimpleNotes.D, SimpleNotes.F, SimpleNotes.A } },
            {
                Chords.EbMinor,
                new SimpleNotes[] { SimpleNotes.DSharp, SimpleNotes.FSharp, SimpleNotes.ASharp }
            },
            { Chords.EMinor, new SimpleNotes[] { SimpleNotes.E, SimpleNotes.G, SimpleNotes.B } },
            {
                Chords.FMinor,
                new SimpleNotes[] { SimpleNotes.F, SimpleNotes.GSharp, SimpleNotes.C }
            },
            {
                Chords.FSharpMinor,
                new SimpleNotes[] { SimpleNotes.FSharp, SimpleNotes.A, SimpleNotes.CSharp }
            },
            {
                Chords.GMinor,
                new SimpleNotes[] { SimpleNotes.G, SimpleNotes.ASharp, SimpleNotes.D }
            },
            {
                Chords.AbMinor,
                new SimpleNotes[] { SimpleNotes.GSharp, SimpleNotes.B, SimpleNotes.DSharp }
            },
            { Chords.AMinor, new SimpleNotes[] { SimpleNotes.A, SimpleNotes.C, SimpleNotes.E } },
            {
                Chords.BbMinor,
                new SimpleNotes[] { SimpleNotes.ASharp, SimpleNotes.CSharp, SimpleNotes.F }
            },
            {
                Chords.BMinor,
                new SimpleNotes[] { SimpleNotes.B, SimpleNotes.D, SimpleNotes.FSharp }
            }
        };

        public static Dictionary<Modes, ModeOrderKey[]> ModeOrder = new Dictionary<
            Modes,
            ModeOrderKey[]
        >()
        {
            {
                Modes.Ionian,
                new ModeOrderKey[]
                {
                    ModeOrderKey.Tone,
                    ModeOrderKey.Tone,
                    ModeOrderKey.Semitone,
                    ModeOrderKey.Tone,
                    ModeOrderKey.Tone,
                    ModeOrderKey.Tone,
                    ModeOrderKey.Semitone
                }
            },
            {
                Modes.Dorian,
                new ModeOrderKey[]
                {
                    ModeOrderKey.Semitone,
                    ModeOrderKey.Tone,
                    ModeOrderKey.Tone,
                    ModeOrderKey.Tone,
                    ModeOrderKey.Semitone,
                    ModeOrderKey.Tone,
                    ModeOrderKey.Tone
                }
            },
            {
                Modes.Lydian,
                new ModeOrderKey[]
                {
                    ModeOrderKey.Tone,
                    ModeOrderKey.Tone,
                    ModeOrderKey.Tone,
                    ModeOrderKey.Semitone,
                    ModeOrderKey.Tone,
                    ModeOrderKey.Tone,
                    ModeOrderKey.Semitone
                }
            },
            {
                Modes.Mixolydian,
                new ModeOrderKey[]
                {
                    ModeOrderKey.Tone,
                    ModeOrderKey.Tone,
                    ModeOrderKey.Semitone,
                    ModeOrderKey.Tone,
                    ModeOrderKey.Tone,
                    ModeOrderKey.Semitone,
                    ModeOrderKey.Tone
                }
            },
            {
                Modes.Aeolian,
                new ModeOrderKey[]
                {
                    ModeOrderKey.Tone,
                    ModeOrderKey.Semitone,
                    ModeOrderKey.Tone,
                    ModeOrderKey.Tone,
                    ModeOrderKey.Semitone,
                    ModeOrderKey.Tone,
                    ModeOrderKey.Tone
                }
            },
            {
                Modes.Locrian,
                new ModeOrderKey[]
                {
                    ModeOrderKey.Semitone,
                    ModeOrderKey.Tone,
                    ModeOrderKey.Tone,
                    ModeOrderKey.Semitone,
                    ModeOrderKey.Tone,
                    ModeOrderKey.Tone,
                    ModeOrderKey.Tone
                }
            },
        };

        public static Dictionary<Scale, KeyName> ScaleTonics = new Dictionary<Scale, KeyName>()
        {
            { Scale.MajorC, KeyName.C4 },
            { Scale.MajorD, KeyName.D4 },
            { Scale.MajorE, KeyName.E4 },
            { Scale.MajorF, KeyName.F4 },
            { Scale.MajorG, KeyName.G4 },
            { Scale.MajorA, KeyName.A4 },
            { Scale.MajorB, KeyName.B4 },
        };

        public static KeyName[] GetKeysFromMode(Modes mode, KeyName key)
        {
            var order = ModeOrder[mode];
            List<KeyName> keys = new List<KeyName>();
            KeyName[] array = (KeyName[])Enum.GetValues(typeof(KeyName));
            int startIndex = array.ToList().FindIndex(obj => obj == key);
            keys.Add(array[startIndex]);
            int outsiderIndex = 1;
            for (int i = 0; i < order.Length; i++)
            {
                int alter = order[i] == ModeOrderKey.Tone ? 2 : 1;
                int current = array.ToList().FindIndex(obj => obj == keys[outsiderIndex - 1]);
                keys.Add(array[current + alter]);
                outsiderIndex++;
            }

            return keys.ToArray();
        }

        public static KeyName ToKey(this Note note)
        {
            if (note.note.NoteBehaviour is NotationBehaviour.Pause)
                return KeyName.Pause;
            int index = note.line;
            var array = note.clef.Clef == Clef.G ? GClefKeysNatural : FCLefKeysNatural;
            var arrayComplete = note.clef.Clef == Clef.G ? GClefKeys : FClefKeys;
            var signature = note.GetSignature();

            if (signature == KeySignature.Sharp)
            {
                int targetIndex = arrayComplete.ToList().FindIndex(obj => obj == array[index]) + 1;
                return arrayComplete[targetIndex];
            }
            else if (signature == KeySignature.Flat)
            {
                int targetIndex = arrayComplete.ToList().FindIndex(obj => obj == array[index]) - 1;
                return arrayComplete[targetIndex];
            }

            if (index >= 0 && index < array.Length)
                return array[index == array.Length - 1 ? --index : index];

            return KeyName.CSharp4;
        }

        public static string ToKeyText(this Note note)
        {
            if (note.note.NoteBehaviour is NotationBehaviour.Pause)
                return KeyName.Pause.ToString();

            int index = note.line;
            var array = note.clef.Clef == Clef.G ? GClefKeysNatural : FCLefKeysNatural;
            var signature = note.GetSignature();

            string additional = "";

            if (signature == KeySignature.Sharp)
                additional = "Sharp";
            else if (signature == KeySignature.Flat)
                additional = "Flat";

            return KeyToSimpleNote(array[index]).ToString() + additional;
        }

        public static IList<Key> ToKeysPlayed(this IList<Note> notes)
        {
            IList<Key> keysPlayed = new List<Key>();

            var orderedNotes = notes.OrderBy(n => n.page).ThenBy(n => n.xPos).AsReadOnlyList();
            float beatDuration = 60.0f / 30;

            for (int i = 0; i < orderedNotes.Count; i++)
            {
                var note = orderedNotes[i];
                KeyName name;
                Key key;
                var chords = note.GetChord(orderedNotes, false);
                if (chords.Count > 1)
                {
                    int iAux = i - 1;

                    Key biggestRelease =
                        i == 0
                            ? new Key()
                            {
                                Name = KeyName.Pause,
                                TimePlayed = 0,
                                TimeReleased = 0
                            }
                            : keysPlayed[i - 1];

                    foreach (var chord in chords)
                    {
                        var activePause =
                            chord.GetActivePause(orderedNotes, keysPlayed) ?? biggestRelease;
                        biggestRelease =
                            biggestRelease.TimeReleased > activePause.TimeReleased
                                ? biggestRelease
                                : activePause;
                    }

                    foreach (var chord in chords)
                    {
                        name = chord.ToKey();
                        key = new Key { Name = name, TimePlayed = biggestRelease.TimeReleased, };
                        key.TimeReleased =
                            key.TimePlayed + note.note.Tempo.ToFloat() * beatDuration;
                        keysPlayed.Add(key);
                        key.PropertyChanged += chord.OnKeyStatusChanged;
                        iAux++;
                    }
                    i = iAux;
                    continue;
                }
                else
                {
                    name =
                        note.note.NoteBehaviour is NotationBehaviour.Pause
                            ? KeyName.Pause
                            : note.ToKey();
                    key = new Key() { Name = name };
                    key.TimePlayed = i == 0 ? 0 : keysPlayed[i - 1].TimeReleased;
                    key.TimeReleased = key.TimePlayed + note.note.Tempo.ToFloat() * beatDuration;
                    key.PropertyChanged += note.OnKeyStatusChanged;
                    keysPlayed.Add(key);
                }
            }

            return keysPlayed;
        }

        public static Key GetActivePause(
            this Note note,
            IList<Note> orderedNotes,
            IList<Key> keysPlayed
        )
        {
            var key = orderedNotes
                .Where(
                    x =>
                        x.note.NoteBehaviour is NotationBehaviour.Pause
                        && x.line == note.line
                        && note.xPos > x.xPos
                )
                .OrderByDescending(x => x.xPos)
                .FirstOrDefault();
            return key is null ? null : keysPlayed[orderedNotes.IndexOf(key)];
        }

        public static List<Note> GetChord(
            this Note note,
            IList<Note> notes,
            bool considerLine = true
        )
        {
            if (considerLine)
            {
                return notes
                    .Where(
                        x =>
                            Mathf.Abs(x.xPos - note.xPos) <= 1f
                            && x.page == note.page
                            && note.line != x.line
                    )
                    .ToList();
            }

            return notes
                .Where(x => Mathf.Abs(x.xPos - note.xPos) <= 1f && x.page == note.page)
                .ToList();
        }

        public static Dictionary<int, List<Note>> NotesByMeasure(this IList<Note> note) =>
            note.GroupBy(n => n.page).ToDictionary(g => g.Key, g => g.ToList());

        public static int GetKeyIndexInClef(this KeyName key, Clef clef)
        {
            KeyName[] keys = clef is Clef.G ? GClefKeys : FClefKeys;
            return keys.AsReadOnlyList().IndexOf(key);
        }

        public static SimpleNotes KeyToSimpleNote(KeyName key) =>
            (SimpleNotes)
                Enum.Parse(typeof(SimpleNotes), string.Concat(key.ToString().Where(char.IsLetter)));
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

    public static class CollectionsHelper
    {
        public static void VerifyAndAddToDictionary<T>(Dictionary<T, int> dictionary, T target)
        {
            if (dictionary.ContainsKey(target))
                dictionary[target]++;
            else
                dictionary.Add(target, 1);
        }

        public static void VerifyAndAddToDictionaryList<T, TT>(
            Dictionary<T, List<TT>> dictionary,
            T target,
            TT value
        )
        {
            if (dictionary.ContainsKey(target))
            {
                if (!dictionary[target].Contains(value))
                    dictionary[target].Add(value);
            }
            else
            {
                dictionary.Add(target, new List<TT>());
                dictionary[target].Add(value);
            }
        }

        public static void VerifyAndAddToList<T>(List<T> list, T target)
        {
            if (list.Contains(target))
                return;
            else
                list.Add(target);
        }
    }
}
