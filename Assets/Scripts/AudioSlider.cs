using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace LD48 {
    public class AudioSlider : MonoBehaviour {
        static string prefsPrefic = "VVOL";
        public string parameterName;
        public Slider slider = null;
        public AudioMixer audioMixer = null;

        private string PrefsKey => prefsPrefic + parameterName;

        public void Init() {
            if (PlayerPrefs.HasKey(PrefsKey)) {
                float savedVolume = PlayerPrefs.GetFloat(PrefsKey);
                slider.value = savedVolume;
                SetVolume(savedVolume);
            } else {
                float volume = 0.8f;
                PlayerPrefs.SetFloat(PrefsKey, volume);
                slider.value = volume;
                SetVolume(volume);
            }
        }

        private void Start() {
            Init();
        }

        public void SetVolume(float volume) {
            float mappedVolume = Mathf.Log10(volume) * 20f;
            audioMixer.SetFloat(parameterName, Mathf.Clamp(mappedVolume, -80f, 0f));
            PlayerPrefs.SetFloat(PrefsKey, volume);
        }
    }
}