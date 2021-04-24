using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class PlatformMover : Interactable {

        [SerializeField]
        private GameObject _level = null;


        private float _currentSpeed = 0.0f;

        [SerializeField]
        public float _maxSpeed = 10.0f;

        [SerializeField]
        public float _acc = 10.0f;

        private void Awake() {
            
        }

        protected override void OnInteract() {
            base.OnInteract();
        }

        protected override void OnRelease() {
            base.OnRelease();
        }

        protected override void Update() {
            base.Update();

            if (Interacting) {
                if (_acc < 0) {
                    _currentSpeed = Mathf.Max(_currentSpeed + _acc * Time.deltaTime, _maxSpeed);
                } else {
                    _currentSpeed = Mathf.Min(_currentSpeed + _acc * Time.deltaTime, _maxSpeed);
                }
            }
            else if (!Interacting && _currentSpeed != 0.0f) {

                if (_acc < 0) {
                    _currentSpeed = Mathf.Min(_currentSpeed - _acc * Time.deltaTime, 0.0f);
                }
                else {
                    _currentSpeed = Mathf.Max(_currentSpeed - _acc * Time.deltaTime, 0.0f);
                }
            }

            if (_currentSpeed != 0.0f) {
                Vector3 levelPos = _level.transform.position;
                levelPos.y += _currentSpeed * Time.deltaTime;
                _level.transform.position = levelPos;
            }
        }
    }
}
