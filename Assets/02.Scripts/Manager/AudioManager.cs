using UnityEngine;
using UnityEngine.UI;
using EnumTypes;

namespace Manager
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance;
        public static AudioManager Instnce { get { return instance; } }

        [SerializeField] AudioClip[] bgms;
        [SerializeField] AudioClip[] sfxs;

        public AudioSource bgmPlayer = null;
        public AudioSource[] sfxPlayer = null;

        public Slider bgmSlider;
        public Slider sfxSlider;

        public float bgmVolume;
        public float sfxVolume;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(instance);
            }
            else
            {
                instance = this;
            }

            DontDestroyOnLoad(this);
            bgmPlayer.volume = PlayerPrefs.GetFloat("bgm_Volume");
        }

        // bgm 재생
        public void PlayBGM(int _sceneNumber)
        {
            switch (_sceneNumber)
            {
                case 0:
                    bgmPlayer.clip = bgms[0];
                    bgmPlayer.Play();
                    break;
                case 1:
                    bgmPlayer.clip = bgms[1];
                    bgmPlayer.Play();
                    break;
                case 2:
                    bgmPlayer.clip = bgms[1];
                    bgmPlayer.Play();
                    break;
            }
        }

        // bgm 재생 정지
        public void StopBGM()
        {
            bgmPlayer.Stop();
        }

        // bgm volume 수치 변경
        public void ChangeBGMVolume()
        {
            bgmPlayer.volume = bgmVolume;
        }

        // bgm volume 수치 저장
        public void SaveBGMVolume()
        {
            bgmVolume = bgmSlider.value;
        }

        // sfx 재생
        public void PlaySFX(AudioSource _audioSource, SoundType _soundType)
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

        // sfx 재생 정지
        public void StopSFX(AudioSource _audioSource, SoundType _soundType)
        {
            if (_audioSource.clip == sfxs[(int)_soundType])
            {
                _audioSource.Stop();
            }
            else
            {
                Debug.Log("::: 해당 SFX 없음 :::");
            }
        }

        // sfx volume 수치 변경
        public void ChangeSFXVolume()
        {
            for (int i = 0; i < sfxPlayer.Length; i++)
            {
                sfxPlayer[i].volume = sfxVolume;
            }
        }

        // sfx volume 수치 저장
        public void SaveSFXVolume()
        {
            sfxVolume = sfxSlider.value;
        }
    }
}