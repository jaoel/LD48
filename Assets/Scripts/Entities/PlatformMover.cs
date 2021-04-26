using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class PlatformMover : Interactable {
        private float _currentSpeed = 0.0f;

        [SerializeField]
        public float _maxSpeed = 10.0f;

        [SerializeField]
        public float _acc = 10.0f;

        [SerializeField]
        public float _deceleration = 10.0f;

        [SerializeField]
        private MinerVessel _miner = null;

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

                if (_deceleration < 0) {
                    _currentSpeed = Mathf.Min(_currentSpeed - _deceleration * Time.deltaTime, 0.0f);
                }
                else {
                    _currentSpeed = Mathf.Max(_currentSpeed - _deceleration * Time.deltaTime, 0.0f);
                }
            }

            if (_currentSpeed != 0.0f) {
                Vector3 levelPos = Level.Instance.transform.position;

                float newPos = levelPos.y + _currentSpeed * Time.deltaTime;

                bool isDigging = Level.Instance.CheckIfDigging(newPos);

                if (isDigging && _miner.drillSpeed <= 20) {
                    _currentSpeed = 0.0f;
                    return;
                }

                if (_miner.drillSpeed > 0) {
                    levelPos.y += _currentSpeed * Time.deltaTime * 0.2f;
                } else {
                    levelPos.y += _currentSpeed * Time.deltaTime;
                }

                if (Level.Instance != null) {
                    if (Level.Instance.MoveLevel(levelPos.y)) {
                        FuelController.Instance.UpdateFuel(-1.0f);
                    }
                }
            }
        }
    }
}
