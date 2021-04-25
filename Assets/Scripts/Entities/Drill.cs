using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class Drill : MonoBehaviour {

        private float _currentDepth = 0.0f;
        private float _maxDepth = 0.0f;

        [SerializeField]
        private FuelController _fuelController = null;

        private void Awake() {
            _fuelController = GetComponent<FuelController>();
        }


        public void MoveDrill(float newY) {
            if (newY >= _maxDepth && _fuelController.Fuel <= 0.0f) {
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
