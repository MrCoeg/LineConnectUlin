using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agents
{
    public int id;
    public Tuple<int, int> position;

    public Agents()
    {

    }


    public int TransferId(int length)
    {
        return this.id + length;
    }
}
