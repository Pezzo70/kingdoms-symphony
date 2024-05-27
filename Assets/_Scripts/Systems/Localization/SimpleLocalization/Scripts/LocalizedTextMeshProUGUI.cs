using System;
using TMPro;
using UnityEngine;

namespace Assets.SimpleLocalization.Scripts
{
    /// <summary>
    /// Localize text component.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedTextMeshProUGUI : MonoBehaviour
    {
        [SerializeField]
        private string _localizationKey;

        public string LocalizationKey
        {
            get => _localizationKey;
            set
            {
                _localizationKey = value;
                if (!String.IsNullOrEmpty(_localizationKey))
                    Localize();
            }
        }

        private bool _wasReplaced = false;

        void OnEnable()
        {
            LocalizationManager.OnLocalizationChanged += Localize;
        }

        void OnDestroy()
        {
            LocalizationManager.OnLocalizationChanged -= Localize;
        }

        public void Start()
        {
            if (!_wasReplaced)
                Localize();

            _wasReplaced = false;
        }

        public void Localize()
        {
            if (this != null)
                GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize(
                    _localizationKey
                );
        }

        public void Replace(Tuple<string, string>[] replacements)
        {
            string target = GetComponent<TextMeshProUGUI>().text;

            foreach (var replacement in replacements)
                target = target.Replace(replacement.Item1, replacement.Item2);

            GetComponent<TextMeshProUGUI>().text = target;
            _wasReplaced = true;
        }

        public void Clear()
        {
            GetComponent<TextMeshProUGUI>().text = "";
        }
    }
}
