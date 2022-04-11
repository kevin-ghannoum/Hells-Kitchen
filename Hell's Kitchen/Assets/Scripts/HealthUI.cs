using Common.Enums;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthValue;
    [SerializeField] private Slider healthSlider;

    private PlayerHealth _playerHealth;
    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag(Tags.Player);
        if (!player)
            return;

        _playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        healthSlider.minValue = 0f;
        healthSlider.maxValue = PlayerHealth.MaxHitPoints;
    }

    private void Update()
    {
        healthValue.text = $"{_playerHealth.HitPoints} / {PlayerHealth.MaxHitPoints}";
        healthSlider.value = _playerHealth.HitPoints;
    }
}
