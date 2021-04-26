using UnityEngine;

namespace LD48 {
    public class RefuelingStation : Interactable {

        [SerializeField]
        private Transform _toolTipPos = null;

        public AudioSource audioSource;

        private float audioAmount = 0f;

        private void Start() {
            audioSource.Play();
            audioSource.Pause();
        }

        protected override void OnInteract() {
            base.OnInteract();

            FuelController.Instance.UpdateFuel(20.0f);
        }

        protected override void OnRelease() {
            base.OnRelease();
        }

        protected override void Update() {
            base.Update();

            if (Interacting) {
                audioAmount = Mathf.MoveTowards(audioAmount, 1f, Time.deltaTime * 2f);
            } else {
                audioAmount = Mathf.MoveTowards(audioAmount, 0f, Time.deltaTime * 2f);
            }

            if (audioAmount == 0f) {
                audioSource.Pause();
            } else {
                audioSource.UnPause();
                audioSource.pitch = Mathf.Lerp(0.4f, 1f, audioAmount);
            }

            if (PlayerInReach) {
                UIManager.Instance.DisplayTextPanel(_toolTipPos, "Hold [space] to refuel");
            }
        }
    }
}
