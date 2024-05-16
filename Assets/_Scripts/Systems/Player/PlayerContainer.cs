using Assets.SimpleLocalization.Scripts;
using Newtonsoft.Json;
using Persistence;
using Unity;
using UnityEngine;

namespace Kingdom.Player
{
    public class PlayerContainer : PersistentSingleton<PlayerContainer>
    {
        [JsonProperty]
        public PlayerData playerData;

        [JsonProperty]
        public PlayerConfig playerConfig;

        protected override void Awake()
        {
            base.Awake();

            if (ToOrFromJSON.CheckIfFileExists(PathConstants.PlayerConfigPath))
            {
                playerConfig = ToOrFromJSON.DeserializeFromJSON<PlayerConfig>(
                    PathConstants.PlayerConfigPath
                );
            }
            else
            {
                playerConfig = new PlayerConfig();
                playerConfig.CreateNewPlayerConfig();
                _ = ToOrFromJSON.SerializeToJSON(PathConstants.PlayerConfigPath, playerConfig);
            }

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

        void Start()
        {
            playerConfig.ApplyConfig();
        }

        protected override void OnApplicationQuit()
        {
            _ = ToOrFromJSON.SerializeToJSON(PathConstants.PlayerDataPath, playerData);
            _ = ToOrFromJSON.SerializeToJSON(PathConstants.PlayerConfigPath, playerConfig);

            base.OnApplicationQuit();
        }
    }
}
