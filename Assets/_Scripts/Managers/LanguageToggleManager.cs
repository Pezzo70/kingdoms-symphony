using Assets.SimpleLocalization.Scripts;
using Kingdom.Player;
using UnityEngine;
using UnityEngine.UI;

public class LanguageControl : MonoBehaviour
{
    ToggleGroup LanguageGroup;

    void Start()
    {
        LanguageGroup = GetComponent<ToggleGroup>();
        LocalizationManager.Read();

        var togglesInGroup = GetComponentsInChildren<Toggle>();
        for (int i = 0; i < togglesInGroup.Length; i++)
        {
            int toggleID = i;
            var toggle = togglesInGroup[i];

            switch (toggleID)
            {
                case 0:
                    toggle.isOn =
                        PlayerContainer.Instance.PlayerConfig.LanguagePreferred == "Portuguese";
                    break;
                case 1:
                    toggle.isOn =
                        PlayerContainer.Instance.PlayerConfig.LanguagePreferred == "English";
                    break;
            }

            toggle
                .onValueChanged
                .AddListener(
                    delegate
                    {
                        OnLanguageSelected(toggle, toggleID);
                    }
                );
        }
    }

    void OnLanguageSelected(Toggle toggle, int toggleID)
    {
        if (toggle.isOn)
        {
            switch (toggleID)
            {
                case 0:
                    LocalizationManager.Language = "Portuguese";
                    break;
                case 1:
                    LocalizationManager.Language = "English";
                    break;
            }

            PlayerContainer.Instance.PlayerConfig.SetLanguage(LocalizationManager.Language);
        }
    }
}
