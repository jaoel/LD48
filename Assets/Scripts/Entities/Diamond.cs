using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace LD48 {
    public class Diamond : Interactable {
        public bool isInMiner = true;

        private static UnityEvent pickedUp = new UnityEvent();

        protected override bool ShouldDisplayInteractMarker() {
            return isInMiner == false;
        }

        private void Start() {
            UpdateDiamondVisibility();
            pickedUp.AddListener(UpdateDiamondVisibility);
        }

        private void OnDestroy() {
            pickedUp.RemoveListener(UpdateDiamondVisibility);
        }

        protected override void OnInteract() {
            Player.Instance.hasDiamond = true;
            pickedUp?.Invoke();
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