using UnityEngine;
using UnityEngine.EventSystems;

namespace LD48 {
    public class AudioSliderOnDragEnd : MonoBehaviour, IPointerUpHandler {
        public AudioSource audioSource;
        public void OnPointerUp(PointerEventData eventData) {
            audioSource.Play();
        }
    }
}
