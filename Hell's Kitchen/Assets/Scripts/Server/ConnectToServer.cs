using Photon.Pun;
using UI;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    // connect to server
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        FindObjectOfType<MainMenuUI>().ShowConnectionMessage();
    }

    // on connect to server, join lobby
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        FindObjectOfType<MainMenuUI>().ShowMainMenu();
    }
}