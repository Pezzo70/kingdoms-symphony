using System.Collections.Generic;
using System.Linq;
using Kingdom.Enemies;
using Kingdom.Enums.Player;
using Kingdom.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Kingdom.Level
{
    public class PlaythroughContainer : PersistentSingleton<PlaythroughContainer>
    {
        [SerializeField]
        private Level[] _levels;
        private int _currentLevelIndex;
        private int _currentLevelPhase;
        private CharacterID _currentCharacterID;
        public PlayerStats PlayerStats;

        private Dictionary<Enemy, int> _enemiesBanished;
        private List<Enemy> _enemiesEncountered;
        private List<EnemyAttack> _enemiesAttacksExecuted;
        private List<EnemyAdvantage> _enemiesAdvantagesTriggered;
        private List<EnemyDisadvantage> _enemiesDisadvantagesTriggered;
        private Dictionary<Scroll, int> _scrollsUsed;
        private Dictionary<Scroll, int> _scrollsAccomplished;

        private UnityAction<Enemy> _enemyBanishedAction;
        private UnityAction<Enemy> _enemyEncounteredAction;
        private UnityAction<EnemyAttack> _enemyAttackExecutedAction;
        private UnityAction<EnemyAdvantage> _enemyAdvantageTriggeredAction;
        private UnityAction<EnemyDisadvantage> _enemyDisadvantageTriggeredAction;
        private UnityAction<Scroll> _scrollUsedAction;
        private UnityAction<Scroll> _scrollAccomplishedAction;

        protected override void Awake()
        {
            base.Awake();

            _enemyBanishedAction = (Enemy e) =>
            {
                VerifyAndAddToDictionary(_enemiesBanished, e);
            };

            _enemyEncounteredAction = (Enemy e) =>
            {
                VerifyAndAddToList(_enemiesEncountered, e);
            };

            _enemyAttackExecutedAction = (EnemyAttack eA) =>
            {
                VerifyAndAddToList(_enemiesAttacksExecuted, eA);
            };

            _enemyAdvantageTriggeredAction = (EnemyAdvantage eA) =>
            {
                VerifyAndAddToList(_enemiesAdvantagesTriggered, eA);
            };

            _enemyDisadvantageTriggeredAction = (EnemyDisadvantage eD) =>
            {
                VerifyAndAddToList(_enemiesDisadvantagesTriggered, eD);
            };

            _scrollUsedAction = (Scroll s) =>
            {
                VerifyAndAddToDictionary(_scrollsUsed, s);
            };

            _scrollAccomplishedAction = (Scroll s) =>
            {
                VerifyAndAddToDictionary(_scrollsAccomplished, s);
            };
        }

        void Start()
        {
            PlayerStats = new PlayerStats();
        }

        void OnEnable()
        {
            EventManager.EnemyBanished += _enemyBanishedAction;
            EventManager.EnemyEncountered += _enemyEncounteredAction;
            EventManager.EnemyAttackExecuted += _enemyAttackExecutedAction;
            EventManager.EnemyAdvantageTriggered += _enemyAdvantageTriggeredAction;
            EventManager.EnemyDisadvantageTriggered += _enemyDisadvantageTriggeredAction;
            EventManager.ScrollUsed += _scrollUsedAction;
            EventManager.ScrollAccomplished += _scrollAccomplishedAction;
        }

        void OnDisable()
        {
            EventManager.EnemyBanished -= _enemyBanishedAction;
            EventManager.EnemyEncountered -= _enemyEncounteredAction;
            EventManager.EnemyAttackExecuted -= _enemyAttackExecutedAction;
            EventManager.EnemyAdvantageTriggered -= _enemyAdvantageTriggeredAction;
            EventManager.EnemyDisadvantageTriggered -= _enemyDisadvantageTriggeredAction;
            EventManager.ScrollUsed -= _scrollUsedAction;
            EventManager.ScrollAccomplished -= _scrollAccomplishedAction;
        }

        public void CreateNewPlaythrough(CharacterID characterID)
        {
            _currentCharacterID = characterID;
            PlayerStats.CreateNewPlayerStats(characterID);
            _currentLevelIndex = 0;
            _currentLevelPhase = 1;
            _enemiesBanished = new Dictionary<Enemy, int>();
            _enemiesEncountered = new List<Enemy>();
            _enemiesAttacksExecuted = new List<EnemyAttack>();
            _enemiesAdvantagesTriggered = new List<EnemyAdvantage>();
            _enemiesDisadvantagesTriggered = new List<EnemyDisadvantage>();
            _scrollsUsed = new Dictionary<Scroll, int>();
            _scrollsAccomplished = new Dictionary<Scroll, int>();
        }

        public Level GetNextLevel()
        {
            Level currentLevel = _levels[_currentLevelIndex];

            if (currentLevel.endGame)
                return _levels[_currentLevelIndex];

            if (_currentLevelPhase == currentLevel.numberOfPhases)
            {
                _currentLevelIndex++;
                _currentLevelPhase = 1;
                return _levels[_currentLevelIndex];
            }
            else
            {
                _currentLevelPhase++;
                return _levels[_currentLevelIndex];
            }
        }

        public EnemyPerLevelPhase GetCurrentLevelEnemies() =>
            _levels[_currentLevelIndex]
                .enemiesPerLevel
                .ToList()
                .Find(obj => obj.phase == _currentLevelPhase);

        private void VerifyAndAddToDictionary<T>(Dictionary<T, int> dictionary, T target)
        {
            if (dictionary.ContainsKey(target))
                dictionary[target]++;
            else
                dictionary.Add(target, 1);
        }

        private void VerifyAndAddToList<T>(List<T> list, T target)
        {
            if (list.Contains(target))
                return;
            else
                list.Add(target);
        }
    }
}
