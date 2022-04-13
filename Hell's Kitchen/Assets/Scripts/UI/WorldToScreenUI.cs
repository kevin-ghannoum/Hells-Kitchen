using UnityEngine;

namespace UI
{
    public class WorldToScreenUI : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private RectTransform textTransform;
        [SerializeField] private float offsetY = 2.5f;
        private void LateUpdate()
        {
            if (!Camera.main)
                return;

            var canvasRect = GetComponent<RectTransform>();
            
            // Offset position above object (in world space)
            var position = target.position;
            float offsetPosY = position.y + offsetY;
            
            // Final position of marker above GO in world space
            Vector3 offsetPos = new Vector3(position.x, offsetPosY, position.z);

            
            // Calculate *screen* position (note, not a canvas/recttransform position)
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);
 
            // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out var canvasPos);
 
            // Set
            textTransform.localPosition = canvasPos;
            textTransform.forward = transform.forward;
        }
    }
}
