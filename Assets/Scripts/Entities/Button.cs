using UnityEngine;

namespace LD48 {
    public class Button : MonoBehaviour {
        public Transform buttonTransform;
        public Transform upPosition;
        public Transform downPosition;

        public bool Pressed { get; set; } = false;

        private void Update() {
            if (Pressed) {
                buttonTransform.localPosition = Vector3.MoveTowards(buttonTransform.localPosition, downPosition.localPosition, Time.deltaTime * 5f);
            } else {
                buttonTransform.localPosition = Vector3.MoveTowards(buttonTransform.localPosition, upPosition.localPosition, Time.deltaTime * 5f);
            }
        }
    }
}