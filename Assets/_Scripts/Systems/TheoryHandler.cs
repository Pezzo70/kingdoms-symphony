using System;
using System.Collections.Generic;
using System.Linq;
using Kingdom.Audio;
using Kingdom.Effects;
using Kingdom.Enemies;
using Kingdom.Enums.Enemies;
using Kingdom.Enums.MusicTheory;
using Kingdom.Enums.Scrolls;
using Kingdom.Extensions;
using static Kingdom.Audio.Procedural.Frequencies;

namespace Kingdom
{
    public static class TheoryHandler
    {
        public static void ValidateAndExecuteScrollAction(
            ScrollDTO scrollDTO,
            ref float damage,
            ref float massiveDamage
        )
        {
            //@TODO: OBJECTIVE, SCALE AND MODES TARGET
            KeyName[] keyNamesScaleMock = new KeyName[] { KeyName.C0, KeyName.CSharp0 };
            KeyName[] keyNameToneMock = new KeyName[] { KeyName.C0, KeyName.CSharp0 };
            KeyName[] keyNameSemiToneMock = new KeyName[] { KeyName.C0, KeyName.CSharp0 };

            //     IList<EffectInfo> effectInfos = new List<EffectInfo>();
            //     var notes = EffectsAndScrollsManager.Instance.playedNotes;

            //     switch (scrollDTO.Scroll.scrollID)
            //     {
            //         case ScrollID.FirstMajorNotes:
            //             float mitigationIncrease =
            //                 notes.Count(n => keyNamesScaleMock.Contains(n.ToKey()))
            //                 * effectDTO.Scroll.xFactor;
            //             mitigationIncrease = Math.Min(mitigationIncrease, effectDTO.Scroll.yFactor);
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.PlayerMitigation,
            //                     Turns = 1,
            //                     Function = (value) => value + (value * (mitigationIncrease / 100))
            //                 }
            //             );
            //             break;

            //         case ScrollID.ExploringMajorMelodies:
            //             float damageIncrease =
            //                 notes.Count(n => keyNamesScaleMock.Contains(n.ToKey()))
            //                 * effectDTO.Scroll.xFactor;
            //             damageIncrease = Math.Min(damageIncrease, effectDTO.Scroll.yFactor);
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.Damage,
            //                     Turns = 1,
            //                     Function = (damage) => damage + (damage * (damageIncrease / 100))
            //                 }
            //             );
            //             break;

            //         case ScrollID.PausesToRest:
            //             float healAmount = effectDTO.Scroll.xFactor;
            //             healAmount = Math.Min(healAmount, effectDTO.Scroll.yFactor);
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.Heal,
            //                     Turns = 3,
            //                     Function = (value) => value + (value * (healAmount / 100))
            //                 }
            //             );
            //             break;

            //         case ScrollID.ChangingStrategies:
            //             float cooldownReduction = effectDTO.Scroll.xFactor;
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.CooldownReduction,
            //                     Turns = 3,
            //                     Function = (cooldown) => cooldown - cooldownReduction
            //                 }
            //             );
            //             break;

            //         case ScrollID.BetweenTones:

            //             int numberOfSemiTones = 0;
            //             float toneDamage = numberOfSemiTones * effectDTO.Scroll.xFactor;
            //             toneDamage = Math.Min(toneDamage, effectDTO.Scroll.yFactor);
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.Damage,
            //                     Turns = 2,
            //                     Function = (value) => value + toneDamage
            //                 }
            //             );
            //             break;

            //         case ScrollID.BetweenSemitones:
            //             int numberOfTones = 0;
            //             float semitoneDamage = numberOfTones * effectDTO.Scroll.xFactor;
            //             semitoneDamage = Math.Min(semitoneDamage, effectDTO.Scroll.yFactor);
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.Damage,
            //                     Turns = 2,
            //                     Function = (value) => value + semitoneDamage
            //                 }
            //             );
            //             break;

            //         case ScrollID.MoreChords:
            //             float chordDamage = effectDTO.Scroll.xFactor;
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.Damage,
            //                     Turns = (int)effectDTO.Scroll.yFactor,
            //                     Function = (value) => value + chordDamage
            //                 }
            //             );
            //             break;

            //         case ScrollID.KeynoteStrength:
            //             float massiveDamage = effectDTO.Scroll.xFactor;
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.MassiveDamage,
            //                     Turns = 5,
            //                     Function = (value) => value + massiveDamage
            //                 }
            //             );
            //             break;

            //         case ScrollID.Ambiguity:
            //             float enarmonicHealing = notes.Count * effectDTO.Scroll.xFactor;
            //             enarmonicHealing = Math.Min(enarmonicHealing, effectDTO.Scroll.yFactor);
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.Heal,
            //                     Turns = 1,
            //                     Function = (value) => value + enarmonicHealing
            //                 }
            //             );
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.PreventEnemyHeal,
            //                     Turns = 1,
            //                     Function = (value) => value
            //                 }
            //             );
            //             break;

            //         case ScrollID.PassageAndControl:
            //             float additionalMana = notes.Count * effectDTO.Scroll.xFactor;
            //             additionalMana = Math.Min(additionalMana, effectDTO.Scroll.yFactor);
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.AdditionalMana,
            //                     Turns = 1,
            //                     Function = (value) => value + additionalMana
            //                 }
            //             );
            //             break;

            //         case ScrollID.CommonProgress:
            //             float mitigationDecrease = effectDTO.Scroll.xFactor;
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.EnemyMitigation,
            //                     Turns = (int)effectDTO.Scroll.yFactor,
            //                     Function = (value) => value - mitigationDecrease
            //                 }
            //             );
            //             break;

            //         case ScrollID.HealingThroughArpeggios:
            //             float arpeggioHealing = effectDTO.Scroll.xFactor;
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.RemoveNegativeEffects,
            //                     Turns = 1,
            //                     Function = (value) => value
            //                 }
            //             );
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.Heal,
            //                     Turns = 8,
            //                     Function = (value) => value + arpeggioHealing
            //                 }
            //             );
            //             break;

            //         case ScrollID.BetweenScales:
            //             float stunTurns = effectDTO.Scroll.xFactor;
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.Stun,
            //                     Turns = (int)stunTurns,
            //                     Function = (value) => value + stunTurns
            //                 }
            //             );
            //             break;

            //         case ScrollID.Melodic:
            //             float melodicDamageMultiplier = effectDTO.Scroll.xFactor;
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.Damage,
            //                     Turns = 1,
            //                     Function = (value) => value * melodicDamageMultiplier
            //                 }
            //             );
            //             break;

            //         case ScrollID.PianistWithModes:
            //             float pianistDamage = effectDTO.Scroll.xFactor;
            //             float pianistHealing = effectDTO.Scroll.yFactor;
            //             float pianistMitigation = effectDTO.Scroll.wFactor;
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.MassiveDamage,
            //                     Turns = 1,
            //                     Function = (value) => pianistDamage
            //                 }
            //             );
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.Heal,
            //                     Turns = 1,
            //                     Function = (value) => value + pianistHealing
            //                 }
            //             );
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.PlayerMitigation,
            //                     Turns = (int)effectDTO.Scroll.zFactor,
            //                     Function = (value) => value + pianistMitigation
            //                 }
            //             );
            //             break;
            //     }

            //     return effectInfos;
            // }

            // public static IEnumerable<EffectInfo> GetAction(EnemyAdvantage enemyAdvantage)
            // {
            //     IList<EffectInfo> effectInfos = new List<EffectInfo>();

            //     switch (enemyAdvantage.enemyAdvantageID)
            //     {
            //         case EnemyAdvantageID.WerewolfsWill:
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.PlayerMitigation,
            //                     Function = (value) => value + (value * (enemyAdvantage.xFactor / 100)),
            //                     Turns = 1
            //                 }
            //             );
            //             break;

            //         case EnemyAdvantageID.MindsWill:
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.CompleteMitigation,
            //                     Function = (value) => value,
            //                     Turns = 1
            //                 }
            //             );
            //             break;

            //         case EnemyAdvantageID.HealingEyes:
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.Heal,
            //                     Function = (value) => value + (value * (enemyAdvantage.xFactor / 100)),
            //                     Turns = 1
            //                 }
            //             );
            //             break;

            //         case EnemyAdvantageID.WatchersWill:
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.PlayerMitigation,
            //                     Function = (value) => value + (value * (enemyAdvantage.xFactor / 100)),
            //                     Turns = 1
            //                 }
            //             );
            //             break;

            //         case EnemyAdvantageID.PhngluiMglwNafhCthulhuRlyehWgahNaglFhtagn:
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.Damage,
            //                     Turns = (int)enemyAdvantage.yFactor,
            //                     Function = (value) => value + (value * (enemyAdvantage.xFactor / 100))
            //                 }
            //             );
            //             break;
            //     }

            //     return effectInfos;
            // }

            // public static IEnumerable<EffectInfo> GetAction(EnemyDisadvantage enemyDisadvantage)
            // {
            //     IList<EffectInfo> effectInfos = new List<EffectInfo>();

            //     switch (enemyDisadvantage.enemyDisadvantageID)
            //     {
            //         case EnemyDisadvantageID.CantPauseHowls:
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.Damage,
            //                     Function = (dano) => dano + (dano * (enemyDisadvantage.xFactor / 100)),
            //                     Turns = 1
            //                 }
            //             );
            //             break;

            //         case EnemyDisadvantageID.Headaches:
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.Damage,
            //                     Function = (dano) => dano + (dano * (enemyDisadvantage.xFactor / 100)),
            //                     Turns = 1
            //                 }
            //             );
            //             break;

            //         case EnemyDisadvantageID.RightAtTheEyes:
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.Damage,
            //                     Function = (value) =>
            //                         value + (value * (enemyDisadvantage.xFactor / 100)),
            //                     Turns = 1
            //                 }
            //             );
            //             break;

            //         case EnemyDisadvantageID.ImNotMyFather:
            //             effectInfos.Add(
            //                 new EffectInfo()
            //                 {
            //                     EffectType = EffectType.Damage,
            //                     Function = (value) =>
            //                         value + (value * (enemyDisadvantage.xFactor / 100)),
            //                     Turns = 1
            //                 }
            //             );
            //             break;
            //     }

            //     return effectInfos;
        }

