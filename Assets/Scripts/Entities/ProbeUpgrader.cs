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


        protected override void OnInteract() {
            base.OnInteract();

            int nextLevel = _currentUpgrade + 1;
            if (nextLevel >= _upgrades.Count) {
                IsInteractable = false;
                return;
            }

            if (Player.Instance.Resources >= _upgrades[nextLevel].Cost) {
                GameObject[] probes = GameObject.FindGameObjectsWithTag("Probe");

                foreach(GameObject go in probes) {
                    go.GetComponent<Probe>().Upgrade(_upgrades[nextLevel].Speed);
                }

                _currentUpgrade++;
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
