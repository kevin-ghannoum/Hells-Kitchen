using Common;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SprintMeterUI : MonoBehaviour
    {
        [SerializeField] private Slider sprintSlider;

        private void Start()
        {
            sprintSlider.maxValue = GameStateManager.Instance.playerMaxStamina;
            sprintSlider.minValue = 0f;
        }

        private void Update()
        {
            sprintSlider.value = GameStateManager.Instance.playerCurrentStamina;
        }
    }
}
