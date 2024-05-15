using Newtonsoft.Json;
using Persistence;

namespace Kingdom.Player
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
    }
}