        public static void ValidateAndExecuteEffectAction(EffectDTO effectDTO, ref float damage)
        {
            //EXECUTE EFFECT ACTION, IF IS A DAMAGE MODIFIER OR SOMETHING
            switch (effectDTO.EffectType)
            {
                case EffectType.PlayerMitigation:
                    //APPLIED HERE
                    break;
                case EffectType.EnemyMitigation:
                    //APPLIED HERE
                    break;
                case EffectType.CooldownReduction:
                    //NOT APPLIED HERE, APPLIED ON CAST BY MODIFYING burnedScrolls.internalCounter += VALUE
                    break;
                case EffectType.Damage:
                    //APPLIED HERE FOR DMG P/ TURN AND THINGS LIKE THAT
                    break;
                case EffectType.DamageModifier:
                    //APPLIED HERE
                    break;
                case EffectType.MassiveDamage:
                    //NOT APPLIED HERE, MASSIVE DAMAGE IS NOT A PERSISTED EFFECT
                    break;
                case EffectType.Heal:
                    //VERIFY IF THERE IS ANY HEAL PER TURN, IF SO, THEN DEAL WITH IT HERE
                    break;
                case EffectType.AdditionalMana:
                    //NOT APPLIED HERE, ADDED ELSEWHERE
                    break;
                case EffectType.AdditionalManaScrollCost:
                    //NOT APPLIED HERE, VERIFIED ELSEWHERE
                    break;
                case EffectType.RemoveNegativeEffects:
                    //NOT APPLIED HERE, APPLIED ON CAST BY CALLING
                    break;
                case EffectType.PreventEnemyHeal:
                    //NOT APPLIED HERE, ALREADY HANDLING IT ON ENEMY ENTITY
                    break;
                case EffectType.CompleteMitigation:
                    //NOT APPLIED HERE
                    break;
                case EffectType.Stun:
                    //NOT APPLIED HERE, ALTER ON MUSIC SHEET OR WHEN NOTES ARE PLAYER
                    //IN CASE OF ENEMIES, ALREADY VERIFYING IT ON ENEMY ENTITY
                    break;
            }
        }

        public static void ValidateAndExecuteAdvantageDisadvantageAction(
            EnemyEntity enemyEntity,
            EnemyID enemyID,
            bool isAdvantage,
            ref float damage
        )
        {
            //IF IS IS ADVANTAGE, DEAL WITH ADVANTAGE STUFF, IF IT IS DISADVANTAGE, DEAL WITH DISADVANTAGE STUFF
            //IN ORDER TO GET THE NOTES EffectsAndScrollsManager.Instance.playedNotes
            //MODIFY DAMAGE VALUE FOR EACH ADVANTAGE AND FOR EACH DISADVANTAGE

            switch (enemyID)
            {
                case EnemyID.OutOfTuneGoblin:
                    break;
                case EnemyID.SteplessWerewolf:
                    break;
                case EnemyID.UnshakenBones:
                    break;
                case EnemyID.SoundlessShadows:
                    break;
                case EnemyID.SilencedClaws:
                    break;
                case EnemyID.SoundWatcher:
                    break;
                case EnemyID.AlienCaptain:
                    break;
                case EnemyID.AbyssalVisitor:
                    break;
            }
        }

        public static float SpawnEnemyAttackEffectsAndGetDamage(EnemyAttack enemyAttack)
        {
            float damage = 0f;
            //DOES NOT NEED TO UPDATE ENEMIES MANA, JUST SPAWN EFFECTS AND RETURN DAMAGE
            switch (enemyAttack.enemyAttackID)
            {
                case EnemyAttackID.Shove:
                    break;
                case EnemyAttackID.RythmlessClaps:
                    break;
                case EnemyAttackID.RythmlessClaws:
                    break;
                case EnemyAttackID.WaywardSteps:
                    break;
                case EnemyAttackID.UncontrollableGloves:
                    break;
                case EnemyAttackID.AirheadHeadbutt:
                    break;
                case EnemyAttackID.SilenceTheMind:
                    break;
                case EnemyAttackID.PoisonedFingers:
                    break;
                case EnemyAttackID.CuttingStrings:
                    break;
                case EnemyAttackID.ScratchingAndControlling:
                    break;
                case EnemyAttackID.StickyTentacles:
                    break;
                case EnemyAttackID.JudgeEyes:
                    break;
                case EnemyAttackID.SonicCrush:
                    break;
                case EnemyAttackID.HarmonicNullification:
                    break;
                case EnemyAttackID.AbysmalForce:
                    break;
                case EnemyAttackID.TroubledMind:
                    break;
            }

            return damage;
        }
    }
}
