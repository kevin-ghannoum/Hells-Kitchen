using Common.Enums;
using UnityEngine;

namespace UI
{
    public class MenuUI : MonoBehaviour
    {
        public void LoadRestaurantScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(Scenes.Restaurant);
        }

        public void OnQuit()
        {
            Application.Quit();
        }
        
        public void ReturnToMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(Scenes.MainMenu);
        }
    }
}