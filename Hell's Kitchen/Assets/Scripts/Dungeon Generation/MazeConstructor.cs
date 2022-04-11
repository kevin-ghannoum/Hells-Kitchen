// Developed using : https://www.raywenderlich.com/82-procedural-generation-of-mazes-with-unity

using System;
using System.Collections.Generic;
using Common.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dungeon_Generation
{
    public class MazeConstructor : MonoBehaviour
    {
        [SerializeField] private bool showDebug = false;

        [Header("Prefabs")]
        [SerializeField] private GameObject floorPrefab;
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject torchPrefab;
        
        [Header("Generation Settings")]
        [SerializeField][Range(0f, 1f)] private float placementThreshold = 0.1f;
        [SerializeField][Range(0f, 1f)] private float torchAbundance = 0.1f;
        [SerializeField] private int minMazeSize = 20;
        [SerializeField] private int maxMazeSize = 30;
        [SerializeField] public float hallwayWidth = 5.0f;
        [SerializeField] public float hallwayHeight = 3.0f;

        private MazeDataGenerator dataGenerator;

        public int StartRow { get; private set; }
        public int StartCol { get; private set; }

        public int GoalRow { get; private set; }
        public int GoalCol { get; private set; }

        public int[,] Data { get; private set; }

        void Awake()
        {
            dataGenerator = new MazeDataGenerator(placementThreshold);

            // default to walls surrounding a single empty cell
            Data = new int[,] {
                {1, 1, 1},
                {1, 0, 1},
                {1, 1, 1}
            };
        }
        
        public void GenerateNewMaze(TriggerEventHandler startCallback = null, TriggerEventHandler goalCallback = null)
        {
            DisposeOldMaze();

            Data = dataGenerator.FromDimensions(
                GetRandomOddNumberInRange(minMazeSize, maxMazeSize), 
                GetRandomOddNumberInRange(minMazeSize, maxMazeSize)
            );

            FindStartPosition();
            FindGoalPosition();

            GenerateMazeFromData(Data);

            PlaceStartTrigger(startCallback);
            PlaceGoalTrigger(goalCallback);
        }

        private void GenerateMazeFromData(int[,] data)
        {
            GameObject parent = new GameObject();
            parent.transform.position = Vector3.zero;
            parent.name = "Procedural Maze";
            parent.tag = Tags.Generated;
            
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    if (data[i, j] == 0)
                    {
                        Vector3 floorPosition = new Vector3(i * hallwayWidth - (hallwayWidth / 2), 0, j * hallwayWidth - (hallwayWidth / 2));
                        GameObject floor = Instantiate(floorPrefab, parent.transform);
                        floor.transform.localPosition = floorPosition;
                        floor.transform.localScale = new Vector3(hallwayWidth, 1, hallwayWidth);

                        Vector3?[] neighbours = GetEmptyNeighbours(i, j);
                        for (int n = 0; n < neighbours.Length; n++)
                        {
                            if (neighbours[n] != null)
                            {
                                Vector3 wallPosition = ((Vector3)neighbours[n] + floorPosition) / 2;
                                GameObject wall = Instantiate(wallPrefab, parent.transform);
                                wall.transform.localPosition = wallPosition;
                                wall.transform.localScale = new Vector3(1, hallwayHeight, hallwayWidth);
                                wall.transform.localRotation = Quaternion.Euler(0, 90 * n, 0);

                                if (Random.value < torchAbundance)
                                {
                                    const float torchHeight = 3.0f / 5.0f;
                                    GameObject torch = Instantiate(torchPrefab, wall.transform);
                                    torch.transform.localPosition = new Vector3(0, torchHeight, Random.Range(-0.5f, 0.5f));
                                    torch.transform.localScale = new Vector3(1, 1 / hallwayHeight, 1 / hallwayWidth);
                                }
                            }
                        }
                    }
                }
            }
        }

        private Vector3?[] GetEmptyNeighbours(int x, int y)
        {
            int[,] offsets = {
                {1, 0},
                {0, -1},
                {-1, 0},
                {0, 1}
            };

            Vector3?[] result = new Vector3?[4];
            for (int i = 0; i < offsets.GetLength(0); i++)
            {
                int nx = x + offsets[i, 0];
                int ny = y + offsets[i, 1];
                if (nx >= 0 && nx < Data.GetLength(0) && ny >= 0 && ny < Data.GetLength(1) && Data[nx, ny] == 1)
                {
                    result[i] = new Vector3(nx * hallwayWidth - (hallwayWidth / 2), 0, ny * hallwayWidth - (hallwayWidth / 2));
                }
                else
                {
                    result[i] = null;
                }
            }

            return result;
        }

        void OnGUI()
        {
            if (!showDebug)
            {
                return;
            }

            int[,] maze = Data;
            int rowMax = maze.GetUpperBound(0);
            int colMax = maze.GetUpperBound(1);

            string drawMaze = "";

            for (int i = rowMax; i >= 0; i--)
            {
                for (int j = 0; j <= colMax; j++)
                {
                    if (maze[i, j] == 0)
                    {
                        drawMaze += "....";
                    }
                    else
                    {
                        drawMaze += "==";
                    }
                }
                drawMaze += "\n";
            }

            GUI.Label(new Rect(20, 20, 500, 500), drawMaze);
        }

        private void FindStartPosition()
        {
            int[,] maze = Data;
            int rowMax = maze.GetUpperBound(0);
            int colMax = maze.GetUpperBound(1);

            for (int i = 0; i <= rowMax; i++)
            {
                for (int j = 0; j <= colMax; j++)
                {
                    if (maze[i, j] == 0)
                    {
                        StartRow = i;
                        StartCol = j;
                        return;
                    }
                }
            }
        }

        private void FindGoalPosition()
        {
            int[,] maze = Data;
            int rowMax = maze.GetUpperBound(0);
            int colMax = maze.GetUpperBound(1);

            // loop top to bottom, right to left
            for (int i = rowMax; i >= 0; i--)
            {
                for (int j = colMax; j >= 0; j--)
                {
                    if (maze[i, j] == 0)
                    {
                        GoalRow = i;
                        GoalCol = j;
                        return;
                    }
                }
            }
        }

        public void DisposeOldMaze()
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(Tags.Generated);
            foreach (GameObject obj in objects)
            {
                Destroy(obj);
            }
        }

        private void PlaceStartTrigger(TriggerEventHandler callback)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.position = new Vector3(StartCol, .5f, StartRow);
            obj.name = "Start Trigger";
            obj.tag = Tags.Generated;

            obj.GetComponent<BoxCollider>().isTrigger = true;
            obj.GetComponent<MeshRenderer>().enabled = false;

            TriggerEventRouter trigger = obj.AddComponent<TriggerEventRouter>();
            trigger.callback = callback;
        }

        private void PlaceGoalTrigger(TriggerEventHandler callback)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.position = new Vector3(GoalCol, .5f, GoalRow);
            obj.name = "Exit";
            obj.tag = Tags.Generated;

            obj.GetComponent<BoxCollider>().isTrigger = true;
            obj.GetComponent<MeshRenderer>().enabled = false;

            TriggerEventRouter trigger = obj.AddComponent<TriggerEventRouter>();
            trigger.callback = callback;
        }
        
        private int GetRandomOddNumberInRange(int low, int high)
        {
            int num = UnityEngine.Random.Range(low, high);
            if (num % 2 == 0) num += 1;
            return num;
        }
        
    }
}
