using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class CameraController : MonoBehaviour {
        [SerializeField]
        private Transform _playerTransform = null;

        [SerializeField]
        private Vector3 _platformCenter = Vector3.zero;

        [SerializeField]
        [Range(0.0f, 1000.0f)]
        private float _lerpRate = 725.0f;

        [Range(0f, 1f)]
        public float zoomLevel = 0f;

        public new Camera camera = null;

        private Vector3 targetPos;
        private Vector3 smoothDampVelV3;
        private float positionSmoothDampVelF;
        private float zoomSmoothDampVelF;

        private void Awake() {
            targetPos = _playerTransform.position;
        }

        private void Start() {

        }

        private void Update() {
            Vector3 toPlatform = _playerTransform.position - _platformCenter;
            float horizontalDistance = new Vector2(toPlatform.x, toPlatform.z).magnitude;
            if (horizontalDistance < 10f) {
                Vector3 newTarget = _platformCenter;
                newTarget.y = _playerTransform.position.y;
                newTarget = Vector3.Lerp(_playerTransform.position, newTarget, 0.85f);
                targetPos = Vector3.SmoothDamp(targetPos, newTarget, ref smoothDampVelV3, 1f, 10f, Time.deltaTime);
                zoomLevel = Mathf.SmoothDamp(zoomLevel, 0.5f, ref zoomSmoothDampVelF, 1f, 8f, Time.deltaTime);
            }
            else if (toPlatform.magnitude > 40f) {
                targetPos = Vector3.SmoothDamp(targetPos, _playerTransform.position, ref smoothDampVelV3, 0.25f, 20f, Time.deltaTime);
                zoomLevel = Mathf.SmoothDamp(zoomLevel, 0.85f, ref zoomSmoothDampVelF, 1f, 8f, Time.deltaTime);
            } else {
                targetPos = Vector3.SmoothDamp(targetPos, toPlatform / 2f, ref smoothDampVelV3, 2f, 10f, Time.deltaTime);
                zoomLevel = Mathf.SmoothDamp(zoomLevel, 0.25f, ref zoomSmoothDampVelF, 1f, 0.1f, Time.deltaTime);
            }

            Vector3 position = transform.position;
            position.x = Mathf.SmoothDamp(position.x, targetPos.x, ref positionSmoothDampVelF, 1f, 50f, Time.deltaTime);
            transform.position = position;

            transform.rotation = Quaternion.LookRotation(targetPos - transform.position, Vector3.up);

            float distanceToTarget = Vector3.Distance(targetPos, transform.position);
            camera.transform.localPosition = Vector3.forward * Mathf.Lerp(0f, distanceToTarget - 1f, zoomLevel);
        }
    }
}

