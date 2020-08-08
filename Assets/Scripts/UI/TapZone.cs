using UnityEngine;
using UnityEngine.EventSystems;

using Grigorov.Events;

namespace Grigorov.LeapAndJump.UI {
    public struct TapZone_PointerDown {
        public PointerEventData PointerEventData;

        public TapZone_PointerDown(PointerEventData pointerEventData) {
            PointerEventData = pointerEventData;
        }
    }
    
    public class TapZone : MonoBehaviour, IPointerDownHandler {
        public void OnPointerDown(PointerEventData pointerEventData) {
            EventManager.Fire(new TapZone_PointerDown(pointerEventData));
        }
    }
}