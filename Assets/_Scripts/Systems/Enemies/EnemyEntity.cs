using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kingdom.Audio;
using Kingdom.Constants;
using Kingdom.Effects;
using Kingdom.Enums.Enemies;
using Kingdom.Enums.FX;
using Kingdom.Level;
using TMPro;
using UnityEngine;

namespace Kingdom.Enemies
{
    public class EnemyEntity : MonoBehaviour
    {
        public Enemy enemyData;
        public GameObject enemyHUD;
        public GameObject enemyActiveIndicator;
        public SpriteRenderer enemySprite;
        public SpriteRenderer shadowSprite;
        public Animator enemyAnimator;
        public Animator pftEffectAnimator;
        public RectTransform moralBarFill;
        public TextMeshProUGUI moralText;
        public GameObject enemyManaContainer;
        public GameObject enemyManaPrefab;

        private float _maxMoral;
        private float _currentMoral;
        private int _manaPerTurn;
        private int _currentMana;

        private List<GameObject> _manasInstantiated;
        private bool _isDead;
        private bool _isOnTurn;
        private bool _isAttacking;
        private bool _endCurrentAttackAnimation;

        public bool IsDead
        {
            get => _isDead;
        }

        void Awake()
        {
            _maxMoral =
                enemyData.moral
                + enemyData.moral
                    * (
                        PlaythroughContainer.Instance.characterLevel
                        * EnemyConstants.ENEMY_CHARACTER_LEVEL_FACTOR
                    );
            _currentMoral = _maxMoral;
            _manaPerTurn = enemyData.manaPerTurn;
            _currentMana = 0;
            _manasInstantiated = new List<GameObject>();
            _isDead = false;
            _isOnTurn = false;
            _isAttacking = true;
            enemyActiveIndicator.SetActive(false);
            EventManager.EnemyEncountered?.Invoke(enemyData.enemyID);
        }

        void Start()
        {
            UpdateMoral();
            UpdateMana();
        }

        public void ExecuteTurn()
        {
            _isOnTurn = true;
            enemyActiveIndicator.SetActive(true);
            Action endTurnAction = () =>
            {
                _isOnTurn = false;
                enemyActiveIndicator.SetActive(false);
                EventManager.NextEnemy?.Invoke();
            };

            HandleTurnBasedEffects();

            if (_isDead)
            {
                endTurnAction?.Invoke();
                return;
            }

            StartCoroutine(
                AttackRoutine(() =>
                {
                    endTurnAction?.Invoke();
                })
            );
        }

        public void HandleEndCurrentAttackAnimation()
        {
            if (_isOnTurn)
                _endCurrentAttackAnimation = true;
        }

        public void ReduceMoral(float damage, float massiveDamage)
        {
            float damageUpdated = damage;

            if (
                EffectsAndScrollsManager
                    .Instance
                    .onGoingEffects
                    .Find(
                        obj =>
                            obj.EffectType == EffectType.CompleteMitigation
                            && obj.Target == this.gameObject
                    ) != null
            )
            {
                damageUpdated = 0f;
            }
            else
            {
                EffectsAndScrollsManager
                    .Instance
                    .onGoingEffects
                    .Where(
                        obj =>
                            obj.EffectType == EffectType.EnemyMitigation
                            && obj.Target == this.gameObject
                    )
                    .ToList()
                    .ForEach(effect =>
                    {
                        TheoryHandler.ValidateAndExecuteEffectAction(effect, ref damage);
                    });
            }

            TheoryHandler.ValidateAndExecuteAdvantageDisadvantageAction(
                this,
                this.enemyData.enemyID,
                false,
                ref damage
            );

            TheoryHandler.ValidateAndExecuteAdvantageDisadvantageAction(
                this,
                this.enemyData.enemyID,
                true,
                ref damage
            );

            damageUpdated += massiveDamage;

            enemyActiveIndicator.SetActive(true);
            StartCoroutine(TakeDamageRoutine(damageUpdated));
        }

        public void Heal(float quantity)
        {
            if (
                EffectsAndScrollsManager
                    .Instance
                    .onGoingEffects
                    .Find(
                        obj =>
                            obj.EffectType == EffectType.PreventEnemyHeal
                            && obj.Target == this.gameObject
                    ) != null
            )
            {
                return;
            }

            _currentMoral += quantity;
            _currentMoral = Mathf.Clamp(_currentMoral, 0f, _maxMoral);
        }

        private (EnemyAttackID, bool) SelectAttack()
        {
            EnemyAttackID[] stunnedAttacks = EffectsAndScrollsManager
                .Instance
                .onGoingEffects
                .Where(obj => obj.Target == this.gameObject && obj.EffectType == EffectType.Stun)
                .Select(obj => (EnemyAttackID)(int)obj.Modifier)
                .ToArray();

            Tuple<EnemyAttackID, float>[] chances = enemyData
                .attacks
                .Where(
                    obj =>
                        obj.manaRequired <= _currentMana
                        && !stunnedAttacks.Contains(obj.enemyAttackID)
                )
                .Select(
                    obj =>
                        new Tuple<EnemyAttackID, float>(obj.enemyAttackID, obj.probability / 100f)
                )
                .OrderByDescending(obj => obj.Item2)
                .ToArray();

            if (chances.Length == 0)
                return (enemyData.attacks[0].enemyAttackID, false);

            if (chances.Length == 1)
                return (chances[0].Item1, true);

            List<Tuple<EnemyAttackID, Tuple<float, float>>> chancesRange =
                new List<Tuple<EnemyAttackID, Tuple<float, float>>>();

            for (int i = 0; i < chances.Length; i++)
            {
                float minFactor = i == 0 ? 0 : chancesRange[i - 1].Item2.Item2;
                float maxFactor = minFactor + chances[i].Item2;
                chancesRange.Add(
                    new(chances[i].Item1, new Tuple<float, float>(minFactor, maxFactor))
                );
            }

            float randomFactor = UnityEngine.Random.Range(0f, 1f);

            return (
                chancesRange
                    .First(obj => randomFactor > obj.Item2.Item1 && randomFactor <= obj.Item2.Item2)
                    .Item1,
                true
            );
        }

