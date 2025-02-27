using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    //SFX 종류들
    public enum SFXType
    {
        SFX_BUTTON,
        SFX_ENDING,
        SFX_TOKEN,
        SFX_BOTTLE,
        SFX_OPENDOOR,
        SFX_MissionClear
    }

    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance;
        public static AudioManager Instance { get { return instance; } }

        //audio clip 담을 수 있는 배열
        [SerializeField] AudioClip[] bgms;
        [SerializeField] AudioClip[] endingBgms;
        [SerializeField] AudioClip[] sfxs;

        private GameObject[] sfxGameObjects;
        private GameObject[] audioGameObjects;
        [SerializeField] AudioSource bgmPlayer;
        [SerializeField] AudioSource[] sfxPlayer;
        [SerializeField] AudioSource[] audioPlayer;

        public Slider bgmSlider;
        public Slider sfxSlider;

        public float bgmVolume = 1.0f;
        public float sfxVolume = 1.0f;
        public int resolutionIndex = 1;

        private void Awake()
        {
            if(instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

            Init();
        }

        public void Init()
        {
            sfxGameObjects = GameObject.FindGameObjectsWithTag("SFX");
            audioGameObjects = GameObject.FindGameObjectsWithTag("BGM");
            bgmSlider = GameObject.Find("Settings")?.transform.GetChild(0).transform.GetChild(1).GetComponentInChildren<Slider>();
            sfxSlider = GameObject.Find("Settings")?.transform.GetChild(0).transform.GetChild(1).transform.GetChild(1).GetComponentInChildren<Slider>();

            sfxPlayer = new AudioSource[sfxGameObjects.Length];
            for (int i = 0; i  < sfxGameObjects.Length; i++)
            {
                sfxPlayer[i] = sfxGameObjects[i].GetComponent<AudioSource>();
            }

            audioPlayer = new AudioSource[audioGameObjects.Length];
            for(int i = 0; i < audioGameObjects.Length; i++)
            {
                audioPlayer[i] = audioGameObjects[i].GetComponent<AudioSource>();
            }

            if (bgmSlider != null && sfxSlider != null)
            {
                bgmSlider.onValueChanged.AddListener(delegate { ChangeBGMVolume(); });
                bgmSlider.onValueChanged.AddListener(delegate { SaveBGMVolume(); });

                sfxSlider.onValueChanged.AddListener(delegate { ChangeSFXVolume(); });
                sfxSlider.onValueChanged.AddListener(delegate { SaveSFXVolume(); });
            }
        }

        public void PlayBGM(int _sceneNumber)
        {
            bgmPlayer.clip = bgms[_sceneNumber];
            bgmPlayer.Play();
        }

        public void EndingBGM(int _endingNumber)
        {
            bgmPlayer.clip = endingBgms[_endingNumber];
            bgmPlayer.Play();
        }

        public void StopBGM()
        {
            bgmPlayer.Stop();
        }

        public void ChangeBGMVolume()
        {
            bgmPlayer.volume = bgmVolume;
        }

        public void SaveBGMVolume()
        {
            bgmVolume = bgmSlider.value;
        }

        public void ChangeSFXVolume()
        {
            for(int i = 0; i < sfxPlayer.Length; i++)
            {
                sfxPlayer[i].volume = sfxVolume;
            }

            for(int i = 0; i < audioPlayer.Length; i++)
            {
                audioPlayer[i].volume = sfxVolume;
            }

            for(int i = 0; i < GameManager.Instance.collectorRobots.Count; i++)
            {
                GameManager.Instance.collectorRobots[i].GetComponent<AudioSource>().volume = sfxVolume;
            }

            for (int i = 0; i < GameManager.Instance.sweeperRobots.Count; i++)
            {
                GameManager.Instance.sweeperRobots[i].GetComponent<AudioSource>().volume = sfxVolume;
            }
        }

        public void SaveSFXVolume()
        {
            sfxVolume = sfxSlider.value;
        }

        public void PlaySFX(int soundType)
        {
            for(int i = 0; i < sfxPlayer.Length; i++)
            {
                if (!sfxPlayer[i].isPlaying)
                {
                    sfxPlayer[i].clip = sfxs[soundType];
                    sfxPlayer[i].Play();
                    return;
                }
            }
        }
    }
}