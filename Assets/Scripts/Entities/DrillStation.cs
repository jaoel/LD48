using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class DrillStation : Interactable {

        [SerializeField]
        private MinerVessel _miner = null;

        private bool _active = false;
        private float _drillVelocity = 0.0f;

        private float _maxSpeed = 360.0f;

        protected override void OnInteract() {
            base.OnInteract();

            _active = !_active;
        }

        protected override void OnRelease() {
            base.OnRelease();
        }

        protected override void Update() {
            base.Update();

            if (_active) {
                if (_miner.drillSpeed < _maxSpeed) {
                    _miner.drillSpeed = Mathf.SmoothDamp(_miner.drillSpeed, _maxSpeed, ref _drillVelocity, 1.0f);
                }

                FuelController.Instance.UpdateFuel(-1.0f);
            }
            else {
                if(_miner.drillSpeed > 0) {
                    _miner.drillSpeed = Mathf.SmoothDamp(_miner.drillSpeed, 0, ref _drillVelocity, 1.0f);
                }
            }

        }
    }
}
