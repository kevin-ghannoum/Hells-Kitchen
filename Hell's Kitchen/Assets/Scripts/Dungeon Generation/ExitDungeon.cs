using Common;
using Common.Enums;
using Input;
using UnityEngine;

namespace Dungeon_Generation
{
    public class ExitDungeon : MonoBehaviour
    {
        private InputManager _input => InputManager.Instance;
        
        private void OnTriggerStay(Collider other)
        {
            if(other.gameObject.CompareTag(Tags.Player) && _input.interact)
            {
                if(GameStateManager.Instance.dungeonTimeHasElapsed)
                    SceneManager.Instance.LoadDungeonScene();
                else
                    SceneManager.Instance.LoadDungeonScene();
            }
        }
    }
}
