using System;
using Common;
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
             PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);
        }
    }
}
