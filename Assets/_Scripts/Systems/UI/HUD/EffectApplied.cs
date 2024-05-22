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
    public GameObject pointPrefab;
    private GameObject _pointInstance;

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

    public void SpawnPointOnTarget()
    {
        if (_pointInstance == null)
        {
            _pointInstance = Instantiate(pointPrefab, effectDTO.Target.transform.parent);
            _pointInstance.transform.position = new Vector3(
                effectDTO.Target.transform.position.x,
                effectDTO.Target.transform.position.y - 2.5f,
                effectDTO.Target.transform.position.z
            );
        }
        else
        {
            _pointInstance.gameObject.SetActive(true);
        }
    }

    public void HidePointOnTarget()
    {
        _pointInstance.SetActive(false);
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
