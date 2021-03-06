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
        public GameObject hiddenParts = null;
        public TerrainManager terrainManager = null;
        public Teleporter teleporter = null;
        public Teleporter surfaceTeleporter = null;
        public Transform tooHotPosition;

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

            Vector3 centerToPlayer = Player.Instance.transform.position - transform.position;
            centerToPlayer.y = 0f;

            if (centerToPlayer.magnitude <= 10f) {
                if (hiddenParts.activeSelf) {
                    hiddenParts.SetActive(false);
                }
            } else {
                if (!hiddenParts.activeSelf) {
                    hiddenParts.SetActive(true);
                }
            }

            if (terrainManager.TryGetCurrentSegment(out var currentSegment) && currentSegment.tileSegment.teleporter != null && Mathf.Abs(currentSegment.tileSegment.teleporter.transform.position.y - transform.position.y) < 2f) {
                if (teleporter.targetTeleporter == null) {
                    teleporter.SetTargetTeleporter(currentSegment.tileSegment.teleporter);
                }
            } else {
                if (teleporter.targetTeleporter != null) {
                    teleporter.SetTargetTeleporter(null);
                }
            }

            if (Level.Instance.tooHot) {
                UIManager.Instance.DisplayTextPanel(tooHotPosition, "It's too hot!");
            }
        }
    }
}