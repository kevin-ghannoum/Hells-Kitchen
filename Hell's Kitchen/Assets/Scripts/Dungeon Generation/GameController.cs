// Developed using : https://www.raywenderlich.com/82-procedural-generation-of-mazes-with-unity
using System;
using Common.Enums;
using UnityEngine;
using Player;

namespace Dungeon_Generation
{
    [RequireComponent(typeof(MazeConstructor))]

    public class GameController : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private float playerHeightPosition = 0f;
        [SerializeField] private int minMazeSize = 20;
        [SerializeField] private int maxMazeSize = 30;

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
            generator.GenerateNewMaze(OnStartTrigger, OnGoalTrigger);

            float x = generator.StartCol * generator.hallwayWidth - (generator.hallwayWidth / 2);
            float y = playerHeightPosition;
            float z = generator.StartRow * generator.hallwayWidth - (generator.hallwayWidth / 2);
            player.transform.position = new Vector3(x, y, z);

            goalReached = false;
            player.enabled = true;

            GameObject.FindWithTag(Tags.Pathfinding).GetComponent<Pathfinding>().Bake(true);
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
        
    }
}

