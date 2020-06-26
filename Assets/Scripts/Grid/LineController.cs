using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LineController : MonoBehaviour
{

    //creates a reference for the lineController prefab
    public GameObject lineControllerPrefab;

    public List<LineLocation> lines = new List<LineLocation>();

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
        {

            foreach (var line in lines)
            {
                Destroy(line.lineControllerPrefab);
                lines.Remove(line);
            }
        }
        int i = 0;
        while (i < Input.touchCount && i <= GameController.Instance.NumberOfPlayers - 1)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began)
            {
                var lineInstance = Instantiate(lineControllerPrefab);
                lineInstance.GetComponent<DrawLine>().OnTouchBegan(touch.position, i);
                lineInstance.name = "line(" + i + ")";
                lineInstance.transform.parent = transform;

                lines.Add(new LineLocation(touch.fingerId, lineInstance));
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                LineLocation lineLocation = lines.Find(tLocation => tLocation.touchId == touch.fingerId);
                if (lineLocation.lineControllerPrefab)
                {
                    lineLocation.lineControllerPrefab.GetComponent<DrawLine>()
                    .OnTouchMoved(touch.position);
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                LineLocation lineLocation = lines.Find(tLocation => tLocation.touchId == touch.fingerId);
                if (lineLocation != null)
                {
                    //stores points and matched figures before destroying the lines
                    DrawLine componentDrawLine = lineLocation.lineControllerPrefab.GetComponent<DrawLine>();
                    List<Point> points = componentDrawLine.Points;

                    //gets the collisioned objects from the collision between the figures 
                    //of the grid and the line edge collider
                    List<MatchedFigure> matchedFigures =
                    componentDrawLine.LineRenderer.GetComponent<OnLineCollider>().MatchedFigures;
                    GameController.Instance.GameIOController.AddNewTouchData(points, matchedFigures);
                    lineLocation.lineControllerPrefab.GetComponent<DrawLine>()
.OnTouchEnded(touch.position);
                    lines.RemoveAt(lines.IndexOf(lineLocation));
                    Destroy(lineLocation.lineControllerPrefab);
                }
            }
            ++i;
        }
    }
}
