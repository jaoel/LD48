using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public abstract class Interactable : MonoBehaviour {
        public bool PlayerInReach { get; private set; } = false;
        public bool IsInteractable { get; protected set; } = true;

        protected abstract void OnInteract();

        protected virtual void Update() {
            if (PlayerInReach && IsInteractable && Input.GetKeyDown(KeyCode.Space)) {
                OnInteract();
            }
        }

        protected void OnTriggerEnter(Collider other) {
            PlayerInReach = true;
        }

        protected void OnTriggerExit(Collider other) {
            PlayerInReach = true;
        }
    }
}

