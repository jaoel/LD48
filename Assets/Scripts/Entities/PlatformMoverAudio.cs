using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LD48 {
    public class PlatformMoverAudio : MonoBehaviour {
        public AudioSource engineAudioSource;

        private float engineVolume = 0f;

        public PlatformMover mover1;
        public PlatformMover mover2;
        public MinerVessel miner;

        float enginePitch = 0.5f;

        private void Start() {
            engineAudioSource.Play();
            engineAudioSource.Pause();
        }

        protected void Update() {
            float maxSpeed = Mathf.Max(Mathf.Abs(mover1.currentSpeed), Mathf.Abs(mover2.currentSpeed));
            Vector3 levelPos = Level.Instance.transform.position;

            float newPos = levelPos.y + maxSpeed * Time.deltaTime;

            bool isDigging = Level.Instance.CheckIfDigging(newPos);

            if (levelPos.y != 0f && isDigging) {
                enginePitch = Mathf.MoveTowards(enginePitch, Mathf.Clamp01(Mathf.Min(engineVolume, maxSpeed / 50f)) * Mathf.Clamp01(miner.drillSpeed / 360f), Time.deltaTime);
            } else {
                enginePitch = Mathf.MoveTowards(enginePitch, 0f, Time.deltaTime);
            }

            if (DrillStation.Instance.Active) {
                engineVolume = Mathf.MoveTowards(engineVolume, 1f, Time.deltaTime * 2f);
            } else {
                engineVolume = Mathf.MoveTowards(engineVolume, 0f, Time.deltaTime * 2f);
            }


            if (engineVolume == 0f) {
                engineAudioSource.Pause();
            } else {
                engineAudioSource.UnPause();

                engineAudioSource.volume = engineVolume;
                engineAudioSource.pitch = Mathf.Lerp(0.5f, 0.75f, enginePitch);
            }
        }
    }
}
