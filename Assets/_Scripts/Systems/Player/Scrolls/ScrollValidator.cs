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
        public static ValidatorResult CheckTargetScale(
            IEnumerable<Note> notes,
            KeyName[] targetScaleArray
        )
        {
            bool result = notes.All(n => targetScaleArray.Contains(n.ToKey()));
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

            Debug.Log(firstCompassNotes.Any(n => n.GetChord(notes.ToList()).Count() > 0));
            if (firstCompassNotes.Any(n => n.GetChord(notes.ToList()).Count() > 0))
                return new ValidatorResult(false, 0);

            bool result = firstCompassNotes.All(n => targetScaleArray.Contains(n.ToKey()));
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

            result = notes.All(obj => targets.Contains(obj.ToKey()));

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
            var result = notes.Where(
                obj => obj.page == targetMeasure && filterScale.Contains(obj.ToKey())
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

        //11
        public static bool CheckArpeggioByCompass(IEnumerable<Note> notes, KeyName[,] chords)
        {
            var notesByCompass = notes.AsReadOnlyList().GroupByCompass();
            if (notesByCompass.Count < chords.GetLength(0))
                return false;

            for (int i = 0; i < chords.GetLength(0); i++)
            {
                if (!notesByCompass.ContainsKey(i + 1))
                    return false;

                var notesInCompass = notesByCompass[i + 1];
                var arpeggio = chords.GetLength(1);

                if (notesInCompass.Count < arpeggio)
                    return false;

                for (int j = 0; j < arpeggio; j++)
                {
                    if (notesInCompass[j].ToKey() != chords[i, j])
                        return false;
                }
            }

            return true;
        }

        //12
        public static bool CheckBetweenScales(IEnumerable<Note> notes, KeyName[,] keys)
        {
            if (notes.Select(n => n.page).Distinct().Count() < 4)
                return false;

            return true;
        }

        //13
        public static bool CheckMelodyComposition(IEnumerable<Note> notes, int totalCompasses)
        {
            var notesList = notes.ToList();
            int compasses = notes.Select(n => n.page).Distinct().Count();
            if (compasses > (totalCompasses / 2.0f))
                return false;

            float sumTempoPerCompass = notes.Sum(n => n.note.Tempo.ToFloat());
            if (sumTempoPerCompass > compasses)
                return false;

            return !notesList.Any(n => n.GetChord(notesList).Count() > 1);
        }

        //14
        public static bool CheckKeys(IEnumerable<Note> notes, KeyName[] keys) =>
            notes.All(n => keys.Contains(n.ToKey()));

        //15
        //Em todos os compassos disponíveis, componha utilizando apenas as notas seguindo um Modo aleatório a partir de uma nota tônica de um acorde maior aleatório.
        public static bool CheckMode(IEnumerable<Note> note, Modes mode, KeyName[] chord)
        {
            var modeNotes = NotationExtensions.GetKeysFromMode(mode, chord[0]);
            return note.All(n => modeNotes.Contains(n.ToKey()));
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
