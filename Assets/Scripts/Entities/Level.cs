using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class Level : MonoBehaviour {
        public static Level Instance { get; private set; } = null;

        private float _currentDepth = 0.0f;
        private float _maxDepth = 0.0f;

        private void Awake() {
            if (Instance != null) {
                Debug.LogError("Level already exists");
                DestroyImmediate(gameObject);
                return;
            }
            Instance = this;
        }

        public void MoveLevel(float newY) {
            if (newY >= _maxDepth && FuelController.Instance.Fuel <= 0.0f) {
                return;
            }

            _currentDepth = newY;
            Vector3 pos = transform.position;
            pos.y = Mathf.Max(0, newY);
            transform.position = pos;

            _maxDepth = Mathf.Max(newY, _maxDepth);
        }

    }
}
