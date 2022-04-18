using Photon.Pun;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    // connect to server
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // on connect to server, join lobby
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
}