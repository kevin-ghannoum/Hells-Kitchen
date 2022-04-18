// Developed using : https://www.raywenderlich.com/82-procedural-generation-of-mazes-with-unity

using System.Linq;
using Common.Enums;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dungeon_Generation
{
    public class MazeConstructor : MonoBehaviour
    {
        [Header("Structure Prefabs")]
        [SerializeField] private GameObject floorPrefab;
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject windowPrefab;
        [SerializeField] private GameObject pillarPrefab;
        [SerializeField] private GameObject torchPrefab;
        [SerializeField] private GameObject chestPrefab;
        [SerializeField] private GameObject exitPrefab;
        [SerializeField] private GameObject[] rockPrefabs;
        [SerializeField] private GameObject[] debrisPrefabs;

        [Header("Enemy Prefabs")]
        [SerializeField] private GameObject[] enemies;

        [Header("Spawn Rates")]
        [SerializeField][Range(0f, 1f)] private float torchSpawnRate = 0.1f;
        [SerializeField][Range(0f, 1f)] private float windowSpawnRate = 0.01f;
        [SerializeField][Range(0f, 1f)] private float enemySpawnRate = 0.02f;
        [SerializeField][Range(0f, 1f)] private float chestSpawnRate = 0.01f;
        [SerializeField][Range(0f, 1f)] private float rockSpawnRate = 0.05f;
        [SerializeField][Range(0f, 1f)] private float debrisSpawnRate = 0.1f;
        
        [Header("Generation Settings")]
        [SerializeField][Range(0f, 1f)] private float placementThreshold = 0.1f;
        [SerializeField] private int rockMinNum = 3;
        [SerializeField] private int rockMaxNum = 7;
        
        [SerializeField] private int debrisMinNum = 1;
        [SerializeField] private int debrisMaxNum = 3;
        
        [SerializeField] private int minMazeSize = 20;
        [SerializeField] private int maxMazeSize = 30;
        
        [SerializeField] public float hallwayWidth = 5.0f;
        [SerializeField] public float hallwayHeight = 3.0f;

        private MazeDataGenerator _dataGenerator;

        private Transform mazeStart;
        private bool startPlaced;

        public int StartRow { get; private set; }
        public int StartCol { get; private set; }

        public int[,] Data { get; private set; }

        void Awake()
        {
            _dataGenerator = new MazeDataGenerator(placementThreshold);

            // default to walls surrounding a single empty cell
            Data = new [,] {
                {1, 1, 1},
                {1, 0, 1},
                {1, 1, 1}
            };
        }
        
        public void GenerateNewMaze(Transform mazeStart)
        {
            DisposeOldMaze();
            
            Data = _dataGenerator.FromDimensions(
                GetRandomOddNumberInRange(minMazeSize, maxMazeSize), 
                GetRandomOddNumberInRange(minMazeSize, maxMazeSize)
            );
            
            this.mazeStart = mazeStart;
            GenerateMazeFromData(Data);
        }

        private void GenerateMazeFromData(int[,] data)
        {
            GameObject parent = new GameObject();
            parent.transform.position = Vector3.zero;
            parent.name = "Procedural Maze";
            parent.tag = Tags.Generated;
            Vector3 lastfloorPosition =  Vector3.zero;

            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    if (data[i, j] == 0)
                    {
                        // Floor position
                        Vector3 floorPosition = new Vector3(i * hallwayWidth - (hallwayWidth / 2), 0, j * hallwayWidth - (hallwayWidth / 2));
                        lastfloorPosition = floorPosition;
            
                        // Place Start Point 
                        if(!startPlaced)
                            PlaceStartPosition(parent.transform, floorPosition);
                        
                        // Spawn everything
                        SpawnFloor(parent.transform, floorPosition);
                        SpawnEnemy(floorPosition);
                        if (!SpawnChest(parent.transform, floorPosition))
                            SpawnDebris(parent.transform, floorPosition);
                        
                        SpawnRocks(parent.transform, floorPosition);
                        SpawnWalls(parent.transform, floorPosition, i, j);
                    }
                }
            }
            
            // End portal
            PlaceGoal(parent.transform, lastfloorPosition);
            
            // Spawn pillars
            SpawnPillars(parent.transform, data);
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
            }

            return result;
        }

        private void PlaceStartPosition(Transform parent, Vector3 position)
        {
            startPlaced = true;
            mazeStart.parent = parent;
            mazeStart.localPosition = position;
        }

        public void DisposeOldMaze()
        {
            startPlaced = false;
            GameObject[] objects = GameObject.FindGameObjectsWithTag(Tags.Generated);
            foreach (GameObject obj in objects)
            {
                Destroy(obj);
            }
        }

        private void PlaceGoal(Transform parent, Vector3 position)
        {
            GameObject obj = Instantiate(exitPrefab , parent);
            obj.transform.localPosition = position;
            obj.transform.rotation = RandomRotation();
            obj.name = "Exit";
            obj.tag = Tags.Generated;

            obj.GetComponent<BoxCollider>().isTrigger = true;
        }

        private void SpawnFloor(Transform parent, Vector3 position)
        {
            GameObject floor = Instantiate(floorPrefab, parent);
            floor.transform.localPosition = position;
            floor.transform.localScale = new Vector3(hallwayWidth, 1, hallwayWidth);
        }

        private void SpawnEnemy(Vector3 position)
        {
            if (Random.value < enemySpawnRate)
            {
                var enemy = enemies[Random.Range(0, enemies.Length)];
                PhotonNetwork.Instantiate(nameof(enemy), position, Quaternion.identity);
            }
        }

        private bool SpawnChest(Transform parent, Vector3 position)
        {
            if (Random.value < chestSpawnRate)
            {
                GameObject chest = Instantiate(chestPrefab, parent);
                chest.transform.position = position + RandomTileOffset();
                chest.transform.rotation = Quaternion.Euler(0, Random.Range(-30, 30) - 135, 0);
                return true;
            }
            return false;
        }

        private void SpawnDebris(Transform parent, Vector3 position)
        {
            if (Random.value < debrisSpawnRate)
            {
                int numDebris = Random.Range(debrisMinNum, debrisMaxNum);
                for (int d = 0; d < numDebris; d++)
                {
                    GameObject debris = Instantiate(debrisPrefabs[Random.Range(0, debrisPrefabs.Length)], parent);
                    debris.transform.position = position + RandomTileOffset() + new Vector3(0, 0.1f, 0);
                    debris.transform.rotation = RandomRotation();
                }
            }
        }

        private void SpawnRocks(Transform parent, Vector3 position)
        {
            if (Random.value < rockSpawnRate)
            {
                int numRocks = Random.Range(rockMinNum, rockMaxNum + 1);
                for (int r = 0; r < numRocks; r++)
                {
                    GameObject rock = Instantiate(rockPrefabs[Random.Range(0, rockPrefabs.Length)], parent);
                    rock.transform.position = position + RandomTileOffset() + new Vector3(0, 0.1f, 0);
                    rock.transform.rotation = RandomRotation();
                }
            }
        }

        private void SpawnWalls(Transform parent, Vector3 position, int x, int y)
        {
            Vector3?[] neighbours = GetEmptyNeighbours(x, y);
            for (int n = 0; n < neighbours.Length; n++)
            {
                // Skip nulls
                if (neighbours[n] == null)
                    continue;
                            
                // Spawn wall
                Vector3 wallPosition = ((Vector3)neighbours[n] + position) / 2;
                GameObject wall = Instantiate(wallPrefab, parent);
                wall.transform.localPosition = wallPosition;
                wall.transform.localScale = new Vector3(1, hallwayHeight, hallwayWidth);
                wall.transform.localRotation = Quaternion.Euler(0, 90 * n, 0);
                            
                // Spawn Wall Decorations
                if (!SpawnTorch(wall.transform))
                    SpawnWindow(wall.transform);
            }
        }

        private bool SpawnTorch(Transform wall)
        {
            if (Random.value < torchSpawnRate)
            {
                const float torchHeight = 3.0f / 5.0f;
                GameObject torch = Instantiate(torchPrefab, wall);
                torch.transform.localPosition = new Vector3(0, torchHeight, Random.Range(-0.5f, 0.5f));
                torch.transform.localScale = new Vector3(1, 1 / hallwayHeight, 1 / hallwayWidth);
                return true;
            }
            return false;
        }

        private void SpawnWindow(Transform wall)
        {
            if (Random.value < windowSpawnRate)
            {
                const float windowHeight = 4.0f / 7.0f;
                GameObject torch = Instantiate(windowPrefab, wall);
                torch.transform.localPosition = new Vector3(0, windowHeight, Random.Range(-0.1f, 0.1f));
                torch.transform.localScale = new Vector3(1, 1 / hallwayHeight, 1 / hallwayWidth);
            }
        }

        private void SpawnPillars(Transform parent, int[,] data)
        {
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    int[,] offsets = {
                        {0, 0},
                        {0, 1},
                        {1, 0},
                        {1, 1}
                    };
                    int[] floors = Enumerable.Range(0, offsets.GetLength(0))
                        .Select(o => {
                            int dx = offsets[o, 0];
                            int dy = offsets[o, 1];
                            if (i + dx >= 0 && i + dx < Data.GetLength(0) &&
                                j + dy >= 0 && j + dy < Data.GetLength(1))
                                return Data[i + dx, j + dy];
                            return 1;
                        }).ToArray();
                    int numFloors = floors.Aggregate(0, (acc, v) => acc + (v == 0 ? 1 : 0));
                    if (numFloors % 2 == 1 || (numFloors == 2 && floors[0] == floors[3]))
                    {
                        GameObject pillar = Instantiate(pillarPrefab, parent.transform);
                        pillar.transform.localPosition = new Vector3(i * hallwayWidth, 0, j * hallwayWidth);
                        float scale = 1.2f * hallwayHeight / 5.0f;
                        pillar.transform.localScale = new Vector3(scale, scale, scale);
                    }
                }
            }
        }

        private Vector3 RandomTileOffset()
        {
            return new Vector3(Random.Range(-hallwayWidth / 3, hallwayWidth / 3), 0, Random.Range(-hallwayWidth / 3, hallwayWidth / 3));
        }

        private Quaternion RandomRotation()
        {
            return Quaternion.Euler(0, Random.Range(0, 360), 0);
        }

        private int GetRandomOddNumberInRange(int low, int high)
        {
            int num = Random.Range(low, high);
            if (num % 2 == 0) num += 1;
            return num;
        }
        
    }
}
