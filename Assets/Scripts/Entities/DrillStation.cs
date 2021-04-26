using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class DrillStation : Interactable {
        private const float switchOnRot = 35f;
        private const float switchOffRot = 65f;

        public Transform switchTransform;

        [SerializeField]
        private MinerVessel _miner = null;

        private bool _active = false;
        private float _drillVelocity = 0.0f;

        private float _maxSpeed = 360.0f;

        private bool _hasSeenTutorial = false;
        [SerializeField]
        private Transform _toolTipPos = null;

        protected override void OnInteract() {
            base.OnInteract();
            _active = !_active;
            _hasSeenTutorial = true;
        }

        protected override void OnRelease() {
            base.OnRelease();
        }

        protected override void Update() {
            base.Update();

            if (PlayerInReach && !_hasSeenTutorial) {
                UIManager.Instance.DisplayTextPanel(_toolTipPos, "Press [space] to toggle drill");
            }

            if (FuelController.Instance.Fuel <= 0.0f) {
                _active = false;
            }

            if (_active) {
                if (_miner.drillSpeed < _maxSpeed) {
                    _miner.drillSpeed = Mathf.SmoothDamp(_miner.drillSpeed, _maxSpeed, ref _drillVelocity, 1.0f);
                }

                FuelController.Instance.UpdateFuel(-3.0f);
            }
            else {
                if(_miner.drillSpeed > 0) {
                    _miner.drillSpeed = Mathf.SmoothDamp(_miner.drillSpeed, 0, ref _drillVelocity, 1.0f);
                }
            }

            Vector3 rot = switchTransform.localEulerAngles;
            if (_active) {
                rot.x = Mathf.MoveTowards(rot.x, switchOnRot, Time.time * 10f);
            } else {
                rot.x = Mathf.MoveTowards(rot.x, switchOffRot, Time.time * 10f);
            }
            switchTransform.localEulerAngles = rot;
        }
    }
}
