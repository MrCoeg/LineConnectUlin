using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Properties")]
public class LevelData : ScriptableObject
{
    [SerializeField] public List<int> level = new List<int>();
    [SerializeField] public List<int> xSize = new List<int>();
    [SerializeField] public List<int> ySize = new List<int>();
    [SerializeField] public List<int> agent = new List<int>();
    [SerializeField] public List<int> rock = new List<int>();



    public (int level, int xSize, int ySize, int agent) getData(int id){
        return (level[id], xSize[id], ySize[id], agent[id]); 
    }
    public void Flush()
    {
        level.Clear();
        xSize.Clear();
        ySize.Clear();
        agent.Clear();
        rock.Clear();
    }
}
