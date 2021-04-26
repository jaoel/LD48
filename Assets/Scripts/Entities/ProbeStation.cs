using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class ProbeStation : Interactable {
        private const float leverOnAngle = 90f;
        private const float leverOffAngle = 0f;

        public Transform leverArm;

        private float leverCurrentAngle = 0f;

        [SerializeField]
        private Probe _probe = null;

        public  bool _hasSeenTutorial = false;
        [SerializeField]
        private Transform _toolTipPos = null;

        protected override void Update() {
            base.Update();

            if (_probe.CurrentState == Probe.State.Idle) {
                leverCurrentAngle = Mathf.MoveTowards(leverCurrentAngle, leverOffAngle, Time.deltaTime * 90f);
            } else {
                float dist = Mathf.Clamp01(Mathf.Abs(leverCurrentAngle - leverOnAngle) / leverOnAngle);
                dist = dist * dist;
                leverCurrentAngle = Mathf.MoveTowards(leverCurrentAngle, leverOnAngle, Time.deltaTime * (20f + (1f - dist) * 1000f));
            }
            leverArm.localRotation = Quaternion.Euler(leverCurrentAngle, 0f, 0f);

            if (PlayerInReach && !_hasSeenTutorial) {
                UIManager.Instance.DisplayTextPanel(_toolTipPos, "Press [space] to fire mining probe");
            }
        }

        protected override void OnInteract() {
            base.OnInteract();

            if (FuelController.Instance.Fuel > 0.0f) {
                _probe.Fire();
            }

            _hasSeenTutorial = true;
        }

        protected override void OnRelease() {
            base.OnRelease();
        }
    }
}
