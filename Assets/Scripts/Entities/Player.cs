using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class Player : MonoBehaviour {
        [SerializeField]
        private CharacterController _characterController = null;

        private float _maxSpeed = 10.0f;
        public float _speed = 7.0f;
        private float _acceleration = 0.0f;

        private void Awake() {
            
        }

        private void Start() {

        }

        private void Update() {

            if (_characterController != null) {

                Vector3 forward = Vector3.zero;
                Vector3 velocity = new Vector3(_speed, 9.82f, _speed);
                if (Input.GetKey(KeyCode.A)) {
                    forward.x = -1;
                }
                else if (Input.GetKey(KeyCode.D)) {
                    forward.x = 1;
                }

                if (Input.GetKey(KeyCode.W)) {
                    forward.z = 1;
                } else if (Input.GetKey(KeyCode.S)) {
                    forward.z = -1;
                }


                if (forward != Vector3.zero || !_characterController.isGrounded) {
                    forward = forward.normalized;

                    if (forward.x != 0 || forward.z != 0) {
                        transform.forward = forward;
                    }

                    if (!_characterController.isGrounded) {
                        forward.y = -1.0f;
                    }

                    Debug.Log(forward);

                    _characterController.Move(Vector3.Scale(forward.normalized, velocity * Time.deltaTime));
                }
            }
        }
    }
}


