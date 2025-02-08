using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;

namespace MiniGame
{
    public class MiniGameController : MonoBehaviour
    {
        [SerializeField] private GameObject errorGameObject;

        [SerializeField] private float currentTime = 0;
        [SerializeField] private float errorTime = 0;
        [SerializeField] private float beepTime = 10.0f;
        const float errorMinTime = 5.0f;
        const float errorMaxTime = 10.0f;

        public static float playTime = 20.0f;
        public static int plusPoint = 35;
        public static int minusPoint = 5;
        public static int reward = 5;
        public static int penalty = 5;

        private bool isError = false;
        private bool isPlaying = false;

        private void Start()
        {
            errorGameObject = transform.GetChild(0).gameObject;
            errorTime = Random.Range(errorMinTime, errorMaxTime);
        }

        private void Update()
        {
            if (!isError && !isPlaying)
            {
                OnError();
            }
            else if(isError && !isPlaying)
            {
                OnBeep();
                OnClick();
            }
        }

        // 에러 발생
        public void OnError()
        {
            currentTime += Time.deltaTime;

            if (currentTime >= errorTime)
            {
                isError = true;
                currentTime = 0;
                errorTime = Random.Range(errorMinTime, errorMaxTime);
                errorGameObject.SetActive(true);
            }
        }

        // 경보 발생
        public void OnBeep()
        {
            currentTime += Time.deltaTime;

            if (currentTime >= beepTime)
            {
                currentTime = 0;
                errorGameObject.SetActive(false);

                // 경보 발생 시간 내 미니 게임 실행 안하면 패널티 발생
                Debug.Log(":: Error 해결 실패 패널티 적용 ::");
            }
        }

        // 에러 클릭
        public void OnClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0.0f);

                if(hit.collider != null && hit.collider.CompareTag("Error"))
                {
                    GameStart();
                }
            }
        }

        // 게임 시작
        protected virtual void GameStart()
        {
            Debug.Log("GameStart");
            isError = false;
            isPlaying = true;
            currentTime = 0;
            errorGameObject.SetActive(false);
        }

        // 게임 종료
        protected virtual void ClearGame()
        {
            isPlaying = false;
        }

        // 게임 수치 상승
        protected virtual void GameLevelUp()
        {
            playTime -= 2.0f;
        }

        // 초기화
        public void Init()
        {

        }
    }
}