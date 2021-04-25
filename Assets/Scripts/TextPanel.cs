using System.Collections;
using UnityEngine;

namespace LD48 {
    public class TextPanel : MonoBehaviour {
        public TMPro.TextMeshProUGUI text;
        public RectTransform rectTransform;
        public CanvasGroup canvasGroup;
        public AudioSource audioSource;

        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetAlpha(float alpha) {
            if (canvasGroup.alpha == 0f && alpha > 0f) {
                audioSource.Play();
            }
            canvasGroup.alpha = Mathf.Clamp01(alpha);
        }
    }
}