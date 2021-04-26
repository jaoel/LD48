using UnityEngine;

namespace LD48 {
    public class FootstepPlayer : MonoBehaviour {
        public AudioSource audioSource;
        public void Footstep() {
            audioSource.Play();
        }
    }
}
