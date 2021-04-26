using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace LD48 {
    public class DrillStation : Interactable {
        public static DrillStation Instance { get; private set; } = null;

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

        private Tweener _cameraShake = null;
        private Tweener _infinteShake = null;

        public AudioSource audioSource;
        public AudioClip onClip;
        public AudioClip offClip;

        private bool lastActive = false;

        public bool Active => _active;

        private void Awake() {
            Instance = this;
            lastActive = _active;
        }

        protected override void OnInteract() {
            base.OnInteract();
            _active = !_active;
            _hasSeenTutorial = true;

            if (_active) {

                _cameraShake = Camera.main.transform.parent.parent.DOShakePosition(1, 1, 10, 180);//DOTween.Sequence();
                //_cameraShake.Append(Camera.main.transform.parent.parent.DOShakePosition(1, 1, 10, 180)).AppendCallback(() => {
                //    _infinteShake = Camera.main.transform.parent.parent.DOShakePosition(9000, 0.1f, 2, 90).SetLoops(-1);
                //});
            }
            else {
                _cameraShake = Camera.main.transform.parent.parent.DOShakePosition(1, 1, 10, 180);
                //_cameraShake.Kill(true);
                //_infinteShake.Kill(false);
            }
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

            if (lastActive != _active) {
                if (_active) {
                    audioSource.clip = onClip;
                } else {
                    audioSource.clip = offClip;
                }
                audioSource.Play();
            }

            lastActive = _active;
        }
    }
}
