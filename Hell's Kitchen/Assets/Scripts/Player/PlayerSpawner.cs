using Common;
using Common.Enums;
using Photon.Pun;
using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Player
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject healerPrefab;
        [SerializeField] private GameObject knightPrefab;
        [SerializeField] private Transform[] spawnPoints;
        public bool shouldSpawnOnAwake = false;
        
        private void Awake()
        {
            if(!shouldSpawnOnAwake)
                return;
            
            SpawnPlayerInScene();
        }

        public void SpawnPlayerInScene()
        {
            int numPlayers = PhotonNetwork.PlayerList.Length - 1;
            var spawnPoint = spawnPoints[numPlayers % spawnPoints.Length];
            var player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);
            player.GetComponentInChildren<Camera>().gameObject.tag = Tags.MainCamera;
            player.transform.Find("VirtualCamera").gameObject.SetActive(true);
            GameStateData.player = player;
        }

        [PunRPC]
        public void AddSousChef()
        {
            // only spawn sous-chef in dungeon
            if (SceneManager.GetActiveScene().name.Equals(Scenes.Dungeon))
            {
                var prefab = GameStateData.sousChefType == SousChefType.Healer ? healerPrefab : knightPrefab;
                GameObject sousChef = PhotonNetwork.Instantiate(prefab.name, spawnPoints[0].position - new Vector3(1f, 0, 1f), Quaternion.identity);
                sousChef.tag = Tags.SousChef;
                GameStateData.sousChef = sousChef;
            }
        }
    }
}
