using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    //BGM 종류들
    public enum EBgm
    {
        Title,
        Intro,
        Main,
        Ending
    }

    //SFX 종류들
    public enum ESfx
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

        [SerializeField] AudioSource bgmPlayer = null;
        [SerializeField] AudioSource[] sfxPlayer = null;

        public Slider bgmSlider;
        public Slider sfxSlider;

        public float bgmVolume = 1.0f;
        public float sfxVolume = 1.0f;

        private void Awake()
        {
            if(instance != null && instance!= this)
            {
                Destroy(instance);
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
            sfxPlayer = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
            bgmSlider = GameObject.Find("Settings")?.transform.GetChild(0).transform.GetChild(0).GetComponentInChildren<Slider>();
            sfxSlider = GameObject.Find("Settings")?.transform.GetChild(0).transform.GetChild(1).GetComponentInChildren<Slider>();

            if(bgmSlider != null && sfxSlider != null)
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
            Debug.Log("Change BGM");
            bgmPlayer.volume = bgmVolume;
        }

        public void SaveBGMVolume()
        {
            Debug.Log("Save BGM");
            bgmVolume = bgmSlider.value;
        }

        public void ChangeSFXVolume()
        {

        }

        public void SaveSFXVolume()
        {
            sfxVolume = sfxSlider.value;
        }

        public void PlaySFX(AudioSource _audioSource, ESfx _soundType)
        {
            if (sfxs[(int)_soundType] != null)
            {
                _audioSource.clip = sfxs[(int)_soundType];
                if (_audioSource.gameObject.activeSelf)
                {
                    _audioSource.Play();
                }
            }
            else
            {
                Debug.Log("::: 해당 SFX 없음 :::");
            }
        }
    }
}