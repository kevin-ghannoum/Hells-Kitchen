// Developed using : https://www.raywenderlich.com/82-procedural-generation-of-mazes-with-unity

using Common.Enums;
using UnityEngine;

namespace Dungeon_Generation
{
    public class MazeConstructor : MonoBehaviour
    {
        public bool showDebug;

        [Header("Materials")]
        [SerializeField] private Material mazeMat1;
        [SerializeField] private Material mazeMat2;
        [SerializeField] private Material startMat;
        [SerializeField] private Material treasureMat;
        
        [Header("Generation Settings")]
        [SerializeField] private float hallHeight = 5f;
        [SerializeField] private float hallWidth = 5f;
        [SerializeField][Range (0f, 1f)] private float chanceOfEmptySpace = 0.1f;
        
        private MazeDataGenerator dataGenerator;
        private MazeMeshGenerator meshGenerator;
        
        public float HallWidth { get=> hallWidth; set => hallWidth = value; }
        public float HallHeight { get => hallHeight; set => hallHeight = value; }

        public int StartRow { get; private set; }
        public int StartCol { get; private set; }

        public int GoalRow { get; private set; }
        public int GoalCol { get; private set; }
        
        public int[,] Data { get; private set; }
        
        void Awake()
        {
            dataGenerator = new MazeDataGenerator(chanceOfEmptySpace);
            meshGenerator = new MazeMeshGenerator(hallHeight, hallWidth);

            // default to walls surrounding a single empty cell
            Data = new int[,]
            {
                {1, 1, 1},
                {1, 0, 1},
                {1, 1, 1}
            };
        }
    
        public void GenerateNewMaze(int sizeRows, int sizeCols,
            TriggerEventHandler startCallback=null, TriggerEventHandler goalCallback=null)
        {
            if (sizeRows % 2 == 0 && sizeCols % 2 == 0)
            {
                Debug.LogError("Odd numbers work better for dungeon size.");
            }

            DisposeOldMaze();

            Data = dataGenerator.FromDimensions(sizeRows, sizeCols);

            FindStartPosition();
            FindGoalPosition();

            // store values used to generate this mesh
            HallWidth = meshGenerator.width;
            HallHeight = meshGenerator.height;

            DisplayMaze();

            PlaceStartTrigger(startCallback);
            PlaceGoalTrigger(goalCallback);
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
    
        private void DisplayMaze()
        {
            GameObject obj = new GameObject();
            obj.transform.position = Vector3.zero;
            obj.name = "Procedural Maze";
            obj.tag = Tags.Generated;

            MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
            meshFilter.mesh = meshGenerator.FromData(Data);
    
            MeshCollider meshCollider = obj.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = meshFilter.mesh;

            MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
            meshRenderer.materials = new Material[2] {mazeMat1, mazeMat2};
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
            foreach (GameObject obj in objects) {
                Destroy(obj);
            }
        }
        
        private void PlaceStartTrigger(TriggerEventHandler callback)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.position = new Vector3(StartCol * HallWidth, .5f, StartRow * HallWidth);
            obj.name = "Start Trigger";
            obj.tag = Tags.Generated;

            obj.GetComponent<BoxCollider>().isTrigger = true;
            obj.GetComponent<MeshRenderer>().sharedMaterial = startMat;

            TriggerEventRouter trigger = obj.AddComponent<TriggerEventRouter>();
            trigger.callback = callback;
        }

        private void PlaceGoalTrigger(TriggerEventHandler callback)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.position = new Vector3(GoalCol * HallWidth, .5f, GoalRow * HallWidth);
            obj.name = "Exit";
            obj.tag = Tags.Generated;

            obj.GetComponent<BoxCollider>().isTrigger = true;
            obj.GetComponent<MeshRenderer>().sharedMaterial = treasureMat;

            TriggerEventRouter trigger = obj.AddComponent<TriggerEventRouter>();
            trigger.callback = callback;
        }


    }
}