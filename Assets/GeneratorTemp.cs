using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratatorTemp : MonoBehaviour
{
    private int[,] maze;
    public int xSize;
    public int ySize;

    public int nAgents;
    private List<Agents> agents = new List<Agents>();
    private List<Tuple<int, int>> directions = new List<Tuple<int, int>>() {
        Tuple.Create(-1, 0), Tuple.Create(0, 1), Tuple.Create(1, 0), Tuple.Create(0, -1)
    };
    public Color[] colors;

    public Transform parent;
    public GameObject Image;
    public GameObject row;
    public AudioSource audioSource;


    public PointerManager manager;
    private int counter = 0;
    private GameObject[] rows;
    public float speed;
    private int tempsize;
    private void Start()
    {
        this.xSize = manager.data.xSize[manager.level - 1];
        this.ySize = manager.data.ySize[manager.level - 1];
        this.maze = new int[ySize, xSize];
        this.nAgents = manager.data.agent[manager.level - 1];
        StartCoroutine(Regen());
    }

    public void ReGenerateMaze()
    {
        this.manager.pointers.Clear();
        for (int i = 0; i < tempsize; i++)
        {
            DestroyImmediate(rows[i]);
        }
        this.maze = new int[ySize, xSize];
        this.agents.Clear();


        StartCoroutine(Regen());

    }

    public IEnumerator RegenAgain()
    {
        this.nAgents = manager.data.agent[manager.level - 1];
        this.maze = new int[ySize, xSize];
        agents.Clear();
        var rand = new System.Random();
        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                maze[i, j] = -1;
            }
        }
        for (int i = 0; i < nAgents; i++)
        {
            var decision = GetRandomStartPosition(rand);
            Tuple<int, int> startPosition = decision.Item1;
            agents.Add(new Agents());
            agents[i].id = i;
            agents[i].position = startPosition;

            maze[startPosition.Item1, startPosition.Item2] = i;

            var tempTemp = rows[startPosition.Item1].transform;
            var temp = tempTemp.GetChild(startPosition.Item2).GetComponentInChildren<PointerTemp>();
            var tempPath = tempTemp.GetChild(startPosition.Item2).gameObject.AddComponent<PathTemp>();
            tempPath.pathId = agents[i].id;
            tempPath.truePath.Add(temp);
            manager.path.Add(tempPath);
            temp.SetNode(agents[i].id, GetColorByIndex(Mathf.Abs(maze[startPosition.Item1, startPosition.Item2])));


            yield return new WaitForSeconds(speed);
        }

        bool movesMade;
        do
        {
            movesMade = false;
            foreach (var agent in agents)
            {
                int agentId = agent.id % colors.Length;
                int pathId = agent.id;
                Tuple<int, int> currentCell = agent.position;
                Shuffle(directions, rand);
                bool moved = false;
                foreach (var direction in directions)
                {
                    var nextCell = Tuple.Create(currentCell.Item1 + direction.Item1, currentCell.Item2 + direction.Item2);


                    if (CanMoveTo(nextCell))
                    {
                        /*                        Debug.Log(currentCell + " " + nextCell);*/

                        maze[nextCell.Item1, nextCell.Item2] = agentId;
                        agents[agentId].position = nextCell;
                        movesMade = true;
                        moved = true;
                        var tempTemp = rows[nextCell.Item1].transform;
                        var temp = tempTemp.GetChild(nextCell.Item2).GetComponentInChildren<PointerTemp>();
                        temp.pathId = -1;
                        manager.GetPath(pathId).truePath.Add(temp);

                        /*                        temp.image.color = GetColorByIndex(Mathf.Abs(maze[nextCell.Item1, nextCell.Item2]));
                                                temp.color = GetColorByIndex(Mathf.Abs(maze[nextCell.Item1, nextCell.Item2]));*/
                        yield return new WaitForSeconds(speed);

                        break;
                    }
                }

                if (!moved)
                {
                    /*                    Debug.Log(agentId + " Ended " + currentCell);*/
                    var tempTemp = rows[currentCell.Item1].transform;
                    var temp = tempTemp.GetChild(currentCell.Item2).GetComponentInChildren<PointerTemp>();
                    temp.SetNode(pathId, GetColorByIndex(Mathf.Abs(maze[currentCell.Item1, currentCell.Item2])));
                    var decision = GetRandomStartPosition(rand);
                    Tuple<int, int> newStartPosition = decision.Item1;

                    if (decision.Item2)
                    {
                        agents[agentId].id = agents[agentId].TransferId(colors.Length);
                        agents[agentId].position = newStartPosition;
                        maze[newStartPosition.Item1, newStartPosition.Item2] = agentId;
                        movesMade = true;
                        /*                        Debug.Log(agentId + " StartNewPos " + newStartPosition);*/
                        tempTemp = rows[newStartPosition.Item1].transform;

                        temp = tempTemp.GetChild(newStartPosition.Item2).GetComponentInChildren<PointerTemp>();
                        temp.SetNode(agents[agentId].id, GetColorByIndex(Mathf.Abs(maze[newStartPosition.Item1, newStartPosition.Item2])));


                        var pathTemp = tempTemp.GetChild(newStartPosition.Item2).gameObject.AddComponent<PathTemp>();
                        pathTemp.truePath.Add(temp);
                        pathTemp.pathId = agents[agentId].id;
                        manager.path.Add(pathTemp);


                        yield return new WaitForSeconds(speed);

                    }
                }
            }
        } while (movesMade);

        manager.SetRock(manager.data.rock[manager.level - 1]);
    }


    private IEnumerator Regen()
    {
        rows = new GameObject[ySize];
        var id = -1;
        // Initialize the maze with walls (-1)
        for (int i = 0; i < ySize; i++)
        {
            rows[i] = Instantiate(row, parent, false);
            for (int j = 0; j < xSize; j++)
            {
                var instantiated = Instantiate(Image, rows[i].transform, false);
                var temp = instantiated.GetComponentInChildren<PointerTemp>();
                temp.audioSource = audioSource;
                temp.id = id--;
                manager.pointers.Add(temp);

                maze[i, j] = -1;
            }
            rows[i].GetComponent<RectTransform>().sizeDelta = new Vector2((xSize * 100) + ((xSize - 1) * 50), 100);
        }
        manager.Init();
        manager.xSize = xSize;
        manager.ySize = ySize;


        // Initialize agents at random positions
        var rand = new System.Random();
        for (int i = 0; i < nAgents; i++)
        {
            var decision = GetRandomStartPosition(rand);
            Tuple<int, int> startPosition = decision.Item1;
            agents.Add(new Agents());
            agents[i].id = i;
            agents[i].position = startPosition;

            maze[startPosition.Item1, startPosition.Item2] = i;
/*            Debug.Log(i + " Start " + agents[i]);*/


            var tempTemp = rows[startPosition.Item1].transform;
            var temp = tempTemp.GetChild(startPosition.Item2).GetComponentInChildren<PointerTemp>();
            var tempPath = tempTemp.GetChild(startPosition.Item2).gameObject.AddComponent<PathTemp>();
            tempPath.pathId = agents[i].id;
            tempPath.truePath.Add(temp);
            manager.path.Add(tempPath);
            temp.SetNode(agents[i].id, GetColorByIndex(Mathf.Abs(maze[startPosition.Item1, startPosition.Item2])));


            yield return new WaitForSeconds(speed);
        }

        bool movesMade;
        do
        {
            movesMade = false;
            foreach (var agent in agents)
            {
                int agentId = agent.id % colors.Length;
                int pathId = agent.id;
                Tuple<int, int> currentCell = agent.position;
                Shuffle(directions, rand);
                bool moved = false;
                foreach (var direction in directions)
                {
                    var nextCell = Tuple.Create(currentCell.Item1 + direction.Item1, currentCell.Item2 + direction.Item2);


                    if (CanMoveTo(nextCell))
                    {
/*                        Debug.Log(currentCell + " " + nextCell);*/

                        maze[nextCell.Item1, nextCell.Item2] = agentId;
                        agents[agentId].position = nextCell;
                        movesMade = true;
                        moved = true;
                        var tempTemp = rows[nextCell.Item1].transform;
                        var temp = tempTemp.GetChild(nextCell.Item2).GetComponentInChildren<PointerTemp>();
                        temp.pathId = -1;
                        manager.GetPath(pathId).truePath.Add(temp);

                        /*                        temp.image.color = GetColorByIndex(Mathf.Abs(maze[nextCell.Item1, nextCell.Item2]));
                                                temp.color = GetColorByIndex(Mathf.Abs(maze[nextCell.Item1, nextCell.Item2]));*/
                        yield return new WaitForSeconds(speed);

                        break;
                    }
                }

                if (!moved)
                {
/*                    Debug.Log(agentId + " Ended " + currentCell);*/
                    var tempTemp = rows[currentCell.Item1].transform;
                    var temp = tempTemp.GetChild(currentCell.Item2).GetComponentInChildren<PointerTemp>();
                    temp.SetNode(pathId, GetColorByIndex(Mathf.Abs(maze[currentCell.Item1, currentCell.Item2])));
                    var decision = GetRandomStartPosition(rand);
                    Tuple<int, int> newStartPosition = decision.Item1;

                    if (decision.Item2)
                    {
                        agents[agentId].id = agents[agentId].TransferId(colors.Length);
                        agents[agentId].position = newStartPosition;
                        maze[newStartPosition.Item1, newStartPosition.Item2] = agentId;
                        movesMade = true;
/*                        Debug.Log(agentId + " StartNewPos " + newStartPosition);*/
                        tempTemp = rows[newStartPosition.Item1].transform;

                        temp = tempTemp.GetChild(newStartPosition.Item2).GetComponentInChildren<PointerTemp>();
                        temp.SetNode(agents[agentId].id, GetColorByIndex(Mathf.Abs(maze[newStartPosition.Item1, newStartPosition.Item2])));


                        var pathTemp = tempTemp.GetChild(newStartPosition.Item2).gameObject.AddComponent<PathTemp>();
                        pathTemp.truePath.Add(temp);
                        pathTemp.pathId = agents[agentId].id;
                        manager.path.Add(pathTemp);



                    }
                }
            }
        } while (movesMade);

        manager.SetRock(manager.data.rock[manager.level-1]);
    }

    /* private void GenerateMaze()
     {
         // Initialize the maze with walls (-1)
         for (int i = 0; i < size; i++)
             for (int j = 0; j < size; j++)
                 maze[i, j] = -1;

         // Initialize agents at random positions
         var rand = new System.Random();
         for (int i = 0; i < nAgents; i++)
         {
             var decision  = GetRandomStartPosition(rand);
             Tuple<int, int> startPosition = decision.Item1;
             agents[i] = startPosition;
             maze[startPosition.Item1, startPosition.Item2] = i;
             Debug.Log(i + " Start " + agents[i]);
         }

         bool movesMade;
         do
         {
             movesMade = false;
             foreach (var agent in new Dictionary<int, Tuple<int, int>>(agents))
             {
                 int agentId = agent.Key;
                 Tuple<int, int> currentCell = agent.Value;

                 Shuffle(directions, rand);
                 bool moved = false;
                 foreach (var direction in directions)
                 {
                     var nextCell = Tuple.Create(currentCell.Item1 + direction.Item1, currentCell.Item2 + direction.Item2);
                     if (CanMoveTo(nextCell))
                     {

                         maze[nextCell.Item1, nextCell.Item2] = agentId;
                         agents[agentId] = nextCell;
                         movesMade = true;
                         moved = true;
                         break;
                     }
                 }

                 if (!moved)
                 {
                     Debug.Log(agentId + " Ended " + currentCell);
                     var decision = GetRandomStartPosition(rand);
                     Tuple<int, int> newStartPosition = decision.Item1;

                     if (decision.Item2)
                     {
                         agents[agentId] = newStartPosition;
                         maze[newStartPosition.Item1, newStartPosition.Item2] = agentId;
                         movesMade = true;
                         Debug.Log(agentId + " StartNewPos " + newStartPosition);
                     }
                 }
             }
         } while (movesMade);
     }*/

    private bool CanMoveTo(Tuple<int, int> cell)
    {
        int row = cell.Item1, col = cell.Item2;
        return row >= 0 && row < ySize && col >= 0 && col < xSize && maze[row, col] == -1;
    }

    private void Shuffle<T>(List<T> list, System.Random rand)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public Color GetColorByIndex(int index)
    {
        return colors[index % colors.Length];
    }

    private (Tuple<int, int>, bool) GetRandomStartPosition(System.Random rand)
    {
        var counter = 0;
        int row, col;
        do
        {
            row = rand.Next(ySize);
            col = rand.Next(xSize);
            counter++;
        } while (maze[row, col] != -1 && counter < 10); // Ensure the start position is not already occupied
        if (counter >= 10)
        {
            return (Tuple.Create(row, col), false);
        }
        return (Tuple.Create(row, col), true);
    }

    public void PrintMaze()
    {

        rows = new GameObject[ySize];
        for (int i = 0; i < ySize; i++)
        {
            rows[i] = Instantiate(row, parent, false);
            for (int j = 0; j < xSize; j++)
            {
                var instantiated = Instantiate(Image, rows[i].transform, false);
                var temp = instantiated.GetComponent<PointerTemp>();
                temp.image.color = GetColorByIndex(Mathf.Abs(maze[i, j]));
                temp.color = GetColorByIndex(Mathf.Abs(maze[i, j]));
                manager.pointers.Add(temp);
            }
        }
    }
}