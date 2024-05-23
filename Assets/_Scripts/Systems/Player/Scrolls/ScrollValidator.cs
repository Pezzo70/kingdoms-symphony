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
using static Kingdom.Audio.Procedural.Frequencies;
namespace Kingdom
{
    public static class ScrollValidator
    {
        //1
        public static ValidatorResult CheckTargetScale(IEnumerable<Note> notes, KeyName[] targetScaleArray) 
        {
            bool result = notes.All(n => targetScaleArray.Contains(n.ToKey()));
            return new ValidatorResult(result, result ? notes.Count() : 0);
        }
        
        //2
        public static ValidatorResult CheckTargetScaleInOneCompass(IEnumerable<Note> notes, KeyName[] targetScaleArray) 
        {   
            var result = notes.GroupBy(n => n.page).FirstOrDefault(g => g.All(n => targetScaleArray.Contains(n.ToKey())));
            return new ValidatorResult(result != null, result.Count());
        }


        //3
        public static bool CheckPausePerCompass(IEnumerable<Note> notes, Tempo tempo)
        {
            int pages = notes.Select(n => n.page).Distinct().Count();
            float tempoCast = tempo.ToFloat();
            for (int i = 0; i < pages; i++)
                if (notes.Where(n => n.page == i && n.note.NoteBehaviour == NotationBehaviour.Pause).Sum(n => n.note.Tempo.ToFloat()) != tempoCast)
                    return false;

            return true;
        }

        //4
        public static bool CheckClef(IEnumerable<Note> notes, Clef clef) => notes.Any(n => n.clef.Clef == clef);

        //5 & 6
        public static ValidatorResult CheckSemiTones(IEnumerable<Note> notes, bool tone)
        {
            var notesByCompass = notes.GroupBy(n => n.page);
            bool result = true;
            float factor = 0;
            foreach (var compassGroup in notesByCompass)
            {
                var sortedNotes = compassGroup.OrderBy(n => n.xPos).ToList();

                for (int i = 1; i < sortedNotes.Count(); i++)
                {
                    var prevNote = sortedNotes[i - 1];
                    var currNote = sortedNotes[i];
                    if (Math.Abs(currNote.ToKey().GetKeyIndexInClef(currNote.clef.Clef) - prevNote.ToKey().GetKeyIndexInClef(prevNote.clef.Clef)) != (1 + Convert.ToInt32(tone)))
                        {
                            result = false;
                            factor= 0;
                        }
                    else
                        factor++;
                }
            }

            return new ValidatorResult(result, factor);
        }

        //7
        public static bool CheckWholeTonic(IEnumerable<Note> notes, KeyName[] keyNames)
        {      
            return notes.Any(n => n.note.Tempo is Tempo.Whole && n.ToKey() == keyNames[0]);
     
        }

        //8
        public static bool CheckEnarmonics(IEnumerable<Note> notes)
        {
            return notes.Any(note => notes.Any(n => n.ToKey() != KeyName.C4 && n.line != note.line && n.ToKey() == note.ToKey()));
        }

        //9
        public static bool CheckDominants(IEnumerable<Note> note, KeyName[,] keyNames)
        {
            return true;
        }
        //10
        public static bool CheckProgressions(IEnumerable<Note> note, KeyName[] keyNames)
        {
            return true;
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
            if(notes.Select(n => n.page).Distinct().Count() < 4)
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
        public static bool CheckKeys(IEnumerable<Note> notes, KeyName[] keys) => notes.All(n => keys.Contains(n.ToKey()));
    }

    public struct ValidatorResult
    {
        bool Result {get;set;}
        float Factor{get;set;}
        public ValidatorResult(bool result, float factor) : this()
        {
            Result = result;
            Factor = factor;
        }
    }
}