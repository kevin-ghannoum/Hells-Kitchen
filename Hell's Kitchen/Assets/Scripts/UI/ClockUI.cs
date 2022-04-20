using System;
using Common;
using Photon.Pun;
using UnityEngine;

namespace UI
{
    public class ClockUI : MonoBehaviour
    {
        [SerializeField] private float realSecondsPerDay = 30f;
        [SerializeField] private RectTransform clockHandTransform;
        [SerializeField] private float clockHandOffset = 90f;

        private void Update()
        {
            if (PhotonNetwork.IsMasterClient && GameStateData.dungeonClock < 1.0f)
            {
                GameStateData.dungeonClock += Time.deltaTime / realSecondsPerDay;
            }

            float dayNormalized = Mathf.Clamp(GameStateData.dungeonClock, 0.0f, 1.0f);
            float rotationDegreesPerDay = 180f;
            clockHandTransform.eulerAngles = new Vector3(0, 0, - dayNormalized * rotationDegreesPerDay + clockHandOffset);
        }
    }
}
