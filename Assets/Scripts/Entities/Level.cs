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

        public void Update() {
            if (FuelController.Instance.Fuel <= 0.0f && _currentDepth > 0.0f && !_probes.Any(x => x.CurrentState == Probe.State.Moving)) {
                MoveLevel(transform.position.y - 20.0f * Time.deltaTime);

                if (!_destroyedResources) {
                    Player.Instance.Resources = Mathf.CeilToInt((float)Player.Instance.Resources * 0.5f);
                    _destroyedResources = true;
                }
            }
            else {
                _destroyedResources = false;
            }
        }
    }
}
