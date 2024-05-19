using System.Collections;
using Kingdom.Level;
using Kingdom.Player;
using TMPro;
using UnityEngine;

namespace Kingdom.UI.HUD
{
    public class PlayerMoralBar : MonoBehaviour
    {
        public RectTransform moralBarFill;
        public TextMeshProUGUI moralText;

        void Start() => UpdateMoral();

        void OnEnable()
        {
            EventManager.OnPlayerMoralChange += UpdateMoral;
        }

        void OnDisable()
        {
            EventManager.OnPlayerMoralChange -= UpdateMoral;
        }

        private void UpdateMoral()
        {
            PlayerStats playerStats = PlaythroughContainer.Instance.PlayerStats;
            float playerMoral = playerStats.CurrentMoral;
            float maxMoral = playerStats.MaxMoral;
            float barSize = moralBarFill.sizeDelta.x;

            float prcHP = Mathf.InverseLerp(0, maxMoral, playerMoral);
            float relativeX = barSize * prcHP;
            moralBarFill.anchoredPosition = new Vector2(
                -barSize + relativeX,
                moralBarFill.anchoredPosition.y
            );

            moralText.text = $"{playerMoral}/{maxMoral}";
        }
    }
}
