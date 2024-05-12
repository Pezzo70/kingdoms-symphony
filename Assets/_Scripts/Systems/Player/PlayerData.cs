using System;
using System.Collections.Generic;
using System.Linq;
using Kingdom.Enemies;
using Kingdom.Enums.Player;
using Newtonsoft.Json;

namespace Kingdom.Player
{
    public class PlayerData
    {
        [JsonProperty]
        private Dictionary<CharacterID, (int level, int currentXP)> _levelPerCharacter;

        [JsonProperty]
        private Dictionary<EnemyID, bool> _enemyInfoUnlocked;

        [JsonProperty]
        private Dictionary<EnemyID, List<EnemyAttackID>> _enemyAttacksUnlocked;

        [JsonProperty]
        private Dictionary<EnemyID, List<EnemyAdvantageID>> _enemyAdvantagesUnlocked;

        [JsonProperty]
        private Dictionary<EnemyID, List<EnemyDisadvantageID>> _enemyDisadvangatesUnlocked;


        public PlayerData() { }

        public void CreateNewPlayerData()
        {
            IEnumerable<CharacterID> chars = Enum.GetValues(typeof(CharacterID))
                .Cast<CharacterID>();

            _levelPerCharacter = new Dictionary<CharacterID, (int level, int currentXP)>();
            foreach (CharacterID c in chars)
                _levelPerCharacter.Add(c, (1, 0));

            IEnumerable<EnemyID> enemies = Enum.GetValues(typeof(EnemyID)).Cast<EnemyID>();

            _enemyInfoUnlocked = new Dictionary<EnemyID, bool>();
            _enemyAttacksUnlocked = new Dictionary<EnemyID, List<EnemyAttackID>>();
            _enemyAdvantagesUnlocked = new Dictionary<EnemyID, List<EnemyAdvantageID>>();
            _enemyDisadvangatesUnlocked = new Dictionary<EnemyID, List<EnemyDisadvantageID>>();

            foreach (EnemyID e in enemies)
            {
                _enemyInfoUnlocked.Add(e, false);
                _enemyAttacksUnlocked.Add(e, new List<EnemyAttackID>());
                _enemyAdvantagesUnlocked.Add(e, new List<EnemyAdvantageID>());
                _enemyDisadvangatesUnlocked.Add(e, new List<EnemyDisadvantageID>());
            }
        }

        public void AddXP(int xp, CharacterID id)
        {
            if(xp == 0)
                return;

            var xpToNext = GetXPForTargetLevel(xp + 1);

            var player = this._levelPerCharacter[id];

            player.currentXP += xpToNext;
            while(player.currentXP < GetXPForTargetLevel(xp + 1))
            {
                player.currentXP -= xpToNext;
                player.level++;
            }
        }

        public int GetLevel(CharacterID id) => this._levelPerCharacter.GetValueOrDefault(id).level;

        public int GetSheetPages(CharacterID id) => 2 + GetLevel(id);

        public int GetMoral(CharacterID id) => 4 + GetLevel(id);
        public int GetMana(CharacterID id) => 4 + GetLevel(id);

        private int GetXPForTargetLevel(int targetLevel)
        {
            if (targetLevel == 10)
                return 0;

            float total = 0;
            float baseExperiencePerLevel = 522;

            for (int i = 1; i <= targetLevel; i++)
                total += (float)Math.Floor(i + baseExperiencePerLevel * Math.Pow(2, i / 7.0));

            return (int)Math.Floor(total / 4);
        }
        
    }
}
