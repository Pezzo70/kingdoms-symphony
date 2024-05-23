using System;
using System.Collections.Generic;
using System.Linq;
using Assets.SimpleLocalization.Scripts;
using Kingdom.Audio;
using Kingdom.Effects;
using Kingdom.Enemies;
using Kingdom.Enums;
using Kingdom.Enums.Enemies;
using Kingdom.Enums.MusicTheory;
using Kingdom.Enums.Scrolls;
using Kingdom.Extensions;
using Kingdom.Level;
using Kingdom.Player;
using Kingdom.Scrolls;
using Unity.VisualScripting;
using UnityEngine;

namespace Kingdom
{
    public static class ScrollsAndEffectsHandler
    {
        public static void ValidateAndExecuteScrollAction(
            ScrollDTO scrollDTO,
            ref float damage,
            ref float massiveDamage
        )
        {
            bool accomplished = false;
            ValidatorResult validatorResult;
            EffectDTO effectDTO;
            ScrollsContainer allScrolls = ScrollsContainer.Instance;
            IEnumerable<Note> notes = EffectsAndScrollsManager.Instance.playedNotes;
            GameObject playerPrefab = GameObject.FindGameObjectWithTag("PlayerPrefab");
            GameObject[] enemies = GameObject
                .FindGameObjectsWithTag("Enemy")
                .ToList()
                .Where(obj => !obj.GetComponent<EnemyEntity>().IsDead)
                .ToArray();
            (Turn, int) turn = PlaythroughContainer.Instance.currentTurn;
            PlayerStats ps = PlaythroughContainer.Instance.PlayerStats;

            switch (scrollDTO.Scroll.scrollID)
            {
                case ScrollID.FirstMajorNotes:
                    validatorResult = ScrollValidator.CheckTargetScale(
                        notes,
                        NotationExtensions.GetKeysFromMode(
                            Modes.Ionian,
                            NotationExtensions.ScaleTonics[scrollDTO.TargetScales[0]]
                        )
                    );
                    accomplished = validatorResult.Result;
                    if (accomplished)
                    {
                        effectDTO = new EffectDTO(
                            playerPrefab.name,
                            GetScrollEffectDescription(scrollDTO.Scroll),
                            playerPrefab,
                            scrollDTO.Scroll.effectsSymbols[0],
                            EffectTarget.Player,
                            false,
                            EffectType.PlayerMitigation,
                            true,
                            turn.Item2,
                            (int)scrollDTO.Scroll.zFactor + turn.Item2,
                            Mathf.Clamp(
                                validatorResult.Factor * scrollDTO.Scroll.xFactor,
                                0,
                                scrollDTO.Scroll.yFactor
                            )
                        );
                        EventManager.AddEffect?.Invoke(effectDTO);
                    }
                    break;
                case ScrollID.ExploringMajorMelodies:
                    validatorResult = ScrollValidator.CheckTargetScaleInFirstMeasure(
                        notes,
                        NotationExtensions.GetKeysFromMode(
                            Modes.Ionian,
                            NotationExtensions.ScaleTonics[scrollDTO.TargetScales[0]]
                        )
                    );
                    accomplished = validatorResult.Result;
                    if (accomplished)
                    {
                        damage += Mathf.Clamp(
                            validatorResult.Factor * scrollDTO.Scroll.xFactor,
                            0f,
                            scrollDTO.Scroll.yFactor
                        );
                    }
                    break;
                case ScrollID.PausesToRest:
                    validatorResult = ScrollValidator.CheckPausePerMeasure(notes, Tempo.Quarter);
                    accomplished = validatorResult.Result;
                    if (accomplished)
                        ps.GainMoral(ps.MaxMoral * scrollDTO.Scroll.xFactor / 100f);

                    break;
                case ScrollID.ChangingStrategies:
                    validatorResult = ScrollValidator.CheckClef(notes, Clef.F);
                    accomplished = validatorResult.Result;
                    if (accomplished)
                    {
                        EffectsAndScrollsManager
                            .Instance
                            .burnedScrolls
                            .ForEach(obj => obj.internalCounter += (int)scrollDTO.Scroll.xFactor);
                    }
                    break;
                case ScrollID.BetweenTones:
                    validatorResult = ScrollValidator.CheckToneAndSemitoneMajorScale(
                        notes,
                        scrollDTO.TargetScales[0],
                        NotationExtensions.ModeOrderKey.Semitone
                    );
                    accomplished = validatorResult.Result;
                    if (accomplished)
                    {
                        damage += Mathf.Clamp(
                            validatorResult.Factor * scrollDTO.Scroll.xFactor,
                            0,
                            scrollDTO.Scroll.yFactor
                        );
                    }
                    break;
                case ScrollID.BetweenSemitones:
                    validatorResult = ScrollValidator.CheckToneAndSemitoneMajorScale(
                        notes,
                        scrollDTO.TargetScales[0],
                        NotationExtensions.ModeOrderKey.Tone
                    );
                    accomplished = validatorResult.Result;
                    if (accomplished)
                    {
                        damage += Mathf.Clamp(
                            validatorResult.Factor * scrollDTO.Scroll.xFactor,
                            0,
                            scrollDTO.Scroll.yFactor
                        );
                    }
                    break;
                case ScrollID.MoreChords:
                    int finalFactor = 0;
                    for (int i = 0; i < scrollDTO.TargetChords.Length; i++)
                    {
                        validatorResult = ScrollValidator.CheckChordsOnTargetMeasure(
                            notes,
                            scrollDTO.TargetChords[i],
                            2
                        );
                        if (accomplished == false && validatorResult.Result)
                            accomplished = true;
                        finalFactor += (int)validatorResult.Factor;
                    }

                    if (accomplished)
                    {
                        effectDTO = new EffectDTO(
                            playerPrefab.name,
                            GetScrollEffectDescription(scrollDTO.Scroll),
                            playerPrefab,
                            scrollDTO.Scroll.effectsSymbols[0],
                            EffectTarget.Player,
                            false,
                            EffectType.DamageModifier,
                            true,
                            turn.Item2,
                            (int)scrollDTO.Scroll.yFactor + turn.Item2,
                            finalFactor * (scrollDTO.Scroll.xFactor / 100)
                        );
                        EventManager.AddEffect?.Invoke(effectDTO);
                    }

                    break;
                case ScrollID.KeynoteStrength:
                    validatorResult = ScrollValidator.CheckWholeTonic(
                        notes,
                        NotationExtensions.ChordsNote[scrollDTO.TargetChords[0]][0]
                    );
                    accomplished = validatorResult.Result;
                    if (accomplished)
                    {
                        massiveDamage += scrollDTO.Scroll.xFactor;
                    }
                    break;
                case ScrollID.Ambiguity:
                    validatorResult = ScrollValidator.CheckEnarmonics(notes, true);
                    accomplished = validatorResult.Result;
                    if (accomplished)
                    {
                        ps.GainMoral(
                            Mathf.Clamp(
                                scrollDTO.Scroll.xFactor * validatorResult.Factor,
                                0,
                                scrollDTO.Scroll.yFactor
                            )
                        );

                        enemies
                            .ToList()
                            .ForEach(enemy =>
                            {
                                effectDTO = new EffectDTO(
                                    enemy.name,
                                    GetScrollEffectDescription(scrollDTO.Scroll),
                                    enemy,
                                    scrollDTO.Scroll.effectsSymbols[0],
                                    EffectTarget.Enemy,
                                    false,
                                    EffectType.PreventEnemyHeal,
                                    true,
                                    turn.Item2,
                                    turn.Item2 + 1,
                                    0f
                                );
                                EventManager.AddEffect?.Invoke(effectDTO);
                            });
                    }
                    break;
                case ScrollID.PassageAndControl:
                    validatorResult = ScrollValidator.CheckDominantAndSubdominant(
                        notes,
                        NotationExtensions.GetKeysFromMode(
                            Modes.Ionian,
                            NotationExtensions.ScaleTonics[scrollDTO.TargetScales[0]]
                        ),
                        3
                    );
                    accomplished = validatorResult.Result;
                    if (accomplished)
                    {
                        effectDTO = new EffectDTO(
                            playerPrefab.name,
                            GetScrollEffectDescription(scrollDTO.Scroll),
                            playerPrefab,
                            scrollDTO.Scroll.effectsSymbols[0],
                            EffectTarget.Player,
                            true,
                            EffectType.AdditionalMana,
                            true,
                            turn.Item2,
                            turn.Item2 + 1,
                            Mathf.Clamp(
                                validatorResult.Factor * scrollDTO.Scroll.xFactor,
                                0f,
                                scrollDTO.Scroll.yFactor
                            )
                        );
                        EventManager.AddEffect?.Invoke(effectDTO);
                    }
                    break;
                case ScrollID.CommonProgress:
                    validatorResult = ScrollValidator.CheckProgressions(
                        notes,
                        scrollDTO.TargetChords[0],
                        0
                    );
                    accomplished = validatorResult.Result;
                    if (accomplished)
                    {
                        damage += scrollDTO.Scroll.xFactor;
                    }
                    break;
                case ScrollID.HealingThroughArpeggios:
                    validatorResult = ScrollValidator.CheckArpeggioOnDifferentMeasure(
                        notes,
                        scrollDTO.TargetChords[0],
                        3
                    );
                    accomplished = validatorResult.Result;
                    if (accomplished)
                    {
                        ps.GainMoral(scrollDTO.Scroll.xFactor);
                        EffectsAndScrollsManager.Instance.ClearAllPlayersEffects();
                    }
                    break;
                case ScrollID.BetweenScales:
                    validatorResult = ScrollValidator.CheckNotesFromMajorScale(
                        notes,
                        scrollDTO.TargetScales
                    );
                    accomplished = validatorResult.Result;
                    if (accomplished)
                    {
                        enemies
                            .ToList()
                            .ForEach(enemy =>
                            {
                                var attacks = enemy.GetComponent<EnemyEntity>().enemyData.attacks;
                                effectDTO = new EffectDTO(
                                    enemy.name,
                                    GetScrollEffectDescription(scrollDTO.Scroll),
                                    enemy,
                                    scrollDTO.Scroll.effectsSymbols[0],
                                    EffectTarget.Enemy,
                                    false,
                                    EffectType.Stun,
                                    true,
                                    turn.Item2,
                                    turn.Item2 + (int)scrollDTO.Scroll.xFactor,
                                    (int)
                                        attacks[
                                            UnityEngine.Random.Range(0, attacks.Count())
                                        ].enemyAttackID
                                );
                                EventManager.AddEffect?.Invoke(effectDTO);
                            });
                    }
                    break;
                case ScrollID.Melodic:
                    validatorResult = ScrollValidator.CheckMelodyComposition(notes);
                    accomplished = validatorResult.Result;
                    if (accomplished)
                    {
                        damage += notes.Count() / 2f * (0.1f * (scrollDTO.Scroll.xFactor / 100f));
                    }
                    break;
                case ScrollID.PianistWithModes:
                    validatorResult = ScrollValidator.CheckMode(
                        notes,
                        Modes.Ionian,
                        NotationExtensions.ScaleTonics[scrollDTO.TargetScales[0]]
                    );
                    accomplished = validatorResult.Result;
                    if (accomplished)
                    {
                        massiveDamage += scrollDTO.Scroll.xFactor;
                        ps.GainMoral(scrollDTO.Scroll.yFactor);
                        effectDTO = new EffectDTO(
                            playerPrefab.name,
                            GetScrollEffectDescription(scrollDTO.Scroll),
                            playerPrefab,
                            scrollDTO.Scroll.effectsSymbols[0],
                            EffectTarget.Player,
                            false,
                            EffectType.PlayerMitigation,
                            true,
                            turn.Item2,
                            (int)scrollDTO.Scroll.zFactor + turn.Item2,
                            scrollDTO.Scroll.wFactor
                        );
                        EventManager.AddEffect?.Invoke(effectDTO);
                    }
                    break;
            }

            if (accomplished)
                EventManager.ScrollAccomplished?.Invoke(scrollDTO.Scroll.scrollID);
            else
                EventManager.ScrollFailed?.Invoke(scrollDTO.Scroll.scrollID);
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
                    /*
                        NOT APPLIED HERE, APPLIED ON CAST BY MODIFYING burnedScrolls.internalCounter += VALUE
                        Apply directly on Burned Scrolls right after the scroll objective is accomplished
                    */
                    break;
                case EffectType.Damage:
                    //APPLIED HERE FOR DMG P/ TURN AND THINGS LIKE THAT
                    //MUST INVOKE EventManager.DamageEffectExecuted after
                    break;
                case EffectType.DamageModifier:
                    //APPLIED HERE
                    break;
                case EffectType.MassiveDamage:
                    //NOT APPLIED HERE, MASSIVE DAMAGE IS NOT A PERSISTED EFFECT
                    break;
                case EffectType.Heal:
                    //NOT APPLIED HERE SINCE THERE IS NO HEAL PER TURN
                    break;
                case EffectType.AdditionalMana:
                    //NOT APPLIED HERE, ADDED ELSEWHERE
                    break;
                case EffectType.AdditionalManaScrollCost:
                    //NOT APPLIED HERE, VERIFIED ELSEWHERE
                    break;
                case EffectType.RemoveAllEffects:
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
                case EffectType.ReduceMana:
                    //NOT APPLIED HERE
                    break;
                case EffectType.SpendMana:
                    //NOT APPLIED HERE
                    break;
                case EffectType.PreventPlayerHeal:
                    //NOT APPLIED HERE
                    break;
                case EffectType.ReduceAvailableSheetBars:
                    //NOT APPLIED HERE
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
            //INVOKE THE FOLLOWING EVENTS WHEN THE ADVANTAGE OR DISADVANTAGE TRIGGER
            //public static UnityAction<EnemyID, EnemyAdvantageID> EnemyAdvantageTriggered;
            //public static UnityAction<EnemyID, EnemyDisadvantageID> EnemyDisadvantageTriggered;

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

        private static string GetScrollEffectDescription(Scroll scroll)
        {
            string effect = LocalizationManager.Localize("Scrolls.Effect." + (int)scroll.scrollID);

            effect = effect.Replace("-X", scroll.xFactor.ToString());
            effect = effect.Replace("-Y", scroll.yFactor.ToString());
            effect = effect.Replace("-Z", scroll.zFactor.ToString());
            effect = effect.Replace("-W", scroll.wFactor.ToString());

            return effect;
        }
    }
}
