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
            sprintSlider.maxValue = GameStateData.playerMaxStamina;
            sprintSlider.minValue = 0f;
        }

        private void Update()
        {
            sprintSlider.value = GameStateData.playerCurrentStamina;
        }
    }
}
