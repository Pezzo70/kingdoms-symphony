using System;
using Newtonsoft.Json;
using Persistence;

namespace Player
{
    public class PlayerContainer : PersistentSingleton<PlayerContainer>
    {
        [JsonProperty]
        public PlayerData playerData;

        protected override void Awake()
        {
            base.Awake();

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
