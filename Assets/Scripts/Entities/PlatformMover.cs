using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LD48 {
    public class PlatformMover : Interactable {
        public Button button;

        public float currentSpeed = 0.0f;

        [SerializeField]
        public float _maxSpeed = 10.0f;

        [SerializeField]
        public float _acc = 10.0f;

        [SerializeField]
        public float _deceleration = 10.0f;

        [SerializeField]
        private MinerVessel _miner = null;

        private List<Probe> _probes = new List<Probe>();

        private bool _hasSeenTutorial = false;
        [SerializeField]
        private Transform _toolTipPos = null;

        private Tweener _infinteShake = null;

        private void Awake() {                                                                                                                                                          
            GameObject[] probes = GameObject.FindGameObjectsWithTag("Probe");
            foreach(GameObject go in probes) {
                _probes.Add(go.GetComponent<Probe>());
            }
        }

        protected override void OnInteract() {

            if (_probes.Any(x => x.CurrentState == Probe.State.Moving)){
                return;                                                  
            }

            base.OnInteract();
        }

        protected override void OnRelease() {
            base.OnRelease();

            _hasSeenTutorial = true;
            _infinteShake.Kill(false);
            _infinteShake = null;
        }

        protected override void Update() {
            base.Update();

            if (PlayerInReach && !_hasSeenTutorial) {
                if (_maxSpeed > 0) {
                    UIManager.Instance.DisplayTextPanel(_toolTipPos, "Hold [space] to move down");
                }
                else {
                    UIManager.Instance.DisplayTextPanel(_toolTipPos, "Hold [space] to move up");
                }
            }

            if (Interacting) {
                if (_acc < 0) {
                    currentSpeed = Mathf.Max(currentSpeed + _acc * Time.deltaTime, _maxSpeed);
                } else {
                    currentSpeed = Mathf.Min(currentSpeed + _acc * Time.deltaTime, _maxSpeed);
                }
            }
            else if (!Interacting && currentSpeed != 0.0f) {

                if (_deceleration < 0) {
                    currentSpeed = Mathf.Min(currentSpeed - _deceleration * Time.deltaTime, 0.0f);
                }
                else {
                    currentSpeed = Mathf.Max(currentSpeed - _deceleration * Time.deltaTime, 0.0f);
                }
            }

            button.Pressed = Interacting;

            if (currentSpeed != 0.0f) {
                Vector3 levelPos = Level.Instance.transform.position;

                float newPos = levelPos.y + currentSpeed * Time.deltaTime;

                bool isDigging = Level.Instance.CheckIfDigging(newPos);

                if (isDigging && _miner.drillSpeed <= 20) {
                    currentSpeed = 0.0f;
                    _infinteShake.Kill(false);
                    _infinteShake = null;
                    return;
                }

                if (isDigging) {
                    levelPos.y += currentSpeed * Time.deltaTime * 0.1f;

                    if (_infinteShake == null && Interacting && isDigging) {
                        _infinteShake = Camera.main.transform.parent.parent.DOShakePosition(9000, 0.1f, 2, 90).SetLoops(-1);
                    }
                } else {
                    levelPos.y += currentSpeed * Time.deltaTime;

                    _infinteShake.Kill(false);
                    _infinteShake = null;
                }

                if (Level.Instance != null && FuelController.Instance.Fuel > 0.0f) {
                    if (Level.Instance.MoveLevel(levelPos.y)) {
                        FuelController.Instance.UpdateFuel(-0.25f);
                    }
                }
            }
        }
    }
}
