// Developed using : https://www.raywenderlich.com/82-procedural-generation-of-mazes-with-unity

using System.Linq;
using Common;
using Common.Enums;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Server;
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
        [SerializeField] private GameObject[] enemyPrefabs;

        [Header("Beehive Spawning")]
        [SerializeField] private GameObject beehivePrefab;
        [SerializeField] private GameObject beehiveEnemyPrefab;
        
        [Header("Spawn Rates")]
        [SerializeField][Range(0f, 1f)] private float torchSpawnRate = 0.1f;
        [SerializeField][Range(0f, 1f)] private float windowSpawnRate = 0.01f;
        [SerializeField][Range(0f, 1f)] private float enemySpawnRate = 0.02f;
        [SerializeField][Range(0f, 1f)] private float chestSpawnRate = 0.01f;
        [SerializeField][Range(0f, 1f)] private float rockSpawnRate = 0.05f;
        [SerializeField][Range(0f, 1f)] private float debrisSpawnRate = 0.1f;
        [SerializeField][Range(0f, 1f)] private float hiveSpawnRate = 0.3f;

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

        [SerializeField] private PhotonView photonView;

        private MazeDataGenerator _dataGenerator;

        private bool _startPlaced;

        public int[,] Data { get; private set; }

        private void Awake()
        {
            _dataGenerator = new MazeDataGenerator(placementThreshold);
            enemySpawnRate += 0.025f * GameStateData.hiddenLevel;

            // default to walls surrounding a single empty cell
            Data = new[,] {
                {1, 1, 1},
                {1, 0, 1},
                {1, 1, 1}
            };
        }

        public void GenerateNewMaze()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Data = _dataGenerator.FromDimensions(
                    GetRandomOddNumberInRange(minMazeSize, maxMazeSize),
                    GetRandomOddNumberInRange(minMazeSize, maxMazeSize)
                );
                photonView.RPC(nameof(GenerateMazeRPC), RpcTarget.All, Serializer.SerializeInt2D(Data));
            }
        }

        [PunRPC]
        private void GenerateMazeRPC(byte[] bytes)
        {
            DisposeOldMaze();

            Data = Serializer.DeserializeInt2D(bytes);
            GenerateMaze();
            
            GameObject.FindWithTag(Tags.Pathfinding).GetComponent<Pathfinding>().Bake(true);
        }

        private void GenerateMaze()
        {
            GameObject parent = new GameObject();
            parent.transform.position = Vector3.zero;
            parent.name = "Procedural Maze";
            parent.tag = Tags.Generated;
            Vector3 lastfloorPosition = Vector3.zero;

            for (int i = 0; i < Data.GetLength(0); i++)
            {
                for (int j = 0; j < Data.GetLength(1); j++)
                {
                    if (Data[i, j] == 0)
                    {
                        // Floor position
                        Vector3 floorPosition = new Vector3(i * hallwayWidth - (hallwayWidth / 2), 0, j * hallwayWidth - (hallwayWidth / 2));
                        lastfloorPosition = floorPosition;

                        // Spawn everything
                        SpawnFloor(parent.transform, floorPosition);
                        SpawnRocks(parent.transform, floorPosition);
                        SpawnWalls(parent.transform, floorPosition, i, j);
                        
                        // Spawn shared game objects
                        if (PhotonNetwork.IsMasterClient)
                        {
                            if (_startPlaced) 
                                SpawnEnemy(floorPosition);
                            if (!SpawnChest(floorPosition))
                                SpawnDebris(floorPosition);
                        }
                        
                        // Place Start Point 
                        if (!_startPlaced)
                            PlaceStartPosition(parent.transform, floorPosition);
                    }
                }
            }

            // End portal
            if (PhotonNetwork.IsMasterClient)
            {
                SpawnBeehives();
                PlaceGoal(lastfloorPosition);
            }

            // Spawn pillars
            SpawnPillars(parent.transform);
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
            _startPlaced = true;
            var gameController = GetComponent<GameController>();
            gameController.mazeStart.parent = parent;
            gameController.mazeStart.localPosition = position;
        }

        private void DisposeOldMaze()
        {
            _startPlaced = false;
            GameObject[] objects = GameObject.FindGameObjectsWithTag(Tags.Generated);
            foreach (GameObject obj in objects)
            {
                Destroy(obj);
            }

            if (PhotonNetwork.IsMasterClient)
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.Enemy);
                foreach (var enemy in enemies)
                {
                    PhotonNetwork.Destroy(enemy);
                }
                
                GameObject[] decorations = GameObject.FindGameObjectsWithTag(Tags.SharedDecoration);
                foreach (var decoration in decorations)
                {
                    PhotonNetwork.Destroy(decoration);
                }
            }
        }

        private void PlaceGoal(Vector3 position)
        {
            GameObject obj = PhotonNetwork.Instantiate(exitPrefab.name, position, RandomRotation());
            obj.name = "Exit";
            obj.tag = Tags.Generated;
        }

        private void SpawnFloor(Transform parent, Vector3 position)
        {
            GameObject floor = Instantiate(floorPrefab, parent);
            floor.transform.localPosition = position;
            floor.transform.localScale = new Vector3(hallwayWidth, 1, hallwayWidth);
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
                Vector3 wallPosition = ((Vector3) neighbours[n] + position) / 2;
                GameObject wall = Instantiate(wallPrefab, parent);
                wall.transform.localPosition = wallPosition;
                wall.transform.localScale = new Vector3(1, hallwayHeight, hallwayWidth);
                wall.transform.localRotation = Quaternion.Euler(0, 90 * n, 0);

                // Spawn Wall Decorations
                if (!SpawnTorch(wall.transform))
                    SpawnWindow(wall.transform);
            }
        }

        private void SpawnPillars(Transform parent)
        {
            for (int i = 0; i < Data.GetLength(0); i++)
            {
                for (int j = 0; j < Data.GetLength(1); j++)
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

        private void SpawnEnemy(Vector3 position)
        {
            if (Random.value < enemySpawnRate)
            {
                var enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                PhotonNetwork.Instantiate(enemy.name, position, Quaternion.identity);
            }
        }

        private bool SpawnChest(Vector3 position)
        {
            if (Random.value < chestSpawnRate)
            {
                PhotonNetwork.Instantiate(chestPrefab.name,
                    position + RandomTileOffset(),
                    Quaternion.Euler(0, Random.Range(-30, 30) - 135, 0)
                );
                return true;
            }
            return false;
        }

        private void SpawnDebris(Vector3 position)
        {
            if (Random.value < debrisSpawnRate)
            {
                int numDebris = Random.Range(debrisMinNum, debrisMaxNum);
                for (int d = 0; d < numDebris; d++)
                {
                    PhotonNetwork.Instantiate(debrisPrefabs[Random.Range(0, debrisPrefabs.Length)].name,
                        position + RandomTileOffset() + new Vector3(0, 0.1f, 0),
                        RandomRotation()
                    );
                }
            }
        }

        private bool SpawnTorch(Transform wall)
        {
            if (Random.value < torchSpawnRate)
            {
                float torchHeight = hallwayHeight * 3.0f / 5.0f;
                PhotonNetwork.Instantiate(torchPrefab.name,
                    wall.position + torchHeight * Vector3.up + wall.forward * hallwayWidth * Random.Range(-0.5f, 0.5f),
                    wall.rotation
                );
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

        private void SpawnBeehives()
        {
            for (int i = 0; i < Data.GetLength(0) - 2; i++)
            {
                for (int j = 0; j < Data.GetLength(1) - 2; j++)
                { 
                    int[,] offsets = {
                        {0, 0}, {0, 1}, {0, 2},
                        {1, 0}, {1, 1}, {1, 2},
                        {2, 0}, {2, 1}, {2, 2}
                    };
                    bool canSpawnHive = Enumerable.Range(0, offsets.GetLength(0))
                        .Select(index => Data[i + offsets[index, 0], j + offsets[index, 1]])
                        .All(d => d == 0);
                    if (canSpawnHive && Random.value < hiveSpawnRate)
                    {
                        Debug.Log($"PLACING HIVE {i} {j}");
                        var center = new Vector3(i * hallwayWidth + hallwayWidth / 2, 0, j * hallwayWidth + hallwayWidth / 2);
                        PhotonNetwork.Instantiate(beehivePrefab.name, center + RandomTileOffset(), Quaternion.identity);
                        var numBees = Random.Range(2, 5);
                        for (var b = 0; b < numBees; b++)
                        {
                            var offsetIndex = Random.Range(0, offsets.GetLength(0));
                            PhotonNetwork.Instantiate(beehiveEnemyPrefab.name, 
                                new Vector3(
                                    (i + offsets[offsetIndex, 0]) * hallwayWidth + hallwayWidth / 2, 0,
                                    (j + offsets[offsetIndex, 1]) * hallwayWidth + hallwayWidth / 2) + RandomTileOffset(), 
                                RandomRotation());
                        }
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
