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
        [SerializeField] AudioClip[] sfxs;

        [SerializeField] AudioSource bgmPlayer = null;
        [SerializeField] AudioSource[] sfxPlayer = null;

        public Slider bgmSlider;
        public Slider sfxSlider;

        public float bgmVolume = 1.0f;
        public float sfxVolume = 1.0f;

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(instance);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

            Init();
            //PlayerPrefs.SetFloat("bgm", 1.0f);
            //PlayerPrefs.SetFloat("sfx", 1.0f);
        }

        private void Init()
        {
            sfxPlayer = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
            bgmSlider = GameObject.Find("Settings")?.transform.GetChild(0).transform.GetChild(0).GetComponentInChildren<Slider>();
            sfxSlider = GameObject.Find("Settings")?.transform.GetChild(0).transform.GetChild(1).GetComponentInChildren<Slider>();
        }

        public void PlayBGM(int _sceneNumber)
        {
            bgmPlayer.clip = bgms[_sceneNumber];
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