// Developed using : https://www.raywenderlich.com/82-procedural-generation-of-mazes-with-unity

using UnityEngine;

namespace Dungeon_Generation
{
    public class MazeDataGenerator
    {
        private float placementThreshold;    // chance of empty space

        public MazeDataGenerator()
        {
            placementThreshold = .1f;                              
        }

        public int[,] FromDimensions(int sizeRows, int sizeCols) 
        {
            int[,] maze = new int[sizeRows, sizeCols];
            int rowMax = maze.GetUpperBound(0);
            int colMax = maze.GetUpperBound(1);
            for (int i = 0; i <= rowMax; i++)
            {
                for (int j = 0; j <= colMax; j++)
                {
                    if (i == 0 || j == 0 || i == rowMax || j == colMax)
                    {
                        maze[i, j] = 1;
                    }
                    else if (i % 2 == 0 && j % 2 == 0)
                    {
                        if (Random.value > placementThreshold)
                        {
                            maze[i, j] = 1;

                            int a = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                            int b = a != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                            maze[i+a, j+b] = 1;
                        }
                    }
                }
            }
            return maze;
        }
    }
}

