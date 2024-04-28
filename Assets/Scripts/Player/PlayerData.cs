using System;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using Newtonsoft.Json;

namespace Player
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
    }
}
