using Kingdom.Enums.Player;
using Kingdom.Player;
using TMPro;
using UnityEngine;

public class CharacterHandler : MonoBehaviour
{
    public CharacterID characterID;
    public TextMeshProUGUI level;
    public RectTransform xpBar;
    public TextMeshProUGUI currentXP;

    void Start()
    {
        PlayerData playerData = PlayerContainer.Instance.PlayerData;
        var characterInfo = playerData.LevelPerCharacter[characterID];
        level.text = characterInfo.level.ToString();

        float barSize = xpBar.sizeDelta.x;

        if (characterInfo.level >= 10)
        {
            currentXP.text = "MAX!";
            xpBar.anchoredPosition = new Vector2(
                xpBar.anchoredPosition.x + barSize,
                xpBar.anchoredPosition.y
            );
        }
        else
        {
            float targetXP = playerData.GetXPForTargetLevel(characterInfo.level + 1);
            currentXP.text = $"{characterInfo.currentXP}/{targetXP}";

            float prcXP = Mathf.InverseLerp(0, targetXP, characterInfo.currentXP);

            float relativeX = barSize * prcXP;

            xpBar.anchoredPosition = new Vector2(
                xpBar.anchoredPosition.x + relativeX,
                xpBar.anchoredPosition.y
            );
        }
    }
}
