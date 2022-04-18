using System;
using Common;
using UnityEngine;

namespace UI
{
    public class ClockUI : MonoBehaviour
    {
        [SerializeField] private float realSecondsPerDay = 30f;
        [SerializeField] private RectTransform clockHandTransform;
        [SerializeField] private float clockHandOffset = 90f;
        private float _day = 0f;

        public void ResetClock()
        {
            _day = 0f;
        }

        private void Update()
        {
            _day += Time.deltaTime / realSecondsPerDay;

            if (_day >= 1f)
            {
                // Time elapsed, stop clock and let player find exit
                GameStateManager.Instance.dungeonTimeHasElapsed = true;
                return;
            }

            float dayNormalized = _day % 1f;
            float rotationDegreesPerDay = 180f;
            clockHandTransform.eulerAngles = new Vector3(0, 0, - dayNormalized * rotationDegreesPerDay + clockHandOffset);
        }
    }
}
