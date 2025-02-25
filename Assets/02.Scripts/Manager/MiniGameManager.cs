using MiniGame;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Manager
{
    public class MiniGameManager : MonoBehaviour
    {
        private static MiniGameManager instance;
        public static MiniGameManager Instance { get { return instance; } }

        public GameObject[] errorGameObjects;
        public int possibleIndex = 2;

        [SerializeField] private float currentTime = 0;
        [SerializeField] private float errorTime = 0;
        const float errorMinTime = 10.0f;
        const float errorMaxTime = 15.0f;

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(instance);
            }
            else
            {
                instance = this;
            }

            Init();
        }

        private void Update()
        {
            if (GameManager.Instance.isGameOver)
                return;

            OnError();
            OnClick();
        }

        private void Init()
        {
            errorTime = Random.Range(errorMinTime, errorMaxTime);
        }

        // 에러 발생
        public void OnError()
        {
            currentTime += Time.deltaTime;

            if (currentTime >= errorTime)
            {
                int i = 0;
                currentTime = 0;
                errorTime = Random.Range(errorMinTime, errorMaxTime);

                while (i <= 100)
                {
                    i++;
                    int errorIndex = Random.Range(0, possibleIndex);
                    MiniGameController miniGame = errorGameObjects[errorIndex].GetComponentInParent<MiniGameController>();

                    if (miniGame == null)
                        return;

                    if (!miniGame.isError && !miniGame.isPlaying)
                    {
                        miniGame.isError = true;
                        miniGame.audioSource.Play();
                        errorGameObjects[errorIndex].SetActive(true);
                        return;
                    }
                }
            }
        }

        // 에러 클릭
        public void OnClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0.0f);

                    if (hit.collider != null && hit.collider.CompareTag("Error"))
                    {
                        MiniGameController miniGame = hit.collider.GetComponentInParent<MiniGameController>();

                        if (miniGame != null)
                        {
                            miniGame.GameStart();
                        }
                    }
                }
            }
        }

        // 전체 종료
        public void AllMiniGameStop()
        {
            for(int i = 0; i < possibleIndex; i++)
            {
                MiniGameController miniGame = errorGameObjects[i].GetComponentInParent<MiniGameController>();

                if (miniGame != null)
                {
                    miniGame.ForcingGameOver();
                }
            }

            for(int i = 0; i < errorGameObjects.Length; i++)
            {
                errorGameObjects[i].SetActive(false);
            }
        }
    }
}
