using UnityEngine;

namespace LD48 {
    public class UIBob : MonoBehaviour {
        private RectTransform rectTransform;

        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update() {
            rectTransform.anchoredPosition = new Vector2(0f, Mathf.Sin(Time.time * 4f) * 20f);
        }
    }
}
