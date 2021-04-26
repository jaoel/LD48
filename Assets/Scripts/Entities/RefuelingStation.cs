using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class RefuelingStation : Interactable {

        [SerializeField]
        private Transform _toolTipPos = null;

        protected override void OnInteract() {
            base.OnInteract();

            FuelController.Instance.UpdateFuel(20.0f);
        }

        protected override void OnRelease() {
            base.OnRelease();
        }

        protected override void Update() {
            base.Update();

            if (PlayerInReach) {
                UIManager.Instance.DisplayTextPanel(_toolTipPos, "Hold [space] to refuel");
            }
        }
    }
}
