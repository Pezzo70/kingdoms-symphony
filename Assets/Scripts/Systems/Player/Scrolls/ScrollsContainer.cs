using UnityEngine;

namespace Scrolls
{
    public class ScrollsContainer : MonoBehaviour
    {
        private static ScrollsContainer _instance = null;

        public Scroll[] scrolls;

        public static ScrollsContainer Instance
        {
            get => _instance;
        }

        void Start()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else if (_instance != null && _instance != this)
                Destroy(this.gameObject);
        }
    }
}
