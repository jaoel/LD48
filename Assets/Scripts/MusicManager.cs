using UnityEngine;

namespace LD48 {
    public class MusicManager : MonoBehaviour {
        public static MusicManager Instance { get; private set; } = null;

        public AudioSource musicSource = null;
        public AudioClip musicClip = null;
        public AudioClip ambientClip = null;

        private AudioClip queuedClip = null;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            musicSource.clip = ambientClip;
            musicSource.volume = 0f;
            musicSource.Play();
        }

        public void QueueClip(AudioClip nextClip) {
            if (musicSource.clip != nextClip) {
                queuedClip = nextClip;
            }
        }

        private void Update() {
            if (queuedClip != null) {
                musicSource.volume = Mathf.MoveTowards(musicSource.volume, 0f, Time.deltaTime * 1f);

                if (musicSource.volume == 0f) {
                    musicSource.clip = queuedClip;
                    musicSource.Play();
                    queuedClip = null;
                }
            } else if (musicSource.clip != null && musicSource.volume < 1f) {
                musicSource.volume = Mathf.MoveTowards(musicSource.volume, 1f, Time.deltaTime * 1f);
            }
        }
    }
}
