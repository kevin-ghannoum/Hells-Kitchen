using Common;
using UnityEngine;

namespace UI
{
    public class MenuUI : MonoBehaviour
    {
        public void LoadRestaurantScene()
        {
            SceneManager.Instance.LoadRestaurantScene();
        }

        public void OnQuit()
        {
            SceneManager.Instance.QuitGame();
        }
        
        public void ReturnToMenu()
        {
            SceneManager.Instance.LoadMainMenu();
        }
    }
}