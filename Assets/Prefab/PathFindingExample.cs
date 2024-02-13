using System;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingExample : MonoBehaviour
{
    public int startx, starty;
    private void Start()
    {
        Main();
    }
    void Main()
    {
        // Define the grid size
        int gridSize = 5;

        // Create a 2D array representing the grid
        int[,] grid = new int[gridSize, gridSize];


        // Perform pathfinding
        TraverseGrid(grid, startx, starty);

        // Display the resulting grid
        PrintGrid(grid);
    }

    void TraverseGrid(int[,] grid, int startX, int startY)
    {
        Queue<(int, int)> queue = new Queue<(int, int)>();
        queue.Enqueue((startX, startY));

        int stepSize = 1;

        while (queue.Count > 0)
        {
            int count = queue.Count;

            for (int i = 0; i < count; i++)
            {
                var (x, y) = queue.Dequeue();
                grid[x, y] = stepSize;

                // Move horizontally
                if (x + 1 < grid.GetLength(0) && grid[x + 1, y] == 0)
                    queue.Enqueue((x + 1, y));

                // Move vertically
                if (y + 1 < grid.GetLength(1) && grid[x, y + 1] == 0)
                    queue.Enqueue((x, y + 1));
            }

            stepSize++;
        }
    }

    void PrintGrid(int[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Debug.Log(grid[i, j]);
            }
            Debug.Log("=======");

        }
    }
}

