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
using static Kingdom.Audio.Procedural.Frequencies;

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
                case EffectType.EnemyMitigation:
                    damage -= damage * (effectDTO.Modifier / 100f);
                    break;
                case EffectType.CooldownReduction:
                    /*
                        NOT APPLIED HERE, APPLIED ON CAST BY MODIFYING burnedScrolls.internalCounter += VALUE
                        Apply directly on Burned Scrolls right after the scroll objective is accomplished
                    */
                    break;
                case EffectType.Damage:
                    switch (effectDTO.EffectTarget)
                    {
                        case EffectTarget.Player:
                            PlaythroughContainer
                                .Instance
                                .PlayerStats
                                .ReduceMoral(effectDTO.Modifier);
                            EventManager.DamageEffectExecuted?.Invoke(effectDTO);
                            break;
                        case EffectTarget.Enemy:
                            effectDTO
                                .Target
                                .GetComponent<EnemyEntity>()
                                .ReduceMoral(effectDTO.Modifier, 0f);
                            break;
                    }
                    break;
                case EffectType.DamageModifier:
                    damage += damage * effectDTO.Modifier;
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

        public static float SpawnEnemyAttackEffectsAndGetDamage(EnemyAttack enemyAttack)
        {
            float damage = 0f;
            EffectDTO effectDTO;
            GameObject playerPrefab = GameObject.FindGameObjectWithTag("PlayerPrefab");
            (Turn, int) turn = PlaythroughContainer.Instance.currentTurn;
            switch (enemyAttack.enemyAttackID)
            {
                case EnemyAttackID.Shove:
                case EnemyAttackID.RythmlessClaps:
                case EnemyAttackID.WaywardSteps:
                case EnemyAttackID.UncontrollableGloves:
                case EnemyAttackID.CuttingStrings:
                case EnemyAttackID.StickyTentacles:
                case EnemyAttackID.HarmonicNullification:
                case EnemyAttackID.AbysmalForce:
                    damage = enemyAttack.xFactor;
                    break;
                case EnemyAttackID.RythmlessClaws:
                    damage = enemyAttack.xFactor;
                    effectDTO = new EffectDTO(
                        playerPrefab.name,
                        GetEnemyAttackDescription(enemyAttack),
                        playerPrefab,
                        enemyAttack.effectsSymbols[0],
                        EffectTarget.Player,
                        true,
                        EffectType.Damage,
                        true,
                        turn.Item2,
                        (int)enemyAttack.wFactor + turn.Item2,
                        enemyAttack.zFactor
                    );
                    EventManager.AddEffect?.Invoke(effectDTO);
                    break;
                case EnemyAttackID.AirheadHeadbutt:
                    damage = enemyAttack.xFactor;
                    effectDTO = new EffectDTO(
                        playerPrefab.name,
                        GetEnemyAttackDescription(enemyAttack),
                        playerPrefab,
                        enemyAttack.effectsSymbols[0],
                        EffectTarget.Player,
                        true,
                        EffectType.DamageModifier,
                        true,
                        turn.Item2,
                        (int)enemyAttack.wFactor + turn.Item2,
                        -enemyAttack.zFactor
                    );
                    EventManager.AddEffect?.Invoke(effectDTO);
                    break;
                case EnemyAttackID.SilenceTheMind:
                    damage = enemyAttack.xFactor;
                    effectDTO = new EffectDTO(
                        playerPrefab.name,
                        GetEnemyAttackDescription(enemyAttack),
                        playerPrefab,
                        enemyAttack.effectsSymbols[0],
                        EffectTarget.Player,
                        true,
                        EffectType.AdditionalManaScrollCost,
                        true,
                        turn.Item2,
                        (int)enemyAttack.yFactor + turn.Item2,
                        enemyAttack.xFactor
                    );
                    EventManager.AddEffect?.Invoke(effectDTO);
                    break;
                case EnemyAttackID.PoisonedFingers:
                    damage = enemyAttack.xFactor;
                    effectDTO = new EffectDTO(
                        playerPrefab.name,
                        GetEnemyAttackDescription(enemyAttack),
                        playerPrefab,
                        enemyAttack.effectsSymbols[0],
                        EffectTarget.Player,
                        true,
                        EffectType.Damage,
                        true,
                        turn.Item2,
                        (int)enemyAttack.wFactor + turn.Item2,
                        enemyAttack.zFactor
                    );
                    EventManager.AddEffect?.Invoke(effectDTO);
                    break;
                case EnemyAttackID.ScratchingAndControlling:
                    damage = enemyAttack.xFactor;
                    effectDTO = new EffectDTO(
                        playerPrefab.name,
                        GetEnemyAttackDescription(enemyAttack),
                        playerPrefab,
                        enemyAttack.effectsSymbols[0],
                        EffectTarget.Player,
                        true,
                        EffectType.ReduceMana,
                        true,
                        turn.Item2,
                        (int)enemyAttack.wFactor + turn.Item2,
                        enemyAttack.zFactor
                    );
                    EventManager.AddEffect?.Invoke(effectDTO);
                    break;
                case EnemyAttackID.JudgeEyes:
                    damage = enemyAttack.xFactor;
                    effectDTO = new EffectDTO(
                        playerPrefab.name,
                        GetEnemyAttackDescription(enemyAttack),
                        playerPrefab,
                        enemyAttack.effectsSymbols[0],
                        EffectTarget.Player,
                        true,
                        EffectType.AdditionalManaScrollCost,
                        true,
                        turn.Item2,
                        (int)enemyAttack.zFactor + turn.Item2,
                        enemyAttack.yFactor
                    );
                    EventManager.AddEffect?.Invoke(effectDTO);
                    break;
                case EnemyAttackID.SonicCrush:
                    damage = enemyAttack.xFactor;
                    effectDTO = new EffectDTO(
                        playerPrefab.name,
                        GetEnemyAttackDescription(enemyAttack),
                        playerPrefab,
                        enemyAttack.effectsSymbols[0],
                        EffectTarget.Player,
                        true,
                        EffectType.PreventPlayerHeal,
                        true,
                        turn.Item2,
                        turn.Item2 + 1,
                        0f
                    );
                    EventManager.AddEffect?.Invoke(effectDTO);
                    break;
                case EnemyAttackID.TroubledMind:
                    damage = enemyAttack.xFactor;
                    effectDTO = new EffectDTO(
                        playerPrefab.name,
                        GetEnemyAttackDescription(enemyAttack),
                        playerPrefab,
                        enemyAttack.effectsSymbols[0],
                        EffectTarget.Player,
                        true,
                        EffectType.PreventPlayerHeal,
                        true,
                        turn.Item2,
                        turn.Item2 + 1,
                        enemyAttack.wFactor
                    );
                    EventManager.AddEffect?.Invoke(effectDTO);
                    break;
            }

            return damage;
        }

        public static void ValidateAndExecuteAdvantageDisadvantageAction(
            EnemyEntity enemyEntity,
            EnemyID enemyID,
            bool isAdvantage,
            ref float damage
        )
        {
            EnemyAdvantageID eAtriggered = EnemyAdvantageID.GoblinsWill;
            EnemyDisadvantageID eDtriggered = EnemyDisadvantageID.HatefulMelodies;
            bool trigger = false;
            List<Note> notes = EffectsAndScrollsManager.Instance.playedNotes;
            switch (enemyID)
            {
                case EnemyID.OutOfTuneGoblin:
                    if (isAdvantage)
                    {
                        if (notes.All(obj => obj.GetChord(notes).Count > 1))
                        {
                            eAtriggered = EnemyAdvantageID.GoblinsWill;
                            damage -= damage * (enemyEntity.enemyData.advantages[0].xFactor / 100f);
                            trigger = true;
                        }
                    }
                    else
                    {
                        if (notes.All(obj => obj.GetChord(notes).Count < 1))
                        {
                            eDtriggered = EnemyDisadvantageID.HatefulMelodies;
                            damage +=
                                damage * (enemyEntity.enemyData.disadvantages[0].xFactor / 100f);
                            trigger = true;
                        }
                    }
                    break;
                case EnemyID.SteplessWerewolf:
                    if (isAdvantage)
                    {
                        if (notes.Any(obj => obj.note.Tempo == Tempo.Whole))
                        {
                            eAtriggered = EnemyAdvantageID.WerewolfsWill;
                            damage -= damage * (enemyEntity.enemyData.advantages[0].xFactor / 100f);
                            trigger = true;
                        }
                    }
                    else
                    {
                        if (
                            notes.All(obj => obj.GetChord(notes).Count < 1)
                            && notes.Any(obj => obj.note.NoteBehaviour == NotationBehaviour.Note)
                        )
                        {
                            eDtriggered = EnemyDisadvantageID.CantPauseHowls;
                            damage +=
                                damage * (enemyEntity.enemyData.disadvantages[0].xFactor / 100f);
                            trigger = true;
                        }
                    }
                    break;
                case EnemyID.UnshakenBones:
                    if (isAdvantage)
                    {
                        NotationExtensions.ChordsNote.TryGetValue(Chords.DMajor, out var simpleNotes);
                        int count = notes.Where(n => simpleNotes.Contains(NotationExtensions.KeyToSimpleNote(n.ToKey()))).Count();
                        if (count > 0)
                        {
                            if (notes.Any(n => n.clef.Clef == Clef.G))
                            {
                                eAtriggered = EnemyAdvantageID.BonesWill;
                                var effectDTO = new EffectDTO(
                                enemyEntity.name,
                                GetAdvantageDescription(enemyEntity.enemyData.advantages[0]),
                                enemyEntity.gameObject,
                                enemyEntity.enemyData.advantages[0].efffectIcon,
                                EffectTarget.Enemy,
                                true,
                                EffectType.EnemyMitigation,
                                false,
                                PlaythroughContainer.Instance.currentTurn.Item2,
                                PlaythroughContainer.Instance.currentTurn.Item2 + 1,
                                (int)enemyEntity.enemyData.advantages[0].xFactor * count
                            );
                                EventManager.AddEffect?.Invoke(effectDTO);
                                trigger = true;
                            }
                        }
                    }
                    else
                    {
                        NotationExtensions.ChordsNote.TryGetValue(Chords.DMajor, out var simpleNotes);
                        if (notes.All(nt => simpleNotes.Contains(NotationExtensions.KeyToSimpleNote(nt.ToKey()))))
                        {
                            eDtriggered = EnemyDisadvantageID.ShakesTooMuch;
                            damage +=
                                damage * (enemyEntity.enemyData.disadvantages[0].xFactor / 100f);
                            trigger = true;
                        }
                    }
                    break;
                case EnemyID.SoundlessShadows:
                    if (isAdvantage)
                    {
                        if (notes.Any(n => n.clef.Clef == Clef.G))
                        {
                            eAtriggered = EnemyAdvantageID.MindsWill;
                            var effectDTO = new EffectDTO(
                            enemyEntity.name,
                            GetAdvantageDescription(enemyEntity.enemyData.advantages[0]),
                            enemyEntity.gameObject,
                            enemyEntity.enemyData.advantages[0].efffectIcon,
                            EffectTarget.Enemy,
                            true,
                            EffectType.CompleteMitigation,
                            false,
                            PlaythroughContainer.Instance.currentTurn.Item2,
                            PlaythroughContainer.Instance.currentTurn.Item2 + 1,
                            (int)enemyEntity.enemyData.advantages[0].xFactor
                        );
                            EventManager.AddEffect?.Invoke(effectDTO);
                            trigger = true;
                        }
                    }
                    else
                    {
                        var keys = NotationExtensions.GetKeysFromMode(
                            Modes.Ionian,
                            NotationExtensions.ScaleTonics[Scale.MajorD]
                        );
                        if (notes.All(n => keys.Contains(n.ToKey())))
                        {
                            eDtriggered = EnemyDisadvantageID.Headaches;
                            trigger = true;
                            damage *= 100 + enemyEntity.enemyData.disadvantages[0].xFactor;
                        }
                    }
                    break;
                case EnemyID.SilencedClaws:
                    if (isAdvantage)
                    {
                        if (notes.Any(n => n.clef.Clef == Clef.G))
                        {
                            eAtriggered = EnemyAdvantageID.ClawsWill;
                            var effectDTO = new EffectDTO(
                            enemyEntity.name,
                            GetAdvantageDescription(enemyEntity.enemyData.advantages[0]),
                            enemyEntity.gameObject,
                            enemyEntity.enemyData.advantages[0].efffectIcon,
                            EffectTarget.Enemy,
                            true,
                            EffectType.EnemyMitigation,
                            false,
                            PlaythroughContainer.Instance.currentTurn.Item2,
                            PlaythroughContainer.Instance.currentTurn.Item2 + 1,
                            (int)enemyEntity.enemyData.advantages[0].xFactor
                        );
                            EventManager.AddEffect?.Invoke(effectDTO);
                            trigger = true;
                        }
                    }
                    else
                    {
                        if (notes.Any(n => n.clef.Clef == Clef.F))
                        {
                            eDtriggered = EnemyDisadvantageID.RightAtTheEyes;
                            trigger = true;
                            damage *= 100 + enemyEntity.enemyData.disadvantages[0].xFactor;
                        }
                    }
                    break;
                case EnemyID.SoundWatcher:
                    if (isAdvantage)
                    {
                        if (notes.Any(nt => nt.clef.Clef == Clef.F))
                        {
                            eAtriggered = EnemyAdvantageID.WatchersWill;
                            trigger = true;
                            damage *= 100 + enemyEntity.enemyData.disadvantages[0].xFactor;
                        }
                        if (notes.Any(nt => nt.note.Tempo.ToFloat() <= 0.25f))
                        {
                            eAtriggered = EnemyAdvantageID.HealingEyes;
                            var effectDTO = new EffectDTO(
                            enemyEntity.name,
                            GetAdvantageDescription(enemyEntity.enemyData.advantages[0]),
                            enemyEntity.gameObject,
                            enemyEntity.enemyData.advantages[0].efffectIcon,
                            EffectTarget.Enemy,
                            true,
                            EffectType.Heal,
                            false,
                            PlaythroughContainer.Instance.currentTurn.Item2,
                            PlaythroughContainer.Instance.currentTurn.Item2 + 1,
                            (int)enemyEntity.enemyData.advantages[0].xFactor
                        );
                            EventManager.AddEffect?.Invoke(effectDTO);
                            trigger = true;
                        }
                    }
                    else
                    {
                        if (notes.Any(n => n.clef.Clef == Clef.G))
                        {
                            eDtriggered = EnemyDisadvantageID.SmoothingTheClaws;
                            trigger = true;
                            damage *= 100 + enemyEntity.enemyData.disadvantages[0].xFactor;
                        }
                    }
                    break;
                case EnemyID.AlienCaptain:
                    if (isAdvantage)
                    {
                        if (!notes.Any(n => n.note.Tempo.ToFloat() > 0.25))
                        {
                            eAtriggered = EnemyAdvantageID.CaptainsWill;
                            trigger = true;
                            damage *= 100 + enemyEntity.enemyData.disadvantages[0].xFactor;
                        }
                    }
                    else
                    {
                        if (
                            notes.All(obj => obj.GetChord(notes).Count < 1)
                            && notes.Any(obj => obj.note.NoteBehaviour == NotationBehaviour.Note)
                        )
                        {
                            eDtriggered = EnemyDisadvantageID.CantPauseHowls;
                            damage +=
                                damage * (enemyEntity.enemyData.disadvantages[0].xFactor / 100f);
                            trigger = true;
                        }
                    }
                    break;
                case EnemyID.AbyssalVisitor:
                    if (isAdvantage)
                    {
                        if (PlaythroughContainer.Instance.PlayerStats.AvailableSheetBars == notes.Select(s => s.page).Distinct().Count())
                        {
                            eAtriggered = EnemyAdvantageID.PhngluiMglwNafhCthulhuRlyehWgahNaglFhtagn;
                            var effectDTO = new EffectDTO(
                            enemyEntity.name,
                            GetAdvantageDescription(enemyEntity.enemyData.advantages[0]),
                            enemyEntity.gameObject,
                            enemyEntity.enemyData.advantages[0].efffectIcon,
                            EffectTarget.Enemy,
                            false,
                            EffectType.DamageModifier,
                            true,
                            PlaythroughContainer.Instance.currentTurn.Item2,
                            (int)enemyEntity.enemyData.advantages[0].yFactor,
                            (int)enemyEntity.enemyData.advantages[0].xFactor
                        );
                            EventManager.AddEffect?.Invoke(effectDTO);
                            trigger = true;
                        }
                    }
                    else
                    {
                        if (notes.Any(n => n.GetChord(notes).Count > 1))
                        {
                            eDtriggered = EnemyDisadvantageID.ImNotMyFather;
                            damage *= 100 + enemyEntity.enemyData.disadvantages[0].xFactor;
                        }
                    }
                    break;
            }

            if (trigger)
            {
                if (isAdvantage)
                    EventManager.EnemyAdvantageTriggered?.Invoke(enemyID, eAtriggered);
                else
                    EventManager.EnemyDisadvantageTriggered?.Invoke(enemyID, eDtriggered);
            }
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

        private static string GetEnemyAttackDescription(EnemyAttack enemyAttack)
        {
            string description = LocalizationManager.Localize(
                "Enemies.Attack.Description." + (int)enemyAttack.enemyAttackID
            );

            description = description.Replace("-X", enemyAttack.xFactor.ToString());
            description = description.Replace("-Y", enemyAttack.yFactor.ToString());
            description = description.Replace("-Z", enemyAttack.zFactor.ToString());
            description = description.Replace("-W", enemyAttack.wFactor.ToString());
            description = description.Replace("-J", enemyAttack.jFactor.ToString());
            description = description.Replace("-M", enemyAttack.mFactor.ToString());
            description = description.Replace("-L", enemyAttack.lFactor.ToString());

            return description;
        }

        private static string GetAdvantageDescription(EnemyAdvantage id)
        {
            string description = LocalizationManager.Localize(
    "Enemies.Attack.Description." + (int)id.enemyAdvantageID
);

            description = description.Replace("-X", id.xFactor.ToString());
            description = description.Replace("-Y", id.yFactor.ToString());
            description = description.Replace("-Z", id.zFactor.ToString());
            return description;
        }
    }
}
