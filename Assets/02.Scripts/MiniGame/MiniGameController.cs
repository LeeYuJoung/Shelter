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
        const float errorMinTime = 15.0f;
        const float errorMaxTime = 20.0f;

        private float playTime = 20.0f;
        private int plusPoint = 35;
        private int minusPoint = 5;
        private int reward = 10;
        private int penalty = 10;

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
                    MiniGameStart();
                }
            }
        }

        // 게임 시작 : 각 miniGame Scripts에서 override해서 사용
        public void MiniGameStart()
        {
            isError = false;
            isPlaying = true;
            currentTime = 0;
            Debug.Log(":: MiniGameStart ::");
        }

        // 게임 결과
        public void MiniGameOver(bool isClear)
        {
            isPlaying = false;

            if(isClear)
            {

            }
            else
            {

            }
        }

        // 미니 게임 난이도 상승
        public void DifficultyLevelUp()
        {
            playTime -= 2.0f;
        }

        // 초기화
        public void Init()
        {

        }
    }
}