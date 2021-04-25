using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class ProbeStation : Interactable {

        [SerializeField]
        private Probe _probe = null;

        [SerializeField]
        private FuelController _fuelController = null;

        private void Awake() {

        }

        private void Start() {

        }

        protected override void Update() {
            base.Update();
        }

        protected override void OnInteract() {
            base.OnInteract();

            if (_fuelController.Fuel > 0.0f) {
                _probe.Fire();
            }
        }

        protected override void OnRelease() {
            base.OnRelease();
        }
    }
}
