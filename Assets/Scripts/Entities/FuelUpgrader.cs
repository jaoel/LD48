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

        public AudioSource audioSource;

        [SerializeField]
        private Transform _toolTipPos = null;
        protected override void OnInteract() {
            base.OnInteract();

            int nextLevel = _currentLevel + 1;
            if (nextLevel >= _upgrades.Count) {
                IsInteractable = false;
                return;
            }
                
            if (Player.Instance.Resources >= _upgrades[nextLevel].Cost) {
                FuelController.Instance.UpdateMax(_upgrades[nextLevel].MaxFuel);
                Player.Instance.Resources -= _upgrades[nextLevel].Cost;

                _currentLevel++;

                audioSource.Play();
            }
        }

        protected override void OnRelease() {
            base.OnRelease();
        }
        protected override void Update() {
            base.Update();

            if (PlayerInReach) {

                if (_currentLevel + 1 >= _upgrades.Count) {
                    UIManager.Instance.DisplayTextPanel(_toolTipPos, "Fuel fully upgraded");
                } else {
                    UIManager.Instance.DisplayTextPanel(_toolTipPos,
                      $"Max fuel +{_upgrades[_currentLevel + 1].MaxFuel - FuelController.Instance._maxFuel}\nCost {_upgrades[_currentLevel + 1].Cost}");
                }
            }
        }
    }
}
