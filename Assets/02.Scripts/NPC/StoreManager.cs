using UnityEngine;

namespace yjlee.store
{
    public class StoreManager : MonoBehaviour
    {
        private static StoreManager instance;
        public static StoreManager Instance { get { return instance; } }

        public GameObject buyPhanel;
        public GameObject upgradePhanel;

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(instance);
            }
        }

        public void RobotBuyAndUpgradeChange(int buttonDir)
        {
            if(buttonDir == 1)
            {

            }
            else
            {
                 
            }
        }
    }
}