using System;
using Newtonsoft.Json;
using Persistence;
using UnityEngine;

namespace Player
{
    public class PlayerContainer : MonoBehaviour
    {
        [JsonIgnore]
        private static PlayerContainer _instance = null;

        [JsonIgnore]
        public static PlayerContainer Instance
        {
            get => _instance;
        }

        [JsonProperty]
        public PlayerData playerData;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else if (_instance != null && _instance != this)
                Destroy(this.gameObject);

            if (ToOrFromJSON.CheckIfFileExists(PathConstants.PlayerDataPath))
            {
                playerData = ToOrFromJSON.DeserializeFromJSON<PlayerData>(
                    PathConstants.PlayerDataPath
                );
            }
            else
            {
                playerData = new PlayerData();
                playerData.CreateNewPlayerData();
                _ = ToOrFromJSON.SerializeToJSON(PathConstants.PlayerDataPath, playerData);
            }
        }

        public int GetXPForTargetLevel(int targetLevel, int maxLevel = 10)
        {
            if (targetLevel == maxLevel)
                return 0;

            float total = 0;
            float baseExperiencePerLevel = 522;

            for (int i = 1; i <= targetLevel; i++)
                total += (float)Math.Floor(i + baseExperiencePerLevel * Math.Pow(2, i / 7.0));

            return (int)Math.Floor(total / 4);
        }
    }
}
