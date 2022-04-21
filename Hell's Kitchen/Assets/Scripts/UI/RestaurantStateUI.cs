using Common;
using Common.Enums;
using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace UI
{
    public class RestaurantStateUI : MonoBehaviour
    {
        [SerializeField] private GameObject openSign;
        [SerializeField] private GameObject closedSign;
        private void Start()
        {
            if (SceneManager.GetActiveScene().name.Equals(Scenes.Restaurant))
            {
                if (GameStateData.hiddenLevel == 0)
                {
                    closedSign.SetActive(true);
                    openSign.SetActive(true);
                }
                else
                {
                    openSign.SetActive(true);
                    closedSign.SetActive(false);
                }
            }
        }
    }
}