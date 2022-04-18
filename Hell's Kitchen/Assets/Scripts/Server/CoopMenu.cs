using Common;
using Common.Enums;
using Input;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CoopMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_InputField gameRoomInput;

        public void CreateRoom()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.CreateRoom(gameRoomInput.text);
            }
            else
            {
                Debug.LogError("You must connect to a server first, before creating a room.");
            }
        }

        public void JoinRoom()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRoom(gameRoomInput.text);
            }
            else
            {
                Debug.LogError("You must connect to a server first, before joining a room.");
            }
        }

        public override void OnJoinedRoom()
        {
            SceneManager.Instance.LoadRestaurantScene();
            InputManager.Instance.Activate();
        }
    }
}