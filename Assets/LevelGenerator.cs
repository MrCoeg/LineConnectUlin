using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{


    public int limit;
    public int color;
    public int gridWidth;
    public int gridHeight;



    public HamiltonianPathGenerator sd;

    public HamiltonianPathGenerator[] HamiltonianPathGenerator;
    public Color[] colors;

    public Transform parent;
    public GameObject Image;
    public GameObject row;
    public AudioSource audioSource;

    public PointerManager manager;
    private GameObject[] rows;

    private void Awake()
    {
        

    }

    public void Generate()
    {
        var baseIdCount = 0d;
        var baseId = 0;
        var c = 0;
        HamiltonianPathGenerator = GetComponentsInChildren<HamiltonianPathGenerator>();
        foreach (var item in HamiltonianPathGenerator)
        {
            item.limit = limit;
            item.hc = color;
            item.totalCols = gridWidth;
            item.totalRows = gridHeight;

            if (item.GeneratePath())
            {
                if (item.counter < item.limit && item.counter > baseIdCount)
                {
                    baseIdCount = item.counter;
                    baseId = c;
                }
            }
            c++;
        }
        var h = HamiltonianPathGenerator[baseId];
        sd = h;
        var ySize = h.totalRows;
        var xSize = h.totalCols;


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

            }
            rows[i].GetComponent<RectTransform>().sizeDelta = new Vector2((xSize * 100) + ((xSize - 1) * 50), 100);
        }
        manager.Init();
        manager.xSize = xSize;
        manager.ySize = ySize;

        foreach (var path in h.nodes)
        {
            var first = true;
            var last = path[path.Count - 1];
            foreach (var node in path)
            {
                var index = (node.Row * xSize) + node.Col;
                if (first)
                {
                    manager.pointers[index].SetNode(node.PathID, GetColorByIndex(node.PathID));
                    var p = manager.pointers[index].AddComponent<PathTemp>();
                    p.pathId = node.PathID;
                    p.truePath.Add(manager.pointers[index]);
                    manager.path.Add(p);
                    first = false;
                }

                else if (last.Value == node.Value)
                {
                    manager.pointers[index].SetNode(node.PathID, GetColorByIndex(node.PathID));
                    var p = manager.pointers[index].AddComponent<PathTemp>();
                    p.pathId = node.PathID;
                    p.truePath.Add(manager.pointers[index]);
                }

                else
                {
                    manager.pointers[index].pathId = -1;
                    manager.GetPath(node.PathID).truePath.Add(manager.pointers[index]);

                }
            }
        }
    }

    public Color GetColorByIndex(int index)
    {
        return colors[index % colors.Length];
    }
}
