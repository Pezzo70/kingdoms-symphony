using System;
using System.Collections.Generic;
using System.Linq;
using Kingdom.Audio;
using Kingdom.Enemies;
using Kingdom.Enums;
using Kingdom.Enums.Enemies;
using Kingdom.Enums.MusicTheory;
using Kingdom.Enums.Scrolls;
using Kingdom.Extensions;
using Unity.VisualScripting;
using UnityEngine;
using static Kingdom.Audio.Procedural.Frequencies;
using static Kingdom.Extensions.NotationExtensions;

namespace Kingdom
{
    public static class ScrollValidator
    {
        public static ValidatorResult CheckNote(IEnumerable<Note> notes, SimpleNotes[] targetNotes)
        {
            bool result = notes.Any(n => targetNotes.Contains(KeyToSimpleNote(n.ToKey())));
            return new ValidatorResult(result, 0f);
        }

        public static ValidatorResult CheckTargetScale(
            IEnumerable<Note> notes,
            KeyName[] targetScaleArray
        )
        {
            var transposed = targetScaleArray.Select(obj => KeyToSimpleNote(obj)).ToList();
            bool result = notes.All(n => transposed.Contains(KeyToSimpleNote(n.ToKey())));
            Debug.Log(string.Join(",", transposed));
            return new ValidatorResult(
                result,
                result ? notes.Count(obj => targetScaleArray.Contains(obj.ToKey())) : 0
            );
        }

        public static ValidatorResult CheckTargetScaleInFirstMeasure(
            IEnumerable<Note> notes,
            KeyName[] targetScaleArray
        )
        {
            List<Note> firstCompassNotes = notes.Where(obj => obj.page == 0).ToList();

            if (firstCompassNotes.Any(n => n.GetChord(notes.ToList()).Count() > 0))
                return new ValidatorResult(false, 0);

            var transposed = targetScaleArray.Select(obj => KeyToSimpleNote(obj)).ToList();
            Debug.Log(string.Join(",", transposed));
            bool result = notes.All(n => transposed.Contains(KeyToSimpleNote(n.ToKey())));
            return new ValidatorResult(
                result,
                result ? notes.Count(obj => targetScaleArray.Contains(obj.ToKey())) : 0
            );
        }

        public static ValidatorResult CheckPausePerMeasure(IEnumerable<Note> notes, Tempo tempo)
        {
            int pages = notes.Select(n => n.page).Distinct().Count();
            float tempoCast = tempo.ToFloat();
            bool isValid = false;
            for (int i = 0; i < pages; i++)
            {
                float totalPauseTempo = notes
                    .Where(n => n.page == i && n.note.NoteBehaviour == NotationBehaviour.Pause)
                    .Sum(n => n.note.Tempo.ToFloat());
                if (totalPauseTempo > tempoCast || totalPauseTempo == 0f)
                {
                    isValid = false;
                    break;
                }
                else
                {
                    isValid = true;
                }
            }

            return new ValidatorResult(isValid, 0);
        }

        public static ValidatorResult CheckClef(IEnumerable<Note> notes, Clef clef) =>
            new ValidatorResult(notes.Any(n => n.clef.Clef == clef), 0f);

        public static ValidatorResult CheckToneAndSemitoneMajorScale(
            IEnumerable<Note> notes,
            Scale scale,
            ModeOrderKey modeOrderKey
        )
        {
            List<Note> firstMeasureNotes = notes.Where(obj => obj.page == 0).ToList();
            bool result = true;
            var arr = NotationExtensions.GetKeysFromMode(
                Modes.Ionian,
                NotationExtensions.ScaleTonics[scale]
            );
            var order = NotationExtensions.ModeOrder[Modes.Ionian];
            List<KeyName> targets = new List<KeyName>();
            for (int i = 0; i < order.Length; i++)
            {
                if (order[i] == modeOrderKey)
                    targets.Add(arr[i]);
            }

            var transposed = targets.Select(obj => KeyToSimpleNote(obj)).ToList();

            result = notes.All(obj => transposed.Contains(KeyToSimpleNote(obj.ToKey())));

            return new ValidatorResult(result, result ? notes.Count() : 0);
        }

        public static ValidatorResult CheckChordsOnTargetMeasure(
            IEnumerable<Note> notes,
            Chords chord,
            int targetMeasure
        )
        {
            List<Note> targetMeasureNotes = notes.Where(obj => obj.page == targetMeasure).ToList();
            bool result = false;
            targetMeasureNotes.ForEach(obj =>
            {
                List<Note> chordsNotes = obj.GetChord(targetMeasureNotes);
                if (chordsNotes.Count > 1)
                {
                    if (
                        chordsNotes.All(
                            notes =>
                                ChordsNote[chord].Contains(
                                    Extensions.NotationExtensions.KeyToSimpleNote(notes.ToKey())
                                )
                        )
                    )
                    {
                        result = true;
                    }
                }
            });

            return new ValidatorResult(result, result ? 1 : 0);
        }

        public static ValidatorResult CheckWholeTonic(IEnumerable<Note> notes, SimpleNotes keyName)
        {
            bool res = notes.Any(
                n => n.note.Tempo is Tempo.Whole && KeyToSimpleNote(n.ToKey()) == keyName
            );
            return new ValidatorResult(res, 0);
        }

        public static ValidatorResult CheckEnarmonics(
            IEnumerable<Note> notes,
            bool differentMeasures = false
        )
        {
            bool check = false;
            if (differentMeasures)
            {
                var found = notes
                    .Where(
                        note =>
                            notes.Any(
                                n =>
                                    n.ToKey() != KeyName.C4
                                    && n.page != note.page
                                    && n.line != note.line
                                    && n.ToKey() == note.ToKey()
                            )
                    )
                    .ToList();
                check = found.Count > 0;
                return new ValidatorResult(check, found.Count);
            }
            else
            {
                var found = notes
                    .Where(
                        note =>
                            notes.Any(
                                n =>
                                    n.ToKey() != KeyName.C4
                                    && n.line != note.line
                                    && n.ToKey() == note.ToKey()
                            )
                    )
                    .ToList();
                check = found.Count > 0;
                return new ValidatorResult(check, found.Count);
            }
        }

        public static ValidatorResult CheckDominantAndSubdominant(
            IEnumerable<Note> notes,
            KeyName[] scale,
            int targetMeasure
        )
        {
            var filterScale = new KeyName[] { scale[3], scale[4] };
            var filterScaleTransposed = filterScale.Select(obj => KeyToSimpleNote(obj)).ToList();
            var result = notes.Where(
                obj =>
                    obj.page == targetMeasure
                    && filterScaleTransposed.Contains(KeyToSimpleNote(obj.ToKey()))
            );
            bool found = result.Count() > 0;
            return new ValidatorResult(found, result.Count());
        }

        //10
        public static ValidatorResult CheckProgressions(
            IEnumerable<Note> note,
            Chords targetChord,
            int targetMeasure
        )
        {
            var notesOnTargetMeasure = note.Where(obj => obj.page == targetMeasure).ToList();
            var targetChords = Extensions
                .NotationExtensions
                .ChordsProgressionIivV[targetChord]
                .ToList();
            bool[] chordsPlayed = { false, false, false };
            int chordsPlayedIndex = 0;
            for (int i = 0; i < notesOnTargetMeasure.Count(); i++)
            {
                var localChord = notesOnTargetMeasure[i].GetChord(notesOnTargetMeasure, false);
                var localChordsToSimple = localChord.Select(obj => KeyToSimpleNote(obj.ToKey()));
                if (localChord.Count > 1)
                {
                    chordsPlayed[chordsPlayedIndex] = ChordsNote[
                        targetChords[chordsPlayedIndex]
                    ].All(obj => localChordsToSimple.Contains(obj));
                    i += 2;
                    chordsPlayedIndex++;
                }
                else
                {
                    break;
                }
            }
            return new ValidatorResult(chordsPlayed.All(r => r == true), 0);
        }

        public static ValidatorResult CheckArpeggioOnDifferentMeasure(
            IEnumerable<Note> notes,
            Chords targetChord,
            int times
        )
        {
            var pages = notes.Select(obj => obj.page).Distinct().ToList();
            int counter = 0;
            for (int i = 0; i < pages.Count(); i++)
            {
                bool res = CheckArpeggioByMeasure(notes, targetChord, pages[i]);
                if (res)
                    counter++;
            }

            return new ValidatorResult(counter >= times, 0f);
        }

        public static bool CheckArpeggioByMeasure(
            IEnumerable<Note> notes,
            Chords targetChord,
            int targetMeasure
        )
        {
            var notesByMeasures = notes.Where(obj => obj.page == targetMeasure).ToList();
            var targetNotes = Extensions.NotationExtensions.ChordsNote[targetChord].ToList();
            bool[] checkNote = { false, false, false };
            int times = 0;

            var initIndex = notesByMeasures.FindIndex(
                obj => KeyToSimpleNote(obj.ToKey()) == targetNotes[0]
            );

            if (initIndex == -1)
                return false;

            int chordCounter = 0;
            for (int j = initIndex; j < notesByMeasures.Count; j++)
            {
                if (chordCounter > targetNotes.Count() - 1)
                    break;
                checkNote[times] =
                    KeyToSimpleNote(notesByMeasures[j].ToKey()) == targetNotes[chordCounter];
                chordCounter++;
                times++;
            }

            return checkNote.All(obj => obj == true);
        }

        public static ValidatorResult CheckNotesFromMajorScale(
            IEnumerable<Note> notes,
            Scale[] scales
        )
        {
            List<KeyName> keys = new List<KeyName>();
            foreach (var scale in scales)
            {
                keys.AddRange(
                    NotationExtensions.GetKeysFromMode(
                        Modes.Ionian,
                        NotationExtensions.ScaleTonics[scale]
                    )
                );
            }
            var keysTransposed = keys.Select(obj => KeyToSimpleNote(obj));
            return new ValidatorResult(
                notes.All(obj => keysTransposed.Contains(KeyToSimpleNote(obj.ToKey()))),
                0f
            );
        }

        public static ValidatorResult CheckMelodyComposition(IEnumerable<Note> notes)
        {
            return new ValidatorResult(
                notes.All(obj => obj.GetChord(notes.ToList()).Count == 0),
                0f
            );
        }

        public static ValidatorResult CheckMode(IEnumerable<Note> notes, Modes mode, KeyName note)
        {
            var modeNotes = NotationExtensions.GetKeysFromMode(mode, note);
            var modeNotesTransposed = modeNotes.Select(obj => KeyToSimpleNote(obj));
            return new ValidatorResult(
                notes.All(n => modeNotesTransposed.Contains(KeyToSimpleNote(n.ToKey()))),
                0f
            );
        }
    }

    public struct ValidatorResult
    {
        public bool Result;
        public float Factor;

        public ValidatorResult(bool result, float factor)
            : this()
        {
            Result = result;
            Factor = factor;
        }
    }
}
