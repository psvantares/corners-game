using UnityEngine;
using UnityEngine.UI;

namespace Game.Core
{
    public class SafeArea : MonoBehaviour
    {
        [Header("SAFE AREA")]
        [SerializeField]
        protected bool SafeAreaEnabled;

        [Space]
        [SerializeField]
        protected CanvasScaler CanvasScaler;

        [SerializeField]
        protected RectTransform TargetRectTransform;

        protected virtual void Start()
        {
            AdjustSaveAreaScreen();
        }

        private void AdjustSaveAreaScreen()
        {
            if (!SafeAreaEnabled)
            {
                return;
            }

            if (!CanvasScaler)
            {
                CanvasScaler = transform.GetComponentInParent<CanvasScaler>();
            }

            if (!TargetRectTransform)
            {
                TargetRectTransform = GetComponent<RectTransform>();
            }

            if (!CanvasScaler || !TargetRectTransform)
            {
                return;
            }

            var safeArea = Screen.safeArea;
            var upOffset = (Screen.height - safeArea.height - safeArea.y) / Screen.height;
            var downOffset = safeArea.y / Screen.height;

            upOffset *= CanvasScaler.referenceResolution.y;
            downOffset *= CanvasScaler.referenceResolution.y;

            var newOffsetMin = new Vector2(0f, downOffset);
            var newOffsetMax = new Vector2(0f, -upOffset);

            TargetRectTransform.offsetMin = newOffsetMin;
            TargetRectTransform.offsetMax = newOffsetMax;
        }
    }
}