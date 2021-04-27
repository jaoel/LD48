using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LD48 {
    public class Level : MonoBehaviour {
        public static Level Instance { get; private set; } = null;

        private float _currentDepth = 0.0f;
        private float _maxDepth = 0.0f;

        private List<Probe> _probes = new List<Probe>();

        private bool _destroyedResources = false;

        public bool tooHot = false;
        private bool hasOpenedWinScreen = false;

        public AudioSource drillingAudioSource;

        private float drillingVolume = 0f;

        private bool dugThisFrame = false;

        private void Awake() {
            if (Instance != null) {
                Debug.LogError("Level already exists");
                DestroyImmediate(gameObject);
                return;
            }
            Instance = this;

            GameObject[] probes = GameObject.FindGameObjectsWithTag("Probe");
            foreach (GameObject go in probes) {
                _probes.Add(go.GetComponent<Probe>());
            }
        }

        private void Start() {
            drillingAudioSource.Play();
            drillingAudioSource.Pause();
        }

        public bool CheckIfDigging(float newY) {
            return newY > _maxDepth;
        }

        public bool MoveLevel(float newY) {
            tooHot = false;
            if (newY >= _maxDepth && FuelController.Instance.Fuel <= 0.0f) {
                return false;
            }

            if (newY >= _maxDepth && TerrainManager.Instance.TryGetCurrentSegment(out var currentSegment)) {
                if (currentSegment.unminable) {
                    tooHot = true;
                    return false;
                }
            }

            if (newY > _maxDepth) {
                drillingVolume = Mathf.MoveTowards(drillingVolume, 1f, Time.deltaTime * 2f);
                dugThisFrame = true;
            }

            _currentDepth = newY;
            Vector3 pos = transform.position;
            pos.y = Mathf.Max(0, newY);
            transform.position = pos;

            _maxDepth = Mathf.Max(newY, _maxDepth);

            if (pos.y == 0) {
                if (!hasOpenedWinScreen && Player.Instance.hasDiamond) {
                    hasOpenedWinScreen = true;
                    UIManager.Instance.winScreen.SetActive(true);
                    Time.timeScale = 0f;
                }
                return false;
            }

            return true;
        }

        private float outOfFuelTime = 0f;
        private string outOfFuelText = null;

        public void Update() {
            if (FuelController.Instance.Fuel <= 0.0f && _currentDepth > 0.0f && !_probes.Any(x => x.CurrentState == Probe.State.Moving)) {
                Vector3 toPlayer = Player.Instance.transform.position;
                toPlayer.y = 0f;
                if (toPlayer.magnitude < 10f) {
                    MoveLevel(transform.position.y - 20.0f * Time.deltaTime);

                    if (outOfFuelTime < 0f) {
                        outOfFuelTime = Time.time + 5f;
                        if (Player.Instance.hasDiamond) {
                            outOfFuelText = "Your cargo is too heavy\nYou'll have to leave\nthat thing behind!";
                            Player.Instance.hasDiamond = false;
                            Diamond.updated?.Invoke();
                        } else {
                            outOfFuelText = "Uncle Bob's express towing\nto the rescue!\nFor a price...";
                        }
                    }

                    if (!_destroyedResources) {
                        Player.Instance.Resources = Mathf.CeilToInt((float)Player.Instance.Resources * 0.5f);
                        _destroyedResources = true;
                    }
                }
            }
            else {
                outOfFuelTime = -1f;
                outOfFuelText = null;
                _destroyedResources = false;
            }

            if (outOfFuelText != null) {
                UIManager.Instance.DisplayTextPanel(Player.Instance.transform, outOfFuelText);
            }
        }

        private void LateUpdate() {
            if (!dugThisFrame) {
                drillingVolume = Mathf.MoveTowards(drillingVolume, 0f, Time.deltaTime * 2f);
            }

            drillingAudioSource.volume = drillingVolume;

            if (drillingVolume == 0f) {
                drillingAudioSource.Pause();
            } else {
                drillingAudioSource.UnPause();
            }

            dugThisFrame = false;
        }
    }
}
