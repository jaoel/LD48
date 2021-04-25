using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class FuelController : MonoBehaviour {
        public float Fuel { get; private set; }

        private float _maxFuel = 100.0f;

        public void UpdateFuel(float rate) {
            Fuel += rate * Time.deltaTime;

            Fuel = Mathf.Max(Mathf.Min(Fuel, _maxFuel), 0.0f);
        }

        private void OnGUI() {
            GUI.Label(new Rect(0, 0, 100, 20), Fuel.ToString());
        }

    }
}
