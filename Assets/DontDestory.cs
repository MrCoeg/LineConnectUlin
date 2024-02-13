using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestory : MonoBehaviour
{
    private void Awake()
    {
        var a = GameObject.Find(gameObject.name);
        if (a.GetInstanceID() != gameObject.GetInstanceID())
        {
            Destroy(gameObject);

        }

        DontDestroyOnLoad(this.gameObject);
    }
}
