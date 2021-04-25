﻿using UnityEngine;

namespace LD48 {
    public class Teleporter : Interactable {
        public Teleporter targetTeleporter = null;
        public Transform movingPart = null;
        public Transform closedTarget = null;
        public Transform openTarget = null;
        public ParticleSystem useParticleSystem = null;
        public ParticleSystem activeParticleSystem = null;

        public bool CanUse => targetTeleporter != null && targetTeleporter.targetTeleporter == this;

        private float targetOpenness = 0f;
        private float openness = 1f;

        private bool receivedTeleport = false;

        private void Start() {
            activeParticleSystem.Stop();
        }

        public void SetTargetTeleporter(Teleporter newTarget) {
            if (newTarget != null) {
                SetTeleporter(newTarget, this);
            }

            SetTeleporter(this, newTarget);
        }

        private static void SetTeleporter(Teleporter teleporter, Teleporter newTarget) {
            if (teleporter.targetTeleporter != null) {
                teleporter.targetTeleporter.targetTeleporter = null;
            }
            teleporter.targetTeleporter = newTarget;
        }

        public void Teleport(Player player) {
            if (!receivedTeleport && player != null && targetTeleporter != null) {
                player.Teleport(targetTeleporter.transform.position);
                useParticleSystem.Play();
                targetTeleporter.useParticleSystem.Play();
                targetTeleporter.receivedTeleport = true;
                HandleExit(player);
            }
        }

        protected override void OnInteract() {
            if (!CanUse) {
                return;
            }

            base.OnInteract();
            Teleport(interactingPlayer);
        }

        protected override void Update() {
            base.Update();

            targetOpenness = CanUse ? 1f : 0f;

            if (openness != targetOpenness) {
                openness = Mathf.MoveTowards(openness, targetOpenness, Time.deltaTime * 5f);

                movingPart.transform.localPosition = Vector3.Lerp(closedTarget.localPosition, openTarget.localPosition, openness);
                if (openness == 0f) {
                    activeParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                } else if (openness == 1f) {
                    activeParticleSystem.Play();
                }
            }
        }

        private void LateUpdate() {
            receivedTeleport = false;
        }
    }
}