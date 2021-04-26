using UnityEngine;

namespace LD48 {
    public class FuelController : MonoBehaviour {
        public static FuelController Instance { get; private set; } = null;

        public FuelTank tank = null;

        public float _maxFuel = 100.0f;
        public float Fuel { get; private set; } = 0;

        public bool ToppedOff => Fuel >= _maxFuel;

        private void Awake() {
            if (Instance != null) {
                Debug.LogError("FuelController already exists");
                DestroyImmediate(gameObject);
                return;
            }
            Instance = this;

            Fuel = _maxFuel;
        }

        private void Start() {
            UpdateFuelUIText();
        }

        public void UpdateFuel(float rate) {
            Fuel += rate * Time.deltaTime;

            Fuel = Mathf.Max(Mathf.Min(Fuel, _maxFuel), 0.0f);

            UpdateFuelUIText();

            if (tank != null) {
                tank.SetFuel(Fuel / _maxFuel);
            }
        }

        private void UpdateFuelUIText() {
            UIManager.Instance.fuelText.SetText($"{(int)Fuel}/{(int)_maxFuel}");
        }

        public void UpdateMax(float newMax) {
            _maxFuel = newMax;
            UpdateFuelUIText();
        }

    }
}
