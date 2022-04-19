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
            return GetLocalPlayerObject().GetComponent<PlayerController>();
        }
        
        public static PhotonView GetLocalPlayerPhotonView()
        {
            return GetLocalPlayerObject().GetComponent<PhotonView>();
        }
        
        public static GameObject GetLocalPlayerObject()
        {
            var players =  GameObject.FindGameObjectsWithTag(Tags.Player);
            foreach (var player in players)
            {
                if (player.GetComponent<PhotonView>().IsMine)
                    return player;
            }

            return null;
        }
    }
}
