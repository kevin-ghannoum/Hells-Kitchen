using Common;
using Photon.Pun;

namespace UI
{
    public class MenuUI : MonoBehaviourPunCallbacks
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
            GameStateManager.Instance.ResetDefaults();
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            SceneManager.Instance.LoadMainMenu();
        }
    }
}
