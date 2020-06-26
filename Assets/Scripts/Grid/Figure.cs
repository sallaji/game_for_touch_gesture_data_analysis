using UnityEngine;

public class Figure : MonoBehaviour, IRecycle
{
    //if the figure has been already selected by a user it gets blocked
    private bool figureIsSelected = false;
    private Vector3 originalSize;

    public Vector3 OriginalSize
    {
        get
        {
            return originalSize;
        }
        set
        {
            originalSize = value;
        }
    }
    
    int counter = 0;
    private void Start()
    {
        OnLineCollider.onFigureTouched += ResizeByCollision;

    }

    public void Restart()
    {
        var renderer = transform.Find("figureSprite")
         .GetComponent<SpriteRenderer>().sprite;
        var collider = GetComponent<BoxCollider2D>();
        var size = renderer.bounds.size*1.35f;
        collider.size = size;
    }

    public void ShutDown()
    {

    }

    // using delegate this will be called every time the figure is hit by the line
    void ResizeByCollision(GameObject gameObject)
    {
        if (counter == 0)
        {
            originalSize = transform.localScale;
            counter++;
        }
        if (counter == 1)
        {
            gameObject.transform.localScale = 1.1f * originalSize;
            gameObject.GetComponent<GamePiece>().SelectedFigureComponent.Select();
        }
    }
}

