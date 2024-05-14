using Kingdom.Enums;
using UnityEngine;

namespace Kingdom.UI
{
    [CreateAssetMenu(
        fileName = "CreditPersona",
        menuName = "ScriptableObjects/Credits/Persona",
        order = 2
    )]
    public class CreditsPersona : ScriptableObject
    {
        public UICreditTitle[] role;
        public string personaName;
    }
}
