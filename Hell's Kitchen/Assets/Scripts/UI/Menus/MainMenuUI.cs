using Common;
using Input;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MainMenuUI : MenuUI
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject coopMenu;

        private void Awake()
        {
            title.outlineWidth = .05f;
            title.outlineColor = Color.black;
            animator.Play("MenuCamera");
            InputManager.Instance.Deactivate();
        }

        public void OnPlay()
        {
            if (PhotonNetwork.IsConnected)
            {
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.MaxPlayers = 1;
                PhotonNetwork.CreateRoom(Random.Range(0, int.MaxValue).ToString(), roomOptions);
            }
            else
            {
                Debug.LogError("You must connect to a server first, before creating a room.");
            }
        }

        public void CoopMode()
        {
            mainMenu.SetActive(false);
            coopMenu.SetActive(true);
        }

        public void Back()
        {
            mainMenu.SetActive(true);
            coopMenu.SetActive(false);
        }
        
        public override void OnJoinedRoom()
        {
            SceneManager.Instance.LoadRestaurantScene();
            InputManager.Instance.Activate();
        }
    }
}
