using Kingdom.Enums;
using Kingdom.Enums.Tips;
using Kingdom.Scrolls;
using UnityEngine;

namespace Kingdom.UI
{
    public class TipOrScrollContainerUI : MonoBehaviour
    {
        [Tooltip("Scroll or Tip?")]
        public UIScriptableObjectDisplayed option;

        public GameObject selectable;

        void Awake()
        {
            switch (option)
            {
                case UIScriptableObjectDisplayed.Tip:
                    foreach (var tip in TipsContainer.Instance.tips)
                        InstantiateChild(tip);
                    break;
                case UIScriptableObjectDisplayed.Scroll:
                    foreach (var scroll in ScrollsContainer.Instance.scrolls)
                        InstantiateChild(scroll);
                    break;
            }
        }

        void Start() { }

        void Update() { }

        private void InstantiateChild(ScriptableObject so)
        {
            GameObject tipInstance = Instantiate(selectable);
            TipOrScrollUI content = selectable.GetComponent<TipOrScrollUI>();
            content.displayTarget = so;
            tipInstance.transform.SetParent(this.transform);
        }
    }
}
