using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace LD48 {
    public class Diamond : Interactable {
        public bool isInMiner = true;

        public static UnityEvent updated = new UnityEvent();
        protected override bool ShouldDisplayInteractMarker() {
            return isInMiner == false;
        }

        private void Start() {
            UpdateDiamondVisibility();
            updated.AddListener(UpdateDiamondVisibility);
        }

        private void OnDestroy() {
            updated.RemoveListener(UpdateDiamondVisibility);
        }

        protected override void OnInteract() {
            if (!Player.Instance.hasDiamond) {
                Player.Instance.PlayPop();
            }
            Player.Instance.hasDiamond = true;
            HandleExit(Player.Instance);

            updated?.Invoke();

            Player.Instance.ShowDiamondTooltip();
        }

        private void UpdateDiamondVisibility() {
            if (isInMiner == Player.Instance.hasDiamond) {
                if (!gameObject.activeSelf) {
                    gameObject.SetActive(true);
                }
            } else {
                if (gameObject.activeSelf) {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}