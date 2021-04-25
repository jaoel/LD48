using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class RefuelingStation : Interactable {

        [SerializeField]
        private FuelController _fuelController = null;

        protected override void OnInteract() {
            base.OnInteract();

            if (_fuelController != null) {
                _fuelController.Refuel(1.0f);
            }
        }

        protected override void OnRelease() {
            base.OnRelease();
        }

        protected override void Update() {
            base.Update();
        }
    }
}
