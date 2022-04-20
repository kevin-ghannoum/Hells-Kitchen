using Common;
using Common.Enums;
using Photon.Pun;
using UnityEngine;

namespace Player
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] public PhotonView photonView;
        public bool shouldSpawnOnAwake = false;
        
        private void Awake()
        {
            if (!shouldSpawnOnAwake)
                return;
            
            SpawnPlayerInScene();
        }

        [PunRPC]
        public void SpawnPlayerInScene()
        {
            int numPlayers = PhotonNetwork.PlayerList.Length - 1;
            var spawnPoint = spawnPoints[numPlayers % spawnPoints.Length];
            var player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);
            player.GetComponentInChildren<Camera>().gameObject.tag = Tags.MainCamera;
            player.transform.Find("VirtualCamera").gameObject.SetActive(true);
            GameStateData.player = player;
        }
    }
}
