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

        private Camera _camera = null;

        private void Awake() {
            _camera = GetComponent<Camera>();
        }

        private void Start() {

        }

        private void Update() {
            Vector3 targetPos = (_playerTransform.position - _platformCenter) / 2.0f;
            Quaternion targetLookRotation = Quaternion.LookRotation(targetPos - _camera.transform.position, Vector3.up);
            _camera.transform.rotation = Quaternion.Slerp(_camera.transform.rotation, targetLookRotation, Mathf.Pow(2, -_lerpRate * Time.deltaTime));
        }
    }
}

