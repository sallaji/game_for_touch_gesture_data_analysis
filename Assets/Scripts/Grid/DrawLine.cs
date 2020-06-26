using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//
public class DrawLine : MonoBehaviour
{
    public delegate void InformMatchesToGameController();
    public static event InformMatchesToGameController onFiguresMatched;

    //holds the prefab of the line object
    public GameObject linePrefab;

    //holds the current line object
    private GameObject currentLine;

    //holds the line renderer component
    private LineRenderer lineRenderer;

    public LineRenderer LineRenderer
    {
        get
        {
            return lineRenderer;
        }
    }

    //holds the edge collider component
    private EdgeCollider2D edgeCollider2D;

    //receives input from the user whenever he touches the touch surface

    //keeps track of the position of the user's finger as he moves it around to draw the line
    public List<Vector2> fingerPositions;

    //the list of recorded touch points to be stored in JSON format
    private List<Point> points;
    public List<Point> Points
    {
        get
        {
            return points;
        }
        set
        {
            points = value;
        }
    }

    //list of figures of the same type that have been linked through the line
    private List<GameObject> sameFigures;
    public List<GameObject> SameFigures
    {
        get
        {
            return sameFigures;
        }
    }

    //creates a new instance of the Point class list
    private void Awake()
    {
        points = new List<Point>();
    }

    //adds the start point of the currentLine 
    public void OnTouchBegan(Vector3 position, int fingerId)
    {
        var toWorldPosition = GetScreenToWorldPosition(position);
        CreateLine(toWorldPosition, fingerId);
        AddNewPoint(position);
    }

    //adds new points to the line when the finger is moved
    public void OnTouchMoved(Vector3 position)
    {
        //the distance between the last point and the new one must be greather than 0.1px
        if (Vector2.Distance(GetScreenToWorldPosition(position),
                     fingerPositions[fingerPositions.Count - 1]) > .1f)
        {
            UpdateLine(GetScreenToWorldPosition(position));
            AddNewPoint(position);
        }
    }
    //Destroys(disables) the figures when there is a match. 
    //Destroys the line when the finger is withdrawn from the touch surface (screen)
    public void OnTouchEnded(Vector3 position)
    {
        UpdateLine(GetScreenToWorldPosition(position));

        var allFiguresSame = (sameFigures.Any(o => o != sameFigures[0]));
        Debug.Log(allFiguresSame);
        if (sameFigures != null && allFiguresSame && sameFigures.Count == GameController.Instance.NumberOfCopies)
        {
            AudioController.PlaySound("correct");
            //Both players can make a match by selecting the same figures and 
            //getting points, however, the figures will be deactivated once the 
            //first match is executed. If the paired figure is active, the number 
            //of figures visible in the grid will be reduced by 1. 
            foreach (var sameFigure in sameFigures)
            {
                if (sameFigure.activeSelf == true)
                {
                    GameController.Instance.NumberOfFigures -= 1;
                }
                //the GameObjectUtil will disable the figure instead of destroying it
                GameObjectUtil.Destroy(sameFigure);
            }
            onFiguresMatched?.Invoke();
        }
        //if just one figure is selected it will be resized by 1.4f
        else if (sameFigures != null && sameFigures.Count < GameController.Instance.NumberOfCopies)
        {
            AudioController.PlaySound("incorrect");
            foreach (var sameFigure in sameFigures)
            {
                sameFigure.transform.localScale = sameFigure.transform.localScale / 1.3f;
            }
        }
        Destroy(currentLine);
    }
    //function for when a player has moved his finger far enough to add a new point to the line
    void CreateLine(Vector3 position, int fingerId)
    {
        //instantiates/Saves the new line prefab into the currentline variable
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        edgeCollider2D = currentLine.GetComponent<EdgeCollider2D>();
        currentLine.transform.parent = transform;
        currentLine.name = "line(" + fingerId + ")";
        //clears the list because this is a new line
        fingerPositions.Clear();
        var toWorldPosition = GetScreenToWorldPosition(position);
        //appends to the end of the list
        fingerPositions.Add(position);
        fingerPositions.Add(position);
        lineRenderer.SetPosition(0, fingerPositions[0]);
        lineRenderer.SetPosition(1, fingerPositions[1]);
        //this function first creates a new line and saves that line into 
        //different variables, it will then set the first two points of the line renderer component and the edge collider component
        edgeCollider2D.points = fingerPositions.ToArray();
    }

    /*
     * Adds a new point to the line  
     */
    void UpdateLine(Vector2 newFingerPos)
    {
        fingerPositions.Add(newFingerPos);


        //gets the collisioned objects from the collision between the figures 
        //of the grid and the line edge collider
        var storedFigureCollisions =
        lineRenderer.GetComponent<OnLineCollider>().GleicheFiguren;

        if (storedFigureCollisions != null)
        {
            sameFigures = storedFigureCollisions.FindAll(figure =>
            {
                var objeto = storedFigureCollisions.First();
                var sameSpriteName =
                figure.transform.Find("figureSprite")
                .GetComponent<SpriteRenderer>().sprite.name
                == objeto.transform.Find("figureSprite")
                .GetComponent<SpriteRenderer>().sprite.name;
                return sameSpriteName;
            });
            //increases the size of how many points there are in the line renderer component
            lineRenderer.positionCount = 1 + sameFigures.Count;
        }

        //sets the newpoint position to the value of the parameter
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);

        edgeCollider2D.points = fingerPositions.ToArray();
    }

    //adds a new serialiable Point to the Point list
    void AddNewPoint(Vector3 position)
    {
        Point point = new Point();
        point.deltaTime = GameController.Instance.DeltaTime;
        point.x = position.x / Screen.width;
        point.y = position.y / Screen.height;
        points.Add(point);
    }

    Vector2 GetScreenToWorldPosition(Vector2 touchPosition)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, transform.position.z));
    }
}
