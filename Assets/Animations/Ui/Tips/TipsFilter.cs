using Kingdom.Enums.Tips;
using UnityEngine;

namespace Kingdom.UI
{
    public class TipsFilter : MonoBehaviour
    {
        public bool filterForEnemies = false;
        public GameObject parent;

        public void Filter()
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                GameObject child = parent.transform.GetChild(i).gameObject;
                TipOrScrollUI data = child.GetComponent<TipOrScrollUI>();
                if (data.displayTarget is Tip)
                {
                    child
                        .gameObject
                        .SetActive(
                            (data.displayTarget as Tip).isEnemy && filterForEnemies
                                || !(data.displayTarget as Tip).isEnemy && !filterForEnemies
                        );
                }
            }
        }
    }
}
