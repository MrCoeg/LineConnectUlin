using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelDataProperties", menuName ="Level")]
public class LevelSC : ScriptableObject
{
    public List<LevelProperties> levels = new List<LevelProperties>();

    public void AddLevel(int index, List<PointerTemp> nodes)
    {
        var pointers = new PointerProperties();

        foreach (var node in nodes)
        {
           pointers.pathIds.Add(node.pathId);
        }
        levels[index].nodes.Add(pointers);

    }
}

[System.Serializable]
public class PointerProperties
{
    public List<int> pathIds = new List<int>();
}

[System.Serializable]
public class LevelProperties
{
    public List<PointerProperties> nodes = new List<PointerProperties>();
}
