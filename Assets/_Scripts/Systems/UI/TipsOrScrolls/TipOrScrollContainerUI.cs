using System.Linq;
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

        void Start()
        {
            switch (option)
            {
                case UIScriptableObjectDisplayed.Tip:
                    foreach (var tip in TipsContainer.Instance.tips)
                        InstantiateChild(tip);
                    break;
                case UIScriptableObjectDisplayed.Scroll:
                    var scrolls = ScrollsContainer
                        .Instance
                        .scrolls
                        .ToList()
                        .OrderBy(obj => obj.levelRequired);
                    foreach (var scroll in scrolls)
                        InstantiateChild(scroll);
                    break;
            }
        }

        private void InstantiateChild(ScriptableObject so)
        {
            GameObject tipInstance = Instantiate(selectable, this.transform);
            TipOrScrollUI content = tipInstance.GetComponent<TipOrScrollUI>();
            content.displayTarget = so;
        }
    }
}