        private void Attack(EnemyAttackID attackID)
        {
            EnemyAttack attack = enemyData.attacks.First(obj => obj.enemyAttackID == attackID);
            float damage = TheoryHandler.SpawnEnemyAttackEffectsAndGetDamage(attack);
            EffectsAndScrollsManager
                .Instance
                .onGoingEffects
                .Where(
                    obj =>
                        obj.EffectType == EffectType.DamageModifier && obj.Target == this.gameObject
                )
                .ToList()
                .ForEach(effect =>
                {
                    TheoryHandler.ValidateAndExecuteEffectAction(effect, ref damage);
                });

            PlaythroughContainer.Instance.PlayerStats.ReduceMoral(damage);
            HandleEnemySpendMana(attack.manaRequired);
            EventManager.EnemyAttackExecuted?.Invoke(enemyData.enemyID, attackID);
        }

        private void HandleTurnBasedEffects()
        {
            float _ = 0f;
            EffectsAndScrollsManager
                .Instance
                .onGoingEffects
                .Where(obj => obj.TriggerOnTurnStart && obj.Target == this.gameObject)
                .ToList()
                .ForEach(effect =>
                {
                    if (!_isDead)
                    {
                        TheoryHandler.ValidateAndExecuteEffectAction(effect, ref _);
                    }
                });
        }

        private void HandleEnemySpendMana(int value)
        {
            if (_currentMana - value < 0)
                return;

            _currentMana -= value;
            UpdateMana();
        }

        public void RegainMana()
        {
            _currentMana += _manaPerTurn;
            UpdateMana();
        }

        private void UpdateMana()
        {
            for (int i = 0; i < _manasInstantiated.Count; i++)
                Destroy(_manasInstantiated[i].gameObject);

            _manasInstantiated.Clear();

            for (int i = 0; i < _currentMana; i++)
            {
                GameObject manaInstantiated = Instantiate(
                    enemyManaPrefab,
                    enemyManaContainer.transform
                );
                _manasInstantiated.Add(manaInstantiated);
            }
        }

        private void UpdateMoral()
        {
            float barSize = moralBarFill.sizeDelta.x;
            float prcHP = Mathf.InverseLerp(0, _maxMoral, _currentMoral);
            float relativeX = barSize * prcHP;
            moralBarFill.anchoredPosition = new Vector2(
                -barSize + relativeX,
                moralBarFill.anchoredPosition.y
            );

            moralText.text = $"{_currentMoral}/{_maxMoral}";
        }

        private IEnumerator AttackRoutine(Action callbackPosAttack)
        {
            _isAttacking = true;
            while (_isAttacking)
            {
                (EnemyAttackID enemyAttackID, bool willAttack) = SelectAttack();

                if (willAttack == false)
                {
                    _isAttacking = false;
                    break;
                }

                enemyAnimator.Play("Attack");

                while (!_endCurrentAttackAnimation)
                    yield return new WaitForSeconds(0.05f);

                Attack(enemyAttackID);
                AudioSystem.Instance.Play(this.enemyData.enemyID, Enums.ActorAudioTypes.Attack);
                enemyAnimator.Play("Idle");
                _isAttacking = enemyData.attacks.Any(attack => attack.manaRequired <= _currentMana);
                _endCurrentAttackAnimation = false;
                yield return new WaitForSeconds(2.5f);
            }

            callbackPosAttack?.Invoke();
        }

        private IEnumerator TakeDamageRoutine(float damage)
        {
            Color redTransparent = new Color(255, 0, 0, 0.5f);

            enemyActiveIndicator.SetActive(true);

            enemyAnimator.Play("Damage");
            AudioSystem.Instance.Play(this.enemyData.enemyID, Enums.ActorAudioTypes.Damage);
            yield return new WaitForSeconds(1f);
            enemyAnimator.Play("Idle");

            _currentMoral -= damage;
            _currentMoral = Mathf.Clamp(_currentMoral, 0f, _maxMoral);
            UpdateMoral();

            if (_currentMoral == 0f)
            {
                _isDead = true;
                pftEffectAnimator.Play("Pft");
                AudioSystem.Instance.Play(FXID.Pft, FXState.Start);

                enemyHUD.SetActive(false);
                enemySprite.enabled = false;
                shadowSprite.enabled = false;
                EventManager.EnemyBanished?.Invoke(enemyData.enemyID);
            }

            yield return new WaitForSeconds(1f);

            enemyActiveIndicator.SetActive(false);

            EventManager.NextEnemyTakesDamage?.Invoke();
        }
    }
}
