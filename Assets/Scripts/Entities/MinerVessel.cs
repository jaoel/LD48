using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class MinerVessel : MonoBehaviour {
        public Transform depthObject = null;
        public List<Transform> wheelTransforms = new List<Transform>();
        public Transform drill = null;
        public float wheelRadius = 2.045f;
        public float drillSpeed = 0f;

        private float currentDrillRotation = 0f;

        private void Update() {
            float c = Mathf.PI * 2f * wheelRadius;
            foreach (var wheel in wheelTransforms) {
                wheel.localRotation = Quaternion.Euler(-depthObject.position.y * c, 0f, 0f);
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