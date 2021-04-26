using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class Player : MonoBehaviour {
        public static Player Instance { get; private set; } = null;

        [SerializeField]
        private CharacterController _characterController = null;

        public Animator animator = null;
        public Transform tutorialTransform = null;

        private float _maxSpeed = 10.0f;
        public float _speed = 7.0f;
        private float _acceleration = 20.0f;
        private float _currentVelocity = 0;

        private Vector3 _oldForward = Vector3.zero;
        public bool hasDiamond = false;

        public int Resources { get; set; }
        public int MaxResources { get; set; } = 1000;

        private void Awake() {
            if (Instance != null) {
                Debug.LogError("Player already exists");
                DestroyImmediate(gameObject);
                return;
            }
            Instance = this;
        }

        public void Teleport(Vector3 position) {
            _characterController.enabled = false;
            transform.position = position;
            _characterController.enabled = true;
        }

        public void AddResource(int resource) {
            Resources = Mathf.Min(Resources + resource, MaxResources);
        }

        private void Update() {
            if (_characterController != null) {

                Vector3 forward = Vector3.zero;

                if (Input.GetKey(KeyCode.D)) {
                    forward.x = -1;
                } else if (Input.GetKey(KeyCode.A)) {
                    forward.x = 1;
                }

                if (Input.GetKey(KeyCode.S)) {
                    forward.z = 1;
                } else if (Input.GetKey(KeyCode.W)) {
                    forward.z = -1;
                }


                if (forward != Vector3.zero/* || !_characterController.isGrounded*/) {
                    forward = forward.normalized;

                    if (forward.x != 0 || forward.z != 0) {
                        transform.forward = forward;
                    }

                    //if (!_characterController.isGrounded) {
                    //    forward.y = -1.0f;
                    //}

                    _currentVelocity += _acceleration * Time.deltaTime;
                    _currentVelocity = Mathf.Min(_currentVelocity, _maxSpeed);
                    Vector3 velocity = new Vector3(_currentVelocity, 0, _currentVelocity);

                    _characterController.Move(Vector3.Scale(forward.normalized, velocity * Time.deltaTime));
                    _oldForward = forward;
                }

                if (!_characterController.isGrounded) {
                    _characterController.Move(new Vector3(0, -1 * 9.82f * Time.deltaTime, 0));
                }

                if (forward.x == 0 && forward.z == 0 && _currentVelocity > 0) {
                    _currentVelocity -= _acceleration * Time.deltaTime;
                    _currentVelocity = Mathf.Max(_currentVelocity, 0);
                    Vector3 velocity = new Vector3(_currentVelocity, 0, _currentVelocity);

                    _characterController.Move(Vector3.Scale(_oldForward.normalized, velocity * Time.deltaTime));

                    if (_currentVelocity <= 0.0f) {
                        _oldForward = Vector3.zero;
                    }

                    animator.SetFloat("RunSpeed", _oldForward.magnitude);

                    Debug.Log(_currentVelocity);
                } else {
                    animator.SetFloat("RunSpeed", forward.magnitude);
                }

                animator.SetFloat("RunSpeed2", Mathf.Max(_currentVelocity / _maxSpeed, 0.1f));
            }

            UIManager.Instance.resourcesText.SetText(Resources.ToString() + "/" + MaxResources.ToString());
        }
    }
}


