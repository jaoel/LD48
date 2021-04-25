using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class FuelController : MonoBehaviour {
        public float Fuel { get; private set; }

        public void Refuel(float rate) {
            Fuel += rate * Time.deltaTime;
        }

        private void OnGUI() {
            GUI.Label(new Rect(0, 0, 100, 20), Fuel.ToString());
        }

    }
}
