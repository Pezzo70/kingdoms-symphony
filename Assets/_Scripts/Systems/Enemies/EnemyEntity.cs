using System;
using System.Collections.Generic;
using Kingdom.Constants;
using Kingdom.Level;
using TMPro;
using UnityEngine;

namespace Kingdom.Enemies
{
    public class EnemyEntity : MonoBehaviour
    {
        public Enemy enemyData;
        public RectTransform moralBarFill;
        public TextMeshProUGUI moralText;
        public GameObject enemyManaContainer;
        public GameObject enemyManaPrefab;
        private float _maxMoral;
        private float _currentMoral;
        private int _manaPerTurn;
        private int _currentMana;
        private List<GameObject> _manasInstantiated;

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
            _currentMana = _manaPerTurn;
            _manasInstantiated = new List<GameObject>();
        }

        void Start()
        {
            UpdateMoral();
            UpdateMana();
        }

        void Update() { }

        void OnEnable()
        {
            EventManager.EnemiesDamaged += HandleEnemyDamaged;
            EventManager.EnemiesRegainMana += HandleEnemyRegainMana;
        }

        void OnDisable()
        {
            EventManager.EnemiesDamaged -= HandleEnemyDamaged;
            EventManager.EnemiesRegainMana -= HandleEnemyRegainMana;
        }

        private void HandleEnemyDamaged(float damage)
        {
            //@TODO
            /*To-Do Advantages and Ongoing Effects*/
            _currentMoral -= damage;
            _currentMoral = Mathf.Clamp(_currentMoral, 0f, _maxMoral);
            UpdateMoral();
        }

        private void Heal(float quantity)
        {
            //@TODO
            /*To-Do Advantages and Ongoing Effects*/
            _currentMoral += quantity;
            _currentMoral = Mathf.Clamp(_currentMoral, 0f, _maxMoral);
        }

        private void Attack()
        {
            //@TODO
            /*To-Do Choose Attack*/
            /*To-Do Advantages and Ongoing Effects*/
        }

        private void HandleEnemyRegainMana()
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
    }
}
