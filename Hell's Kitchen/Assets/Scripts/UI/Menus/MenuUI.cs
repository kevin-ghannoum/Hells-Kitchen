using Common;
using Photon.Pun;
using UnityEngine;

namespace UI
{
    public abstract class MenuUI : MonoBehaviourPunCallbacks
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
