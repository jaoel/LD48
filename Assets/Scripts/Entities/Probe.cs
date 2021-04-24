using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class Probe : MonoBehaviour {

        enum State {
            Idle,
            Moving
        }

        private float _time = 2.0f;
        private float _speed = 10.0f;
        private Vector3 _direction = Vector3.zero;

        private Vector3 _origin = Vector3.zero;
        private Vector3 _startPosition = Vector3.zero;
        private float _timeElapsed = 0.0f;

        private State _state = State.Idle;

        private void Awake() {
            _origin = transform.position;
        }

        private void Start() {

        }

        private void Update() {

            if (_state == State.Moving) {
                if (_timeElapsed <= _time) {
                    transform.position += _direction * _speed * Time.deltaTime;
                    _timeElapsed += Time.deltaTime;

                    if (_direction == -transform.forward && Vector3.Distance(transform.position, _origin) < 0.1f) {
                        transform.position = _origin;
                        _timeElapsed = 0.0f;
                        _state = State.Idle;
                    }
                } else {
                    _timeElapsed = 0.0f;
                    _direction = -transform.forward;
                }


            }
        }

        public void Fire() {
            if (_state == State.Idle) {
                _direction = transform.forward;
                _state = State.Moving;
                _timeElapsed = 0.0f;
            }
        }

        private void OnTriggerEnter(Collider other) {
            Resource resource = other.GetComponentInParent<Resource>();

            Destroy(resource.gameObject);

            _direction = -transform.forward;
            _timeElapsed = 0.0f;
        }
    }
}
