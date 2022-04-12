using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthValue;
        [SerializeField] private Slider healthSlider;

        private void Start()
        {
            healthSlider.minValue = 0f;
            healthSlider.maxValue = GameStateManager.Instance.playerMaxHitPoints;
        }

        private void Update()
        {
            healthValue.text = $"{GameStateManager.Instance.playerCurrentHitPoints} / {GameStateManager.Instance.playerMaxHitPoints}";
            healthSlider.value = GameStateManager.Instance.playerCurrentHitPoints;
        }
    }
}
