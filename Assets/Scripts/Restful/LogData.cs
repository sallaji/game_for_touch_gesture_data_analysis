using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LogData
{

    //stores the coordinates of the line
    public Dictionary<long, Dictionary<string, float>> points;

    public LogData()
    {
        points = new Dictionary<long, Dictionary<string, float>>();
    }
}
