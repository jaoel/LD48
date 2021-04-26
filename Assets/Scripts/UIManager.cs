using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class UIManager : MonoBehaviour {
        public static UIManager Instance { get; private set; } = null;

        private class TextPanelEntry {
            public Transform target;
            public TextPanel panel;
            public float refreshTime;
        }

        private const float fadeTime = 0.25f;

        public Canvas worldCanvas = null;
        public new Camera camera = null;
        public RectTransform canvasRect = null;
        public RectTransform worldCanvasWrapper = null;
        public GameObject pauseMenu = null;
        public GameObject winScreen = null;

        public RectTransform interactMarker = null;

        public TMPro.TextMeshProUGUI fuelText = null;
        public TMPro.TextMeshProUGUI resourcesText = null;

        public TextPanel textPanelPrefab = null;

        private List<TextPanelEntry> panelEntries = new List<TextPanelEntry>();

        private float lastInteractMarkerFrame = 0;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            foreach (AudioSlider audioSlider in pauseMenu.GetComponentsInChildren<AudioSlider>()) {
                audioSlider.Init();
            }
            pauseMenu.SetActive(false);
            winScreen.SetActive(false);
        }

        private void Update() {
            for (int i = panelEntries.Count - 1; i >= 0; i--) {
                TextPanelEntry entry = panelEntries[i];
                if (entry.target == null) {
                    panelEntries.RemoveAt(i);
                } else {
                    float refreshTime = Time.time - entry.refreshTime;
                    if (refreshTime > 2f) {
                        DestroyPanelEntry(entry);
                    } else {
                        entry.panel.SetAlpha(1f - refreshTime / fadeTime);
                        UpdatePanelWorldPosition(entry.panel.rectTransform, entry.target);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (Time.timeScale == 1f) {
                    Time.timeScale = 0f;
                    pauseMenu.SetActive(true);
                } else {
                    Time.timeScale = 1f;
                    pauseMenu.SetActive(false);
                    winScreen.SetActive(false);
                }
            }

            if (lastInteractMarkerFrame < Time.frameCount - 1) {
                if (interactMarker.gameObject.activeSelf) {
                    interactMarker.gameObject.SetActive(false);
                }
            } else {
                if (!interactMarker.gameObject.activeSelf) {
                    interactMarker.gameObject.SetActive(true);
                }
            }
        }

        public void DisplayInteractMarker(Transform target) {
            UpdatePanelWorldPosition(interactMarker, target);
            lastInteractMarkerFrame = Time.frameCount;
        }

        public void DisplayTextPanel(Transform worldTarget, string text) {
            if (worldTarget == null) {
                return;
            }

            foreach (TextPanelEntry entry in panelEntries) {
                if (entry.target == worldTarget) {
                    entry.panel.text.SetText(text);
                    entry.refreshTime = Time.time;
                    return;
                }
            }

            TextPanel newPanel = Instantiate(textPanelPrefab, worldCanvasWrapper.transform);
            newPanel.text.SetText(text);
            panelEntries.Add(new TextPanelEntry() {
                target = worldTarget,
                panel = newPanel,
                refreshTime = Time.time,
            });
            UpdatePanelWorldPosition(newPanel.rectTransform, worldTarget);
        }

        private void DestroyPanelEntry(TextPanelEntry entry) {
            Destroy(entry.panel.gameObject);
            panelEntries.Remove(entry);
        }

        private void UpdatePanelWorldPosition(RectTransform rectTransform, Transform target) {
            Vector2 viewportPosition = camera.WorldToViewportPoint(target.position);
            Vector2 WorldObject_ScreenPosition = new Vector2(
                (viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
                (viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)
            );

            rectTransform.anchoredPosition = WorldObject_ScreenPosition;
        }
    }
}