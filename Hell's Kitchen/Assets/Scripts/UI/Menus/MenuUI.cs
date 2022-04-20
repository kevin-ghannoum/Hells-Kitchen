using Common;
using Photon.Pun;
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
            PhotonNetwork.LeaveRoom();
            SceneManager.Instance.LoadMainMenu();
        }
    }
}