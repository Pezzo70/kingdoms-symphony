using Newtonsoft.Json;
using Persistence;

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
            LoadPlayerData();
            LoadConfig();
        }

        void Start()
        {
            playerConfig.ApplyConfig();
        }

        void OnEnable()
        {
            EventManager.Save += SavePlayerData;
            EventManager.Save += SaveConfig;
        }

        void OnDisable()
        {
            EventManager.Save -= SavePlayerData;
            EventManager.Save -= SaveConfig;
        }

        private void SaveConfig() =>
            _ = ToOrFromJSON.SerializeToJSON(PathConstants.PlayerDataPath, playerData);

        private void LoadConfig()
        {
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
                SaveConfig();
            }
        }

        private void SavePlayerData() =>
            ToOrFromJSON.SerializeToJSON(PathConstants.PlayerConfigPath, playerConfig);

        private void LoadPlayerData()
        {
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
                SavePlayerData();
            }
        }

        protected override void OnApplicationQuit()
        {
            if (playerData is not null || playerConfig is not null)
            {
                _ = ToOrFromJSON.SerializeToJSON(PathConstants.PlayerDataPath, playerData);
                _ = ToOrFromJSON.SerializeToJSON(PathConstants.PlayerConfigPath, playerConfig);
            }
            base.OnApplicationQuit();
        }
    }
}
