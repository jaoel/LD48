using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public abstract class Interactable : MonoBehaviour {
        public bool PlayerInReach { get; private set; } = false;
        public bool IsInteractable { get; protected set; } = true;
        public bool Interacting { get; private set; } = false;

        public Transform interactMarkerPosition = null;

        protected Player interactingPlayer = null;

        protected virtual bool ShouldDisplayInteractMarker() {
            return true;
        }

        [SerializeField]
        private bool _holdToInteract = false;
        protected virtual void OnInteract() {
            Interacting = true;
        }
        protected virtual void OnRelease() {
            Interacting = false;
        }
        protected virtual void Update() {
            if (PlayerInReach && IsInteractable) {
                if ((_holdToInteract && Input.GetKey(KeyCode.Space)) || (!_holdToInteract && Input.GetKeyDown(KeyCode.Space))) {
                    OnInteract();
                }
                else if (Interacting) {
                    OnRelease();
                }
            }

            if (interactMarkerPosition != null && PlayerInReach && ShouldDisplayInteractMarker()) {
                UIManager.Instance.DisplayInteractMarker(interactMarkerPosition);
            }
        }

        protected void OnTriggerEnter(Collider other) {
            if (other.gameObject.TryGetComponent(out Player player)) {
                interactingPlayer = player;
                PlayerInReach = true;
            }
        }

        protected void OnTriggerExit(Collider other) {
            if (other.gameObject.TryGetComponent(out Player player)) {
                HandleExit(player);
            }
        }

        protected void HandleExit(Player player) {
            interactingPlayer = null;
            PlayerInReach = false;

            if (Interacting) {
                OnRelease();
            }
        }
    }
}

