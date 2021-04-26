using UnityEngine;

namespace LD48 {
    public class ProbeHead: MonoBehaviour {

        public Probe probe;

        private void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent(out Resource resource)) {
                probe.OnPickedUpResource(resource);
            }
        }
    }
}
