using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {

    [Serializable]
    public class FuelUpgrade {
        public int Cost;
        public int MaxFuel;
    }
    public class FuelUpgrader : Interactable {

        [SerializeField]
        private List<FuelUpgrade> _upgrades = new List<FuelUpgrade>();

        private int _currentLevel = -1;

        protected override void OnInteract() {
            base.OnInteract();

            int nextLevel = _currentLevel + 1;
            if (nextLevel >= _upgrades.Count) {
                IsInteractable = false;
                return;
            }
                
            if (Player.Instance.Resources >= _upgrades[nextLevel].Cost) {
                FuelController.Instance.UpdateMax(_upgrades[nextLevel].MaxFuel);
                _currentLevel++;
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
