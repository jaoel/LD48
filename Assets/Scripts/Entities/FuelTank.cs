using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class FuelTank : MonoBehaviour {
        public GameObject fuelCylinder = null;
        public ParticleSystem bubbles = null;

        private bool didSetFuel = false;

        public void SetFuel(float fuel) {
            fuel = Mathf.Clamp01(fuel);
            if (fuel == 0f) {
                if (fuelCylinder.activeSelf) {
                    fuelCylinder.SetActive(false);
                }
            } else {
                if (!fuelCylinder.activeSelf) {
                    fuelCylinder.SetActive(true);
                }
                fuelCylinder.transform.localScale = new Vector3(1f, fuel, 1f);
                didSetFuel = true;
                var main = bubbles.main;
                main.startLifetime = fuel * 5f;
            }
        }

        private void LateUpdate() {
            if (didSetFuel) {
                if (!bubbles.isPlaying) {
                    bubbles.Play();
                }
            } else {
                if (!bubbles.isStopped) {
                    bubbles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                }
            }
            didSetFuel = false;
        }
    }
}
