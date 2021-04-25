using System.Collections;
using UnityEngine;

namespace LD48 {
    public class TextPanel : MonoBehaviour {
        public TMPro.TextMeshProUGUI text;
        public RectTransform rectTransform;
        public CanvasGroup canvasGroup;

        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetAlpha(float alpha) {
            canvasGroup.alpha = Mathf.Clamp01(alpha);
        }
    }
}