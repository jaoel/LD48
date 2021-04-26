using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LD48 {

    [Serializable]
    public class CargoUpgrade {
        public int Cost;
        public int MaxCargo;
    }

    public class CargoUpgrader : Interactable {
        [SerializeField]
        private List<CargoUpgrade> _upgrades = new List<CargoUpgrade>();

        private int _currentLevel = -1;
        protected override void OnInteract() {
            base.OnInteract();

            int nextLevel = _currentLevel + 1;
            if (nextLevel >= _upgrades.Count) {
                IsInteractable = false;
                return;
            }

            if (Player.Instance.Resources >= _upgrades[nextLevel].Cost) {
                Player.Instance.MaxResources = _upgrades[nextLevel].MaxCargo;
                Player.Instance.Resources -= _upgrades[nextLevel].Cost;

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
