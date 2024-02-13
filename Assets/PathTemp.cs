using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PathTemp : MonoBehaviour
{
    public int pathId;
    public List<PointerTemp> truePath = new List<PointerTemp>();
    [SerializeField] public List<PointerTemp> userPath = new List<PointerTemp>();
    public PointerTemp activePoint;

    public int IsInUserPath(PointerTemp temp)
    {
        return userPath.IndexOf(temp);
    }
    public void CheckPath()
    {
        Debug.Log(truePath.SequenceEqual(userPath, new PointerComparer()));
        userPath.Reverse();
        Debug.Log(truePath.SequenceEqual(userPath, new PointerComparer()));
        userPath.Reverse();
    }

    public void ResetPath()
    {

        int count = userPath.Count;
        for (int i = 0; i < count; i++)
        {
            Debug.Log(count);
            userPath[i].ResetImage();
            userPath[i].ResetUIPath();
        }
        userPath.Clear();
    }


    public void DestroyFromIndex(int id)
    {
        int count = userPath.Count;
        userPath[id].ResetUIPath();
        userPath[id].manager.SetActive(userPath[id]);
        for (int i = count-1; i > id; i--)
        {
            userPath[i].ResetImage();
            userPath[i].ResetUIPath();
            userPath.RemoveAt(i);
        }
    }
}

public class PointerComparer : IEqualityComparer<PointerTemp>
{
    public bool Equals(PointerTemp x, PointerTemp y)
    {
        if (x == null || y == null)
        {
            return false;
        }
        return x.id == y.id;
    }

    public int GetHashCode(PointerTemp obj)
    {
        return obj.id.GetHashCode();
    }
}