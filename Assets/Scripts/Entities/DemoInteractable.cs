using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class DemoInteractable : Interactable {
        protected override void OnInteract() {
            Debug.Log("HELLO I MADE AN INTERACT AYY LMAOOOOOOO");
        }

        private void Awake() {

        }

        private void Start() {

        }
    }
}
