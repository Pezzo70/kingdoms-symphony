using Assets.SimpleLocalization.Scripts;
using Kingdom.Effects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectApplied : MonoBehaviour
{
    public EffectDTO effectDTO;
    public Image icon;
    public TextMeshProUGUI description;
    public LocalizedTextMeshProUGUI expireIn;

    public void UpdateData()
    {
        expireIn.LocalizationKey = "InGame.Effect.Expire";
        expireIn.Replace(
            new System.Tuple<string, string>[]
            {
                new System.Tuple<string, string>(
                    "-X",
                    (effectDTO.EffectExpireOnTurn - effectDTO.internalCounter).ToString()
                )
            }
        );
    }

    public void SetData(EffectDTO effectDTO)
    {
        this.effectDTO = effectDTO;
        icon.sprite = effectDTO.EffectIcon;
        description.text = effectDTO.DisplayText;
        expireIn.LocalizationKey = "InGame.Effect.Expire";
        expireIn.Replace(
            new System.Tuple<string, string>[]
            {
                new System.Tuple<string, string>(
                    "-X",
                    (effectDTO.EffectExpireOnTurn - effectDTO.internalCounter).ToString()
                )
            }
        );
    }
}
