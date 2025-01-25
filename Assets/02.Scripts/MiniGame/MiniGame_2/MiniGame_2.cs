using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniGame02 : MonoBehaviour
{
    public Slider timeSlider; // �ð� �����̴�
    public Slider powerSlider; // ���� ������ �����̴�
    public TextMeshProUGUI arrowDisplay; // ����Ű ǥ��
    public TextMeshProUGUI resultText; // ��� �޽��� ǥ��
    public AudioSource wrongInputSound; // Ʋ�� �Է� �� ����Ǵ� �Ҹ�
    public Transform arrowParent;
    public GameObject arrowPrefab;

    private float maxTime = 10f; // ���� �ð�
    private float currentTime; // ���� �ð�
    private float maxPower = 100f; // �ִ� ���� ������
    private float currentPower; // ���� ���� ������
    private bool isGameActive = true; // ���� ���� ����
    private string[] currentArrowKeys = new string[4]; // ���� ǥ�õ� ����Ű �迭
    private int currentInputIndex = 0; // �÷��̾ �Է� ���� ����Ű �ε���

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (!isGameActive) return;

        // �ð� �� ������ ����
        currentTime -= Time.deltaTime;
        currentPower -= Time.deltaTime * 1f; // ���� �������� õõ�� ����
        timeSlider.value = currentTime / maxTime;
        powerSlider.value = currentPower / maxPower;

        // �ð� �ʰ� �Ǵ� �������� 0�� �Ǹ� ���� ó��
        if (currentTime <= 0 || currentPower <= 0)
        {
            FailGame();
        }

        // ����Ű �Է� üũ
        CheckInput();
    }

    void StartGame()
    {
        // �ʱ�ȭ
        isGameActive = true;
        currentTime = maxTime;
        currentPower = maxPower;
        resultText.gameObject.SetActive(false);
        GenerateRandomArrowKeys(); // ���� ����Ű ����
    }

    void CheckInput()
    {
        if (!isGameActive) return;

        // ����Ű �Է� ó��
        if (Input.GetKeyDown(KeyCode.UpArrow) && currentArrowKeys[currentInputIndex] == "��" ||
            Input.GetKeyDown(KeyCode.DownArrow) && currentArrowKeys[currentInputIndex] == "��" ||
            Input.GetKeyDown(KeyCode.LeftArrow) && currentArrowKeys[currentInputIndex] == "��" ||
            Input.GetKeyDown(KeyCode.RightArrow) && currentArrowKeys[currentInputIndex] == "��")
        {
            // ���� ����Ű�� �ùٸ� ���
            Transform currentArrow = arrowParent.GetChild(currentInputIndex); // ���� ȭ��ǥ ��������
            currentArrow.GetComponentInChildren<TextMeshProUGUI>().color = Color.green; // �ʷϻ����� ����
            currentInputIndex++; // ���� ����Ű�� �̵�


            // ��� ����Ű�� ��Ȯ�� �Է��� ���
            if (currentInputIndex >= currentArrowKeys.Length)
            {
                currentInputIndex = 0;
                GenerateRandomArrowKeys(); // ���ο� ���� ����Ű ����
                powerSlider.value += 20f; // ���� ������ ȸ��
                if (currentPower > maxPower) currentPower = maxPower; // �ִ� ���� ������ ����
                //if (powerSlider.value > 1) powerSlider.value = 1; // ������ �ִ밪 ����
            }
        }
        else if (Input.anyKeyDown) // �߸��� Ű �Է� ��
        {
            PlayWrongInputSound(); // Ʋ�� �Է� �Ҹ� ���
            StartCoroutine(WrongInputFeedback()); // �߸��� �Է� �� �ǵ�� ����
            //currentInputIndex = 0; // �Է� �ʱ�ȭ
            //GenerateRandomArrowKeys(); // ���ο� ���� ����Ű ����
        }
    }

    void GenerateRandomArrowKeys()
    {
        // ���� ȭ��ǥ ����
        foreach (Transform child in arrowParent)
        {
            Destroy(child.gameObject);
        }

        // ���� ����Ű 4�� ����
        string[] arrowKeys = { "��", "��", "��", "��" };
        for (int i = 0; i < currentArrowKeys.Length; i++)
        {
            currentArrowKeys[i] = arrowKeys[Random.Range(0, arrowKeys.Length)];

            // ȭ��ǥ UI ����
            GameObject arrow = Instantiate(arrowPrefab, arrowParent);
            TextMeshProUGUI arrowText = arrow.GetComponentInChildren<TextMeshProUGUI>();
            arrowText.text = currentArrowKeys[i];
            arrowText.color = Color.white; // �⺻ ������ ���
        }

        currentInputIndex = 0; // �Է� �ε��� �ʱ�ȭ
        // ����Ű�� ȭ�鿡 ǥ��
        //arrowDisplay.text = string.Join(" ", currentArrowKeys);
    }

    IEnumerator WrongInputFeedback()
    {
        // ��� ȭ��ǥ�� ���������� �����̱�
        foreach (Transform child in arrowParent)
        {
            TextMeshProUGUI arrowText = child.GetComponentInChildren<TextMeshProUGUI>();
            arrowText.color = Color.red;
        }

        yield return new WaitForSeconds(0.5f); // 0.5�� ���

        // �ٽ� ���� ����Ű ����
        GenerateRandomArrowKeys();
    }


    void FailGame()
    {
        isGameActive = false; // ���� ����
        resultText.gameObject.SetActive(true);
        resultText.text = "Game over!"; // ���� �޽��� ǥ��
    }

    void PlayWrongInputSound()
    {
        if (wrongInputSound != null)
        {
            wrongInputSound.Play();
        }
    }
}