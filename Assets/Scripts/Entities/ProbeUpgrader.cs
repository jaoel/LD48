using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {

    [Serializable]
    class ProbeUpgrade {
        public int Cost;
        public float Speed;
    }

    public class ProbeUpgrader : Interactable {

        [SerializeField]
        List<ProbeUpgrade> _upgrades = new List<ProbeUpgrade>();

        int _currentUpgrade = -1;

        [SerializeField]
        private Transform _toolTipPos = null;

        public AudioSource audioSource;

        protected override void OnInteract() {
            base.OnInteract();

            int nextLevel = _currentUpgrade + 1;
            if (nextLevel >= _upgrades.Count) {
                IsInteractable = false;
                return;
            }

            if (Player.Instance.Resources >= _upgrades[nextLevel].Cost) {
                GameObject[] probes = GameObject.FindGameObjectsWithTag("Probe");

                foreach (GameObject go in probes) {
                    go.GetComponent<Probe>().Upgrade(_upgrades[nextLevel].Speed);
                }

                Player.Instance.Resources -= _upgrades[nextLevel].Cost;
                _currentUpgrade++;

                audioSource.Play();
            }
        }
        protected override void OnRelease() {
            base.OnRelease();
        }
        protected override void Update() {
            base.Update();

            if (PlayerInReach) {
                if (_currentUpgrade + 1 >= _upgrades.Count) {
                    UIManager.Instance.DisplayTextPanel(_toolTipPos, "Probes fully upgraded");
                } else {
                    UIManager.Instance.DisplayTextPanel(_toolTipPos,
                      $"Probe range +{_upgrades[_currentUpgrade + 1].Speed * 2.0f}\nCost {_upgrades[_currentUpgrade + 1].Cost}");
                }
            }
        }
    }
}
