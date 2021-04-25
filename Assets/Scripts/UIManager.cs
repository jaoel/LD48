﻿using System.Collections;
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

        public TextPanel textPanelPrefab = null;

        private List<TextPanelEntry> panelEntries = new List<TextPanelEntry>();

        private void Awake() {
            Instance = this;
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
                        UpdatePanelWorldPosition(entry.panel, entry.target);
                    }
                }
            }
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
            UpdatePanelWorldPosition(newPanel, worldTarget);
        }

        private void DestroyPanelEntry(TextPanelEntry entry) {
            Destroy(entry.panel.gameObject);
            panelEntries.Remove(entry);
        }

        private void UpdatePanelWorldPosition(TextPanel panel, Transform target) {
            Vector2 viewportPosition = camera.WorldToViewportPoint(target.position);
            Vector2 WorldObject_ScreenPosition = new Vector2(
                (viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
                (viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)
            );

            panel.rectTransform.anchoredPosition = WorldObject_ScreenPosition;
        }
    }
}