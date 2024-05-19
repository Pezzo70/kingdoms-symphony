using System.Collections.Generic;
using Kingdom.Level;
using Kingdom.Player;
using UnityEngine;

namespace Kingdom.UI.HUD
{
    public class PlayerMana : MonoBehaviour
    {
        public GameObject playerManaContainer;
        public GameObject playerManaPrefab;
        private List<PlayerManaState> _manaInstantiated;
        private List<PlayerManaState> _additionalManaInstantiaded;

        void Awake()
        {
            _manaInstantiated = new List<PlayerManaState>();
            _additionalManaInstantiaded = new List<PlayerManaState>();
        }

        void Start() => UpdateMana();

        void OnEnable()
        {
            EventManager.OnPlayerManaChange += UpdateMana;
        }

        void OnDisable()
        {
            EventManager.OnPlayerManaChange -= UpdateMana;
        }

        private void UpdateMana()
        {
            PlayerStats playerStats = PlaythroughContainer.Instance.PlayerStats;
            if (_manaInstantiated.Count == 0)
            {
                for (int i = 0; i < playerStats.ManaPerTurn; i++)
                {
                    GameObject manaInstance = Instantiate(
                        playerManaPrefab,
                        playerManaContainer.transform
                    );
                    _manaInstantiated.Add(manaInstance.GetComponent<PlayerManaState>());
                    _manaInstantiated[i].UpdateManaState(false);
                }
            }
            else
            {
                if (playerStats.ManaPerTurn > playerStats.MaxMana)
                {
                    for (int j = 0; j < _additionalManaInstantiaded.Count; j++)
                        Destroy(_additionalManaInstantiaded[j].gameObject);

                    _additionalManaInstantiaded.Clear();

                    for (int i = 0; i < playerStats.ManaPerTurn - playerStats.MaxMana; i++)
                    {
                        GameObject manaInstance = Instantiate(
                            playerManaPrefab,
                            playerManaContainer.transform
                        );
                        _additionalManaInstantiaded.Add(
                            manaInstance.GetComponent<PlayerManaState>()
                        );
                        _additionalManaInstantiaded[i].UpdateManaState(false);
                    }
                }
                else
                {
                    if (_additionalManaInstantiaded.Count > 0)
                    {
                        for (int i = 0; i < _additionalManaInstantiaded.Count; i++)
                        {
                            Destroy(_additionalManaInstantiaded[i].gameObject);
                        }
                        _additionalManaInstantiaded.Clear();
                    }
                }

                List<PlayerManaState> allMana = new List<PlayerManaState>();
                allMana.AddRange(_manaInstantiated);
                allMana.AddRange(_additionalManaInstantiaded);

                for (int i = 0; i < allMana.Count; i++)
                {
                    if (i < playerStats.CurrentMana)
                        allMana[i].UpdateManaState(false);
                    else
                        allMana[i].UpdateManaState(true);
                }
            }
        }
    }
}
