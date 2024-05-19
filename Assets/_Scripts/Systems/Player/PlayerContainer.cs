using Newtonsoft.Json;
using Persistence;

namespace Kingdom.Player
{
    public class PlayerContainer : PersistentSingleton<PlayerContainer>
    {
        [JsonProperty]
        public PlayerData PlayerData;

        [JsonProperty]
        public PlayerConfig PlayerConfig;

        protected override void Awake()
        {
            base.Awake();
            LoadPlayerData();
            LoadConfig();
        }

        void Start()
        {
            PlayerConfig.ApplyConfig();
        }

        void OnEnable()
        {
            EventManager.SavePlayerData += SavePlayerData;
            EventManager.SavePlayerConfig += SaveConfig;
        }

        void OnDisable()
        {
            EventManager.SavePlayerData -= SavePlayerData;
            EventManager.SavePlayerConfig -= SaveConfig;
        }

        private void SaveConfig() =>
            _ = ToOrFromJSON.SerializeToJSON(PathConstants.PlayerDataPath, PlayerData);

        private void LoadConfig()
        {
            if (ToOrFromJSON.CheckIfFileExists(PathConstants.PlayerDataPath))
            {
                PlayerData = ToOrFromJSON.DeserializeFromJSON<PlayerData>(
                    PathConstants.PlayerDataPath
                );
            }
            else
            {
                PlayerData = new PlayerData();
                PlayerData.CreateNewPlayerData();
                SaveConfig();
            }
        }

        private void SavePlayerData() =>
            ToOrFromJSON.SerializeToJSON(PathConstants.PlayerConfigPath, PlayerConfig);

        private void LoadPlayerData()
        {
            if (ToOrFromJSON.CheckIfFileExists(PathConstants.PlayerConfigPath))
            {
                PlayerConfig = ToOrFromJSON.DeserializeFromJSON<PlayerConfig>(
                    PathConstants.PlayerConfigPath
                );
            }
            else
            {
                PlayerConfig = new PlayerConfig();
                PlayerConfig.CreateNewPlayerConfig();
                SavePlayerData();
            }
        }

        protected override void OnApplicationQuit()
        {
            if (PlayerData is not null || PlayerConfig is not null)
            {
                _ = ToOrFromJSON.SerializeToJSON(PathConstants.PlayerDataPath, PlayerData);
                _ = ToOrFromJSON.SerializeToJSON(PathConstants.PlayerConfigPath, PlayerConfig);
            }
            base.OnApplicationQuit();
        }
    }
}
