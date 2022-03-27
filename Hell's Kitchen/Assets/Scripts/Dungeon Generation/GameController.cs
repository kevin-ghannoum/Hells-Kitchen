// Developed using : https://www.raywenderlich.com/82-procedural-generation-of-mazes-with-unity
using UnityEngine;
using Player;

namespace Dungeon_Generation
{
    [RequireComponent(typeof(MazeConstructor))]

    public class GameController : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private float playerHeightPosition = 0f;

        private MazeConstructor generator;
        
        private bool goalReached;
        
        void Start() {
            generator = GetComponent<MazeConstructor>();
            StartNewGame();
        }
        
        private void StartNewGame()
        {
            StartNewMaze();
        }
        
        private void StartNewMaze()
        {
            generator.GenerateNewMaze(GetRandomOddNumberInRange(9, 15), GetRandomOddNumberInRange(7, 13), OnStartTrigger, OnGoalTrigger);

            float x = generator.StartCol * generator.HallWidth;
            float y = playerHeightPosition;
            float z = generator.StartRow * generator.HallWidth;
            player.transform.position = new Vector3(x, y, z);

            goalReached = false;
            player.enabled = true;
        }
 
        private void OnGoalTrigger(GameObject trigger, GameObject other)
        {
            Debug.Log("Reached maze goal");
            goalReached = true;

            Destroy(trigger);
        }

        private void OnStartTrigger(GameObject trigger, GameObject other)
        {
            if (goalReached)
            {
                // currently just restarts the maze but we can add behavior here for when we enter the dungeon
                player.enabled = false;
                Invoke(nameof(StartNewMaze), 4);
            }
        }

        private int GetRandomOddNumberInRange(int low, int high)
        {
            int num = UnityEngine.Random.Range(low, high);
            if (num % 2 == 0) num += 1;
            return num;
        }
    }
}

