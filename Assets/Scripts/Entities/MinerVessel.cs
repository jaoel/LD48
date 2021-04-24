using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class MinerVessel : MonoBehaviour {
        public List<Transform> wheelTransforms = new List<Transform>();
        public Transform drill = null;
        public float wheelSpeed = 0f;
        public float drillSpeed = 0f;

        private float currentWheelRotation = 0f;
        private float currentDrillRotation = 0f;

        private void Update() {
            if (wheelSpeed != 0f) {
                currentWheelRotation += wheelSpeed * Time.deltaTime;
                if (currentWheelRotation > 360f) {
                    currentWheelRotation -= 360f;
                }

                foreach (var wheel in wheelTransforms) {
                    wheel.localRotation = Quaternion.Euler(currentWheelRotation, 0f, 0f);
                }
            }

            if (drillSpeed != 0f) {
                currentDrillRotation += drillSpeed * Time.deltaTime;
                if (currentDrillRotation > 360f) {
                    currentDrillRotation -= 360f;
                }

                drill.localRotation = Quaternion.Euler(0f, currentDrillRotation, 0f);
            }
        }
    }
}