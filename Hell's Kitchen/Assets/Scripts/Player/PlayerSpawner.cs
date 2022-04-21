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
        [SerializeField] private bool shouldSpawnOnAwake = false;
        
        private void Awake()
        {
            if (!shouldSpawnOnAwake)
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
            GameStateManager.Instance.inventoryUI.UpdateInventory(GameStateData.inventory.GetInventoryItems());
            
            if (SceneManager.GetActiveScene().name.Equals(Scenes.Dungeon))
            {
                player.transform.Find("Lights").gameObject.SetActive(true);
            }
            else
            {
                //GameStateData.playerCurrentHitPoints = GameStateData.playerMaxHitPoints;
                player.GetComponent<Player.PlayerHealth>().internalHealth = GameStateData.playerMaxHitPoints;
            }
        }

        public void SpawnSousChefInScene()
        {
            // only spawn sous-chef in dungeon
            if (SceneManager.GetActiveScene().name.Equals(Scenes.Dungeon))
            {
                var prefab = GameStateData.sousChefType == SousChefType.Healer ? healerPrefab : knightPrefab;
                GameObject sousChef = PhotonNetwork.Instantiate(prefab.name, spawnPoints[0].position - new Vector3(2f, 0, 2f), Quaternion.identity);
                sousChef.tag = Tags.SousChef;
            }
        }
    }
}
