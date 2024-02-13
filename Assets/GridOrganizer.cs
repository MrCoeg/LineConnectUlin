using System;
using UnityEngine;

public class GridOrganizer : MonoBehaviour
{
    public int rows = 4;
    public int cols = 4;
    public int maxValue = 2;
    public Vector2Int[] target;
    private int[,] grid;
    public Vector2Int[] oTarger;
    public int counter;
    void Start()
    {

    }

    private void Update()
    {
        target = new Vector2Int[maxValue];

        grid = GeneratePairedGrid(rows, cols, maxValue);
        FillRemainingGridRandomly();
        MoveCellToTarget();
        counter++;
        
    }

    int[,] GeneratePairedGrid(int rows, int cols, int maxValue)
    {
        int[,] newGrid = new int[rows, cols];
        for (int i = 1; i <= maxValue; i++)
        {
            var counter = 1;
            do
            {
                var row = UnityEngine.Random.Range(1, rows );
                var col = UnityEngine.Random.Range(1, cols);

                if (newGrid[row, col] == 0)
                {
                    newGrid[row, col] = i;
                    target[i - 1] = new Vector2Int(col, row);
                    counter++;
                }
            } while (counter <= 1);

        }
        oTarger = target;
        return newGrid;

    }

    void PlacePair(int[,] gridToFill, int pairValue)
    {
        int rows = gridToFill.GetLength(0);
        int cols = gridToFill.GetLength(1);

        for (int count = 0; count < 2; count++)
        {
            int row, col;

            do
            {
                row = UnityEngine.Random.Range(0, rows);
                col = UnityEngine.Random.Range(0, cols);
            } while (gridToFill[row, col] != 0);

            gridToFill[row, col] = pairValue;
        }
    }

    void FillRemainingGridRandomly()
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (grid[i, j] == 0)
                {
                    // Fill remaining empty cells with random values
                    grid[i, j] = UnityEngine.Random.Range(1, maxValue + 1);
                }
            }
        }
    }

    void move()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (grid[i, j] == 0)
                {
                    // Fill remaining empty cells with random values
                    grid[i, j] = UnityEngine.Random.Range(1, maxValue + 1);
                }
            }
        }
    }

    void PrintGrid(int[,] gridToPrint)
    {
        for (int i = 0; i < gridToPrint.GetLength(0); i++)
        {
            for (int j = 0; j < gridToPrint.GetLength(1); j++)
            {
                Debug.Log(gridToPrint[i, j] + " ");
            }
            Debug.Log("");
        }
    }
    void MoveCellToTarget()
    {
        for (int i = 1; i <= maxValue; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                for (int k = 0; k < cols; k++)
                {
                    int[] directions = { -1, 1 };
                    if (grid[j,k] != i && (j + 1) + k == (target[i - 1].y + 1) + target[i - 1].x)
                    {
                        continue;
                    }

                    Tuple<int, int>[] coordinate =
                    {
                        Tuple.Create(0,1),
                        Tuple.Create(1,0),
                        Tuple.Create(0,-1),
                        Tuple.Create(-1,0)

                    };
                    var moved = false; 
                    foreach (var item in coordinate)
                    {
                        if (moved)
                        {
                            continue;
                        }
                        int newRow = target[i - 1].y + item.Item2;
                        int newCol = target[i - 1].x + item.Item1;
                        if (!IsValidCell(newRow, newCol, rows, cols))
                        {
                            continue;
                        }
                        var val = grid[newRow, newCol];
                        var targetVa = grid[target[i - 1].y, target[i - 1].x];
                        if (val != targetVa)
                        {

                            if (IsValidCell(newRow, newCol, rows, cols))
                            {


                                SwapCells(j, k, newRow, newCol);
                                target[i - 1] = new Vector2Int(newCol, newRow);
                                moved = true;
                            }
                        }
                    }


                       
                }
            }
        }
    
    }

    void SwapCells(int row1, int col1, int row2, int col2)
    {
        int temp = grid[row1, col1];
        grid[row1, col1] = grid[row2, col2];
        grid[row2, col2] = temp;
    }

    bool IsValidCell(int row, int col, int maxRows, int maxCols)
    {
        return row >= 0 && row < maxRows && col >= 0 && col < maxCols;
    }

}
