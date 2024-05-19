using UnityEngine;
using UnityEngine.UI;

namespace Kingdom.UI.HUD
{
    public class PlayerManaState : MonoBehaviour
    {
        public Image image;
        public Sprite manaFill;
        public Sprite manaDepleted;

        [HideInInspector]
        public bool isDepleted;

        public void UpdateManaState(bool depleted)
        {
            isDepleted = depleted;

            if (isDepleted)
                image.sprite = manaDepleted;
            else
                image.sprite = manaFill;
        }
    }
}
