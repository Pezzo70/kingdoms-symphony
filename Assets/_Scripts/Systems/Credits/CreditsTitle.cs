using Kingdom.Enums;
using UnityEngine;

namespace Kingdom.UI
{
    [CreateAssetMenu(
        fileName = "CreditTitle",
        menuName = "ScriptableObjects/Credits/Title",
        order = 1
    )]
    public class CreditsTitle : ScriptableObject
    {
        public UICreditTitle UICreditTitle;
        public string key;
    }
}
