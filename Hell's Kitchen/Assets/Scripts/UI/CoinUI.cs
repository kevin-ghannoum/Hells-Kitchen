using Common;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CoinUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinValue;

        private void Start()
        {
            coinValue.text = "{000}";
        }

        private void Update()
        {
            coinValue.text = $"{GameStateManager.Instance.cashMoney}";
        }
    }
}
