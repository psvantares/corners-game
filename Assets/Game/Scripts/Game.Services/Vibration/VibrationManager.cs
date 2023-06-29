using UnityEngine;

namespace Game.Services.Vibration
{
    public class VibrationManager : MonoBehaviour
    {
        private static VibrationController vibrationController;
        private static IVibrationController ivc;
        private static IVibration iv;

        private float delayVibration = 0.25f;
        private bool isCanVibration;

        private void Awake()
        {
            vibrationController = new VibrationController();
            ivc = vibrationController;
            iv = vibrationController;
        }

        private void Update()
        {
            delayVibration += Time.deltaTime;

            if (delayVibration > 0.5f)
            {
                delayVibration = 0;
                isCanVibration = true;
            }
        }

        private void OnDestroy()
        {
            ivc?.Dispose();
        }

        public static void VibrationEnabled(bool value)
        {
            ivc.VibrationEnabled = value;
        }

        public static bool IsVibrationEnabled()
        {
            return ivc == null || ivc.VibrationEnabled;
        }

        public void PlayVibration()
        {
            iv.PlayVibration();
        }

        public void PlayVibrationWithDelay()
        {
            if (!isCanVibration)
            {
                return;
            }

            isCanVibration = false;

            iv.PlayVibration();
        }
    }
}