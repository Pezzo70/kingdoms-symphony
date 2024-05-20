using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kingdom.Audio;
using Kingdom.Enums.Enemies;
using Kingdom.Enums.MusicTheory;
using Kingdom.Enums.Scrolls;
using Kingdom.Extensions;
using static Kingdom.Audio.Procedural.Frequencies;

namespace Kingdom
{
    public static class TheoryHandler
    {

        public static IEnumerable<EffectInfo> GetAction(ScrollEffectDTO effectDTO)
        {
            //@TODO: SCALE AND MODES TARGET
            KeyName[] keyNamesScaleMock = new KeyName[] { KeyName.C0, KeyName.CSharp0};
            KeyName[] keyNameToneMock = new KeyName[] { KeyName.C0, KeyName.CSharp0};
            KeyName[] keyNameSemiToneMock = new KeyName[] { KeyName.C0, KeyName.CSharp0};
            
            IList<EffectInfo> effectInfos = new List<EffectInfo>();

            switch (effectDTO.Scroll.scrollID)
            {
                case ScrollID.FirstMajorNotes:
                    float mitigationIncrease = effectDTO
                                               .Notes
                                               .Count(n => keyNamesScaleMock.Contains(n.ToKey())) * effectDTO.Scroll.xFactor;
                    mitigationIncrease = Math.Min(mitigationIncrease, effectDTO.Scroll.yFactor);
                    effectInfos.Add(
                        new EffectInfo()
                        {
                            EffectType = EffectType.PlayerMitigation,
                            Turns = 1,
                            Function = (value) => value + mitigationIncrease
                        }
                    );
                    break;

                case ScrollID.ExploringMajorMelodies:
                    float damageIncrease = effectDTO
                                           .Notes
                                           .Count(n => keyNamesScaleMock.Contains(n.ToKey())) * effectDTO.Scroll.xFactor;
                    damageIncrease = Math.Min(damageIncrease, effectDTO.Scroll.yFactor);
                    effectInfos.Add(
                        new EffectInfo()
                        {
                            EffectType = EffectType.Damage,
                            Turns = 1,
                            Function = (damage) => damage + damageIncrease
                        }
                    );
                    break;

                case ScrollID.PausesToRest:
                    float healAmount = effectDTO.Scroll.xFactor;
                    healAmount = Math.Min(healAmount, effectDTO.Scroll.yFactor);
                    effectInfos.Add(
                        new EffectInfo()
                        {
                            EffectType = EffectType.Heal,
                            Turns = 3,
                            Function = (value) => value + healAmount
                        }
                    );
                    break;

                case ScrollID.ChangingStrategies:
                    float cooldownReduction = effectDTO.Scroll.xFactor;
                    effectInfos.Add(
                        new EffectInfo()
                        {
                            EffectType = EffectType.CooldownReduction,
                            Turns = 3,
                            Function = (cooldown) => cooldown - cooldownReduction
                        }
                    );
                    break;

                case ScrollID.BetweenTones:
                    float toneDamage = effectDTO.TargetSemiTone * effectDTO.Scroll.xFactor;
                    toneDamage = Math.Min(toneDamage, effectDTO.Scroll.yFactor);
                    effectInfos.Add(
                        new EffectInfo()
                        {
                            EffectType = EffectType.Damage,
                            Turns = 2,
                            Function = (value) => value + toneDamage
                        }
                    );
                    break;

                case ScrollID.BetweenSemitones:
                    float semitoneDamage = effectDTO.TargetKey * effectDTO.Scroll.xFactor;
                    semitoneDamage = Math.Min(semitoneDamage, effectDTO.Scroll.yFactor);
                    effectInfos.Add(
                        new EffectInfo()
                        {
                            EffectType = EffectType.Damage,
                            Turns = 2,
                            Function = (value) => value + semitoneDamage
                        }
                    );
                    break;

                case ScrollID.MoreChords:
                    float chordDamage = effectDTO.Scroll.xFactor;
                    effectInfos.Add(
                        new EffectInfo()
                        {
                            EffectType = EffectType.Damage,
                            Turns = (int)effectDTO.Scroll.yFactor,
                            Function = (value) => value + chordDamage
                        }
                    );
                    break;

                case ScrollID.KeynoteStrength:
                    float massiveDamage = effectDTO.Scroll.xFactor;
                    effectInfos.Add(
                        new EffectInfo()
                        {
                            EffectType = EffectType.MassiveDamage,
                            Turns = 5,
                            Function = (value) => value + massiveDamage
                        }
                    );
                    break;

                case ScrollID.Ambiguity:
                    float enarmonicHealing = effectDTO.Notes.Count * effectDTO.Scroll.xFactor;
                    enarmonicHealing = Math.Min(enarmonicHealing, effectDTO.Scroll.yFactor);
                    effectInfos.Add(
                        new EffectInfo()
                        {
                            EffectType = EffectType.Heal,
                            Turns = 1,
                            Function = (value) => value + enarmonicHealing
                        }
                    );
                    effectInfos.Add(
                        new EffectInfo()
                        {
                            EffectType = EffectType.PreventEnemyHeal,
                            Turns = 1,
                            Function = (value) => value
                        }
                    );
                    break;

                case ScrollID.PassageAndControl:
                    float additionalMana = effectDTO.Notes.Count * effectDTO.Scroll.xFactor;
                    additionalMana = Math.Min(additionalMana, effectDTO.Scroll.yFactor);
                    effectInfos.Add(
                        new EffectInfo()
                        {
                            EffectType = EffectType.AdditionalMana,
                            Turns = 1,
                            Function = (value) => value + additionalMana
                        }
                    );
                    break;

                case ScrollID.CommonProgress:
                    float mitigationDecrease = effectDTO.Scroll.xFactor;
                    effectInfos.Add(
                        new EffectInfo()
                        {
                            EffectType = EffectType.EnemyMitigation,
                            Turns = (int)effectDTO.Scroll.yFactor,
                            Function = (value) => value - mitigationDecrease
                        }
                    );
                    break;

                case ScrollID.HealingThroughArpeggios:
                    float arpeggioHealing = effectDTO.Scroll.xFactor;
                    effectInfos.Add(
                        new EffectInfo()
                        {
                            EffectType = EffectType.RemoveNegativeEffects,
                            Turns = 1,
                            Function = (value) => value
                        }
                    );
                    effectInfos.Add(
                        new EffectInfo()
                        {
                            EffectType = EffectType.Heal,
                            Turns = 8,
                            Function = (value) => value + arpeggioHealing
                        }
                    );
                    break;

                case ScrollID.BetweenScales:
                    float stunTurns = effectDTO.Scroll.xFactor;
                    effectInfos.Add(
                        new EffectInfo()
                        {
                            EffectType = EffectType.Stun,
                            Turns = (int)stunTurns,
                            Function = (value) => value + stunTurns
                        }
                    );
                    break;

                case ScrollID.Melodic:
                    float melodicDamageMultiplier = effectDTO.Scroll.xFactor;
                    effectInfos.Add(
                        new EffectInfo()
                        {
                            EffectType = EffectType.Damage,
                            Turns = 1,
                            Function = (value) => value * melodicDamageMultiplier
                        }
                    );
                    break;

                case ScrollID.PianistWithModes:
                    float pianistDamage = effectDTO.Scroll.xFactor;
                    float pianistHealing = effectDTO.Scroll.yFactor;
                    float pianistMitigation = effectDTO.Scroll.wFactor;
                    effectInfos.Add(
                        new EffectInfo()
                        {
                            EffectType = EffectType.MassiveDamage,
                            Turns = 1,
                            Function = (value) => pianistDamage
                        }
                    );
                    effectInfos.Add(
                        new EffectInfo()
                        {
                            EffectType = EffectType.Heal,
                            Turns = 1,
                            Function = (value) => value + pianistHealing
                        }
                    );
                    effectInfos.Add(
                        new EffectInfo()
                        {
                            EffectType = EffectType.PlayerMitigation,
                            Turns = (int)effectDTO.Scroll.zFactor,
                            Function = (value) => value + pianistMitigation
                        }
                    );
                    break;
            }

            return effectInfos;
        }
    
    
        public static IEnumerable<EffectInfo> GetAction(Enemies.EnemyAdvantage enemyAdvantage)
        {
            return null;
        }

        public static IEnumerable<EffectInfo> GetAction(Enemies.EnemyDisadvantage enemyDisadvantage)
        {
            return null;
        }
    }


    //@TODO Change struct file
    public struct EffectInfo
    {
        public EffectType EffectType { get; set; }
        public int Turns { get; set; }
        public Func<float, float> Function { get; set; }
    }

    //@TODO Change dto file
    public struct ScrollEffectDTO
    {
        public IList<Note> Notes { get; set;}
        public Scroll Scroll { get; set;}
        public Clef? TargetClef { get; set;}
        public Chords? TargetChord { get; set; }
        public Scale? TargetScale { get; set;}
        public Modes? TargetModes { get; set; }
        public int TargetSemiTone { get;set; }
        public int TargetKey { get; set; }


        public ScrollEffectDTO(Scroll scroll, IList<Note> notes) : this()
        {
            Scroll = scroll;
            Notes = notes;
        }
    }


    //@TODO Change enum file
    public enum EffectType
    {
        PlayerMitigation, EnemyMitigation, CooldownReduction, Damage, MassiveDamage, Heal, AdditionalMana, RemoveNegativeEffects,
        PreventEnemyHeal,
        Stun,
    }
}