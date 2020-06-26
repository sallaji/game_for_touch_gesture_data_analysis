using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineLocation
{
    //gebrauch um die FingerID zu identifizieren
    public int touchId;
    public GameObject lineControllerPrefab;

    public LineLocation(int touchId, GameObject lineControllerPrefab)
    {
        this.touchId = touchId;
        this.lineControllerPrefab = lineControllerPrefab;
    }
}
