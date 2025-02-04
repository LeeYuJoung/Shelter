using UnityEngine;

namespace Manager
{
    public class StatusManager : MonoBehaviour
    {
        private static StatusManager instance;
        public static StatusManager Instance { get { return instance; } }

        public Status status;

        private int hullRestorationRatePrice;
        private int motorRestorationRatePrice;
        private int engineRestorationRatePrice;
        private int radarRestorationRatePrice;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }

            Init();
        }

        private void Init()
        {
            status = GetComponent<Status>();
        }

        // Status UI에서 수리 버튼 클릭 시 실행
        public void Reapir(string part)
        {

        }
    }
}
