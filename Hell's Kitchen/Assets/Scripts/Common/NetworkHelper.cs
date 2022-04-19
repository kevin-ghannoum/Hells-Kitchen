using Common.Enums;
using Photon.Pun;
using Player;
using UnityEngine;

namespace Common
{
    public static class NetworkHelper
    {
        public static PlayerController GetLocalPlayerController()
        {
           var players =  GameObject.FindGameObjectsWithTag(Tags.Player);
           foreach (var player in players)
           {
               if (player.GetComponent<PhotonView>().IsMine)
                   return player.GetComponent<PlayerController>();
           }

           return null;
        }
    }
}