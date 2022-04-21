using Cinemachine;
using Common;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LevelUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        private void Start()
        {
            levelText.text = $"Level {GameStateData.hiddenLevel}";
        }
    }
}
