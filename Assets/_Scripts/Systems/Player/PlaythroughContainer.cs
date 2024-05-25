using System;
using System.Collections.Generic;
using System.Linq;
using Kingdom.Enemies;
using Kingdom.Enums;
using Kingdom.Enums.Enemies;
using Kingdom.Enums.Player;
using Kingdom.Enums.Scrolls;
using Kingdom.Extensions;
using Kingdom.Player;
using Kingdom.Scrolls;
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
        public CharacterID currentCharacterID;
        public int characterLevel;
        public PlayerStats PlayerStats;
        public ValueTuple<Turn, int> currentTurn;

        private Dictionary<EnemyID, int> _enemiesBanished;
        private List<EnemyID> _enemiesEncountered;
        private Dictionary<EnemyID, List<EnemyAttackID>> _enemiesAttacksExecuted;
        private Dictionary<EnemyID, List<EnemyAdvantageID>> _enemiesAdvantagesTriggered;
        private Dictionary<EnemyID, List<EnemyDisadvantageID>> _enemiesDisadvantagesTriggered;
        private Dictionary<ScrollID, int> _scrollsUsed;
        private Dictionary<ScrollID, int> _scrollsAccomplished;

        private UnityAction<EnemyID> _enemyBanishedAction;
        private UnityAction<EnemyID> _enemyEncounteredAction;
        private UnityAction<EnemyID, EnemyAttackID> _enemyAttackExecutedAction;
        private UnityAction<EnemyID, EnemyAdvantageID> _enemyAdvantageTriggeredAction;
        private UnityAction<EnemyID, EnemyDisadvantageID> _enemyDisadvantageTriggeredAction;
        private UnityAction<ScrollID> _scrollUsedAction;
        private UnityAction<ScrollID> _scrollAccomplishedAction;

        protected override void Awake()
        {
            base.Awake();

            _enemyBanishedAction = (EnemyID e) =>
            {
                CollectionsHelper.VerifyAndAddToDictionary(_enemiesBanished, e);
            };

            _enemyEncounteredAction = (EnemyID e) =>
            {
                CollectionsHelper.VerifyAndAddToList(_enemiesEncountered, e);
            };

            _enemyAttackExecutedAction = (EnemyID eID, EnemyAttackID eaID) =>
            {
                CollectionsHelper.VerifyAndAddToDictionaryList(_enemiesAttacksExecuted, eID, eaID);
            };

            _enemyAdvantageTriggeredAction = (EnemyID eID, EnemyAdvantageID eaID) =>
            {
                CollectionsHelper.VerifyAndAddToDictionaryList(
                    _enemiesAdvantagesTriggered,
                    eID,
                    eaID
                );
            };

            _enemyDisadvantageTriggeredAction = (EnemyID eID, EnemyDisadvantageID edID) =>
            {
                CollectionsHelper.VerifyAndAddToDictionaryList(
                    _enemiesDisadvantagesTriggered,
                    eID,
                    edID
                );
            };

            _scrollUsedAction = (ScrollID s) =>
            {
                CollectionsHelper.VerifyAndAddToDictionary(_scrollsUsed, s);
            };

            _scrollAccomplishedAction = (ScrollID s) =>
            {
                CollectionsHelper.VerifyAndAddToDictionary(_scrollsAccomplished, s);
            };

            EventManager.TurnChanged += (Turn turn) =>
            {
                currentTurn.Item1 = turn;
                currentTurn.Item2++;
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
            currentCharacterID = characterID;
            PlayerStats.CreateNewPlayerStats(characterID);
            _currentLevelIndex = 0;
            _currentLevelPhase = 1;
            _enemiesBanished = new Dictionary<EnemyID, int>();
            _enemiesEncountered = new List<EnemyID>();
            _enemiesAttacksExecuted = new Dictionary<EnemyID, List<EnemyAttackID>>();
            _enemiesAdvantagesTriggered = new Dictionary<EnemyID, List<EnemyAdvantageID>>();
            _enemiesDisadvantagesTriggered = new Dictionary<EnemyID, List<EnemyDisadvantageID>>();
            _scrollsUsed = new Dictionary<ScrollID, int>();
            _scrollsAccomplished = new Dictionary<ScrollID, int>();
            currentTurn = new ValueTuple<Turn, int>(Turn.PlayersTurn, 0);
        }

        public Level GetNextLevel()
        {
            Level currentLevel = _levels[_currentLevelIndex];

            if (currentLevel.endGame)
                return _levels[_currentLevelIndex];

            PlayerStats.CleanseAllEffects(currentCharacterID);

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

        public Level GetCurrentLevel => _levels[_currentLevelIndex];

        public EnemyPerLevelPhase GetCurrentLevelEnemies() =>
            _levels[_currentLevelIndex]
                .enemiesPerLevel
                .ToList()
                .Find(obj => obj.phase == _currentLevelPhase);

        public (
            int enemiesBanished,
            int enemiesXP,
            int scrollsUsed,
            int scrollsAccomplished,
            int scrollsXP,
            bool leveledUp
        ) GetEndGameInfoAndUpdatePlayerData(bool victory)
        {
            (
                int enemiesBanished,
                int enemiesXP,
                int scrollsUsed,
                int scrollsAccomplished,
                int scrollsXP,
                bool leveledUp
            ) endGameInfo = new();

            EnemiesContainer enemiesContainer = EnemiesContainer.Instance;
            ScrollsContainer scrollsContainer = ScrollsContainer.Instance;

            endGameInfo.enemiesBanished = _enemiesBanished.Aggregate(
                0,
                (total, next) => total + next.Value
            );

            endGameInfo.enemiesXP = (int)
                Mathf.CeilToInt(
                    _enemiesBanished.Aggregate(
                        0,
                        (total, next) =>
                            total
                            + (
                                enemiesContainer.enemies.First(obj => obj.enemyID == next.Key).XP
                                * next.Value
                            )
                    )
                );

            endGameInfo.scrollsUsed = _scrollsUsed.Aggregate(
                0,
                (total, next) => total + next.Value
            );

            endGameInfo.scrollsAccomplished = _scrollsAccomplished.Aggregate(
                0,
                (total, next) => total + next.Value
            );

            endGameInfo.scrollsXP = (int)
                Mathf.CeilToInt(
                    _scrollsAccomplished.Aggregate(
                        0,
                        (total, next) =>
                            total
                            + (
                                scrollsContainer.scrolls.First(obj => obj.scrollID == next.Key).XP
                                * next.Value
                            )
                    )
                );

            int totalXP = endGameInfo.enemiesXP + endGameInfo.scrollsXP;

            PlayerData playerData = PlayerContainer.Instance.PlayerData;
            int previousLevel = playerData.GetLevel(currentCharacterID);
            playerData.AddXP(totalXP, currentCharacterID);
            int currentLevel = playerData.GetLevel(currentCharacterID);

            if (currentLevel > previousLevel)
                endGameInfo.leveledUp = true;
            else
                endGameInfo.leveledUp = false;

            playerData.UpdateEnemyInfoUnlocked(_enemiesEncountered);
            playerData.UpdateEnemyAttacksUnlocked(_enemiesAttacksExecuted);
            playerData.UpdateEnemyAdvantagesUnlocked(_enemiesAdvantagesTriggered);
            playerData.UpdateEnemyDisadvantagesUnlocked(_enemiesDisadvantagesTriggered);

            EventManager.SavePlayerData?.Invoke();

            return endGameInfo;
        }
    }
}
