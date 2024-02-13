using UnityEngine;

public class GridMover : MonoBehaviour
{
    private int[,] grid;

    void Start()
    {

        InitializeGrid();
        Debug.Log("Original Grid:");
        PrintGrid(grid);

        MoveCellToTarget(0, 0, 4, 4);

        Debug.Log("Moved Grid:");
        PrintGrid(grid);
    }

    private void Update()
    {
        
    }

    void InitializeGrid()
    {
        // Initialize a 5x5 grid with random values
        grid = new int[5, 5];
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                grid[i, j] = Random.Range(1, 10);
            }
        }
    }

    void MoveCellToTarget(int cellRow, int cellCol, int targetRow, int targetCol)
    {
        // Ensure the target position is within bounds
        if (IsValidCell(targetRow, targetCol, 5, 5))
        {
            int[] directions = { -1, 1 }; // Up, Down, Left, Right

            foreach (int dirRow in directions)
            {
                foreach (int dirCol in directions)
                {
                    int newRow = targetRow + dirRow;
                    int newCol = targetCol + dirCol;

                    if (IsValidCell(newRow, newCol, 5, 5))
                    {
                        SwapCells(cellRow, cellCol, newRow, newCol);
                        Debug.Log($"\nCell moved from ({cellRow}, {cellCol}) to ({newRow}, {newCol}).");
                        return; // Cell moved successfully
                    }
                }
            }

            Debug.LogError("No valid adjacent position found to move the cell.");
        }
        else
        {
            Debug.LogError("Target position is out of bounds.");
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

    void PrintGrid(int[,] gridToPrint)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Debug.Log(gridToPrint[i, j] + " ");
            }
            Debug.Log("");
        }
    }
}
