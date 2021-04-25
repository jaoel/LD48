using UnityEngine;

namespace LD48 {
    public class FuelController : MonoBehaviour {
        public static FuelController Instance { get; private set; } = null;

        private float _maxFuel = 100.0f;
        public float Fuel { get; private set; } = 0;

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
            UIManager.Instance.fuelText.SetText(((int)Fuel).ToString());
        }

        public void UpdateFuel(float rate) {
            Fuel += rate * Time.deltaTime;

            Fuel = Mathf.Max(Mathf.Min(Fuel, _maxFuel), 0.0f);

            UIManager.Instance.fuelText.SetText(((int)Fuel).ToString());
        }

        public void UpdateMax(float newMax) {
            _maxFuel = newMax;
        }

    }
}
