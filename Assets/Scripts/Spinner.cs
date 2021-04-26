using UnityEngine;

namespace LD48 {
    public class Spinner : MonoBehaviour {
        float rot = 0f;
        private void Awake() {
            rot = Random.Range(0f, 360f);
        }

        private void Update() {
            rot += Time.deltaTime * 35f;
            transform.localEulerAngles = new Vector3(0f, rot, 0f);
        }
    }
}
