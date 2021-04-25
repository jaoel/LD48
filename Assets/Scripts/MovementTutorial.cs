using UnityEngine;

namespace LD48 {
    public class MovementTutorial : MonoBehaviour {
        private static bool hasSeenTutorial = false;

        private float beginShowTime = 0f;

        private void Start() {
            beginShowTime = Time.time + 5f;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent(out Player _)) {
                hasSeenTutorial = true;
            }
        }

        private void Update() {
            if (Time.time > beginShowTime && !hasSeenTutorial) {
                UIManager.Instance.DisplayTextPanel(Player.Instance.tutorialTransform, "Move around:\nW A S D");
            }
        }
    }
}