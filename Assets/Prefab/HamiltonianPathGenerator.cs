using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamiltonianPathGenerator : MonoBehaviour
{
    public int totalRows = 3;
    public int totalCols = 4;


    private Node[,] grid;
    private int totalNodes;
    private List<Node> hamiltonianPath;
    public double counter;
    public double limit;

    private bool find;
    public bool search = true;
    public int startRow;
    public int startCol;

    public int hc;
    public List<int> pathportions = new List<int>();

    public List< List<Node>> nodes = new List<List<Node>>();
    public int mover;

    public bool GeneratePath()
    {
        mover = Random.Range(0, 23);
        InitializeGrid();
        // Randomly select a starting node

        Node startNode = grid[Random.Range(0, totalRows), Random.Range(0, totalCols)];

        hamiltonianPath = FindHamiltonianPath(startNode);
        var x = totalCols * totalRows;

        for (int i = 0; i < hc; i++)
        {
            pathportions.Add(2);
            x -= pathportions[i];
        }
        for (int i = 0; i < hc - 1; i++)
        {
            var v = (Random.Range(0, x / 2));
            pathportions[i] += v;
            x -= v;
        }
        pathportions[pathportions.Count-1] += x;
        if (hamiltonianPath.Count > 0)
        {
            nodes.Add(new List<Node>());
            var nc = 0;
            var c = 0;
            for (int i = 0; i < hamiltonianPath.Count; i++)
            {


                if (c >= pathportions[nc])
                {
                    nodes.Add(new List<Node>());
                    nc++;
                    hamiltonianPath[i].PathID = nc;

                    nodes[nc].Add(hamiltonianPath[i]);
                    c = 0;

                }
                else
                {
                    hamiltonianPath[i].PathID = nc;
                    nodes[nc].Add(hamiltonianPath[i]);

                }
                c++;

            }

            return true;

        }
        return false;

    }


    private void InitializeGrid()
    {
        totalNodes = totalRows * totalCols;
        grid = new Node[totalRows, totalCols];

        int value = 0;
        for (int row = 0; row < totalRows; row++)
        {
            for (int col = 0; col < totalCols; col++)
            {
                grid[row, col] = new Node(row, col, --value);
            }
        }

        for (int row = 0; row < totalRows; row++)
        {
            for (int col = 0; col < totalCols; col++)
            {
                if (col > 0)
                {
                    grid[row, col].Left = grid[row, col - 1];
                }
                if (col < totalCols - 1)
                {
                    grid[row, col].Right = grid[row, col + 1];
                }
                if (row > 0)
                {
                    grid[row, col].Up = grid[row - 1, col];
                }
                if (row < totalRows - 1)
                {
                    grid[row, col].Down = grid[row + 1, col];
                }
            }
        }
    }

    public void aaa()
    {
        search = ! search;
    }

    private List<Node> FindHamiltonianPath(Node startNode)
    {
        List<Node> path = new List<Node>();
        bool[] visited = new bool[totalNodes];
        path.Add(startNode);
        visited[(startNode.Row  * totalCols) + startNode.Col]  = true;
        if (HamiltonianPathUtil(startNode, 1, visited, path))
        {
            return path;
        }

        return new List<Node>(); // No Hamiltonian path found
    }

    private bool HamiltonianPathUtil(Node current, int count, bool[] visited, List<Node> path)
    {
        
        counter += 1;
        if (count == totalNodes)
        {
            return true; // Hamiltonian path found
        }

        if (counter > limit)
        {
            return false;
        }

        Node[] neighbors = GetUnvisitedNeighbors(current, visited);
        foreach (Node neighbor in neighbors)
        {

            path.Add(neighbor);

            visited[neighbor.Row * totalCols + neighbor.Col] = true;

            if (HamiltonianPathUtil(neighbor, count + 1, visited, path))
            {
                return true;
            }

            path.RemoveAt(path.Count - 1); // Backtrack
            var a =path[path.Count - 1];
            visited[neighbor.Row * totalCols + neighbor.Col] = false;
        }

        return false;
    }

    private Node[] GetUnvisitedNeighbors(Node current, bool[] visited)
    {
        List<Node> unvisitedNeighbors = new List<Node>();

        if (totalNodes <= 36)

        {
            Node[] neighbors = { current.Left, current.Up, current.Right, current.Down };

            for (int i = 0; i < neighbors.Length; i++)
            {
                int j = Random.Range(0, i + 1);
                Node temp = neighbors[i];
                neighbors[i] = neighbors[j];
                neighbors[j] = temp;
            }

            foreach (Node neighbor in neighbors)
            {
                if (neighbor != null && !visited[neighbor.Row * totalCols + neighbor.Col])
                {
                    unvisitedNeighbors.Add(neighbor);
                }
            }

            return unvisitedNeighbors.ToArray();
        }
        else
        {
            List<Node[]> n = new List<Node[]>
            {
                new Node[] { current.Left, current.Up, current.Right, current.Down },
                new Node[] { current.Left, current.Up, current.Down, current.Right },
                new Node[] { current.Left, current.Right, current.Up, current.Down },
                new Node[] { current.Left, current.Right, current.Down, current.Up },
                new Node[] { current.Left, current.Down, current.Up, current.Right },
                new Node[] { current.Left, current.Down, current.Right, current.Up },
                new Node[] { current.Up, current.Left, current.Right, current.Down },
                new Node[] { current.Up, current.Left, current.Down, current.Right },
                new Node[] { current.Up, current.Right, current.Down, current.Left },
                new Node[] { current.Up, current.Right, current.Left, current.Down },
                new Node[] { current.Up, current.Down, current.Left, current.Right },
                new Node[] { current.Up, current.Down, current.Right, current.Left },
                new Node[] { current.Down, current.Up, current.Right, current.Left },
                new Node[] { current.Down, current.Left, current.Up, current.Right },
                new Node[] { current.Down, current.Right, current.Left, current.Up },
                new Node[] { current.Down, current.Right, current.Up, current.Left },
                new Node[] { current.Down, current.Up, current.Left, current.Right },
                new Node[] { current.Down, current.Left, current.Right, current.Up },




                new Node[] { current.Right, current.Up, current.Down, current.Left },
                new Node[] { current.Right, current.Up, current.Left, current.Down },
                new Node[] { current.Right, current.Left, current.Up, current.Down },
                new Node[] { current.Right, current.Left, current.Down, current.Up },
                new Node[] { current.Right, current.Down, current.Left, current.Up },
                new Node[] { current.Right, current.Down, current.Up, current.Left },












            };

            foreach (Node neighbor in n[mover])
            {
                if (neighbor != null && !visited[neighbor.Row * totalCols + neighbor.Col])
                {
                    unvisitedNeighbors.Add(neighbor);
                }
            }

            return unvisitedNeighbors.ToArray();
        }

        /*        Node[] neighbors = { current.Down, current.Up, current.Right, current.Left };*/
        /*
                Node[] neighbors = { current.Left, current.Up, current.Right, current.Down };
                Node[] neighbors = { current.Left, current.Up, current.Right, current.Down };*/


    }

    private void PrintHamiltonianPath(List<Node> path)
    {
        Debug.Log("===========================");

        foreach (Node node in path)
        {
            Debug.Log($"({node.Row}, {node.Col}) - Value: {node.Value}");
        }
    }
}

public class Node
{
    public int Row { get; }
    public int Col { get; }
    public int PathID { get; set; }

    public int Value { get; set; }
    public Node Left { get; set; }
    public Node Right { get; set; }
    public Node Up { get; set; }
    public Node Down { get; set; }

    public Node(int row, int col, int value)
    {
        Row = row;
        Col = col;
        Value = value;
        Left = null;
        Right = null;
        Up = null;
        Down = null;
    }
}
