using Assets.SimpleLocalization;
using Assets.SimpleLocalization.Scripts;
using UnityEditor.Localization.Editor;
using UnityEngine;

public class LanguageSettings : MonoBehaviour
{
    [SerializeField]
    public Language GameLanguage = Language.Portuguese;
    void Start()
    {

    }

    void Awake()
    {
        LocalizationManager.Read();

        SetLanguage(GameLanguage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetLanguage(Language language)
    {
        switch (language)
        {
            case Language.Portuguese:
                LocalizationManager.Language = "Portuguese";
                break;
            case Language.Spanish:
                LocalizationManager.Language = "Spanish";
                break;
            case Language.English:
                LocalizationManager.Language = "English";
                break;
        }

    }
}

public enum Language
{
    Portuguese, English, Spanish
}
