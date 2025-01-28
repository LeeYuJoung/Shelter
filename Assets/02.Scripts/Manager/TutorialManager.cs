using UnityEngine;

namespace Manager
{
    public class TutorialManager : MonoBehaviour
    {
        private static TutorialManager instance;
        public static TutorialManager Instance { get { return instance; } }

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
        }
    }
}