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
            healthSlider.maxValue = GameStateData.playerMaxHitPoints;
        }

        private void Update()
        {
            var player = GameStateData.player;
            if (!player)
                return;
            
            var playerHealth = player.GetComponent<Player.PlayerHealth>();

            healthValue.text = $"{playerHealth.internalHealth} / {GameStateData.playerMaxHitPoints}";
            healthSlider.value = playerHealth.internalHealth;
        }
    }
}
