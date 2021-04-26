using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class RefuelingStation : Interactable {

        protected override void OnInteract() {
            base.OnInteract();

            FuelController.Instance.UpdateFuel(20.0f);
        }

        protected override void OnRelease() {
            base.OnRelease();
        }

        protected override void Update() {
            base.Update();
        }
    }
}
