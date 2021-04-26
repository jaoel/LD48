using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class Probe : MonoBehaviour {

        public enum State {
            Idle,
            Moving
        }

        public Transform probeHead;
        public Transform probeHeadVisual;
        public Transform probeArm;

        private float _time = 2.0f;
        private float _speed = 10.0f;
        private float _direction = 1f;

        private float _timeElapsed = 0.0f;

        private State _state = State.Idle;

        public State CurrentState { get => _state; }

        public AudioSource audioSource;

        private void Update() {

            if (_state == State.Moving) {
                if (_timeElapsed <= _time) {
                    Vector3 pos = probeHead.localPosition;
                    pos.z += _direction * _speed * Time.deltaTime;

                    if (_direction < 0f && pos.z < 0.1f) {
                        pos.z = 0f;
                        _timeElapsed = 0.0f;
                        _state = State.Idle;
                        audioSource.Stop();
                    }

                    probeHead.localPosition = pos;
                    _timeElapsed += Time.deltaTime;
                } else {
                    _timeElapsed = 0.0f;
                    _direction = -1f;
                }

                if (_direction > 0f) {
                    audioSource.pitch = 1f;
                } else {
                    audioSource.pitch = 0.96f;
                }

                probeArm.localScale = new Vector3(1f, 1f, probeHead.localPosition.z);
                probeHeadVisual.localRotation = Quaternion.Euler(0f, 0f, -Time.time * 720f);
            }
        }

        public void Fire() {
            if (_state == State.Idle) {
                FuelController.Instance.UpdateFuel(-20.0f);
                _direction = 1f;
                _state = State.Moving;
                audioSource.Play();
                _timeElapsed = 0.0f;
            }
        }

        public void Upgrade(float newSpeed) {
            _speed += newSpeed;
        }

        public void OnPickedUpResource(Resource resource) {
            Player.Instance.AddResource(resource.Value);

            Destroy(resource.gameObject);

            _direction = -1f;
            _timeElapsed = 0.0f;
        }
    }
}
