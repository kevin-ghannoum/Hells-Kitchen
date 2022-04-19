using Common.Enums;
using Photon.Pun;
using UnityEngine;

namespace Player
{
    public class SpawnPlayers : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform[] spawnPoints;
        
        private void Awake()
        {
            int numPlayers = PhotonNetwork.PlayerList.Length - 1;
            var spawnPoint = spawnPoints[numPlayers % spawnPoints.Length];
            var player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);
            player.GetComponentInChildren<Camera>().gameObject.tag = Tags.MainCamera;
            player.transform.Find("VirtualCamera").gameObject.SetActive(true);
        }
    }
}
