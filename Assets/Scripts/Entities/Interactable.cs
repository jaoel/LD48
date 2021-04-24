using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public abstract class Interactable : MonoBehaviour {
        public bool PlayerInReach { get; private set; } = false;
        public bool IsInteractable { get; protected set; } = true;

        [SerializeField]
        private bool _holdToInteract = false;

        protected abstract void OnInteract();

        protected virtual void Update() {
            if (PlayerInReach && IsInteractable) {

                if ((_holdToInteract && Input.GetKey(KeyCode.Space)) || (!_holdToInteract && Input.GetKeyDown(KeyCode.Space))) {
                    OnInteract();
                }
            }
        }

        protected void OnTriggerEnter(Collider other) {
            PlayerInReach = true;
        }

        protected void OnTriggerExit(Collider other) {
            PlayerInReach = false;
        }
    }
}

