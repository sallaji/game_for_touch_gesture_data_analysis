using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OnLineCollider : MonoBehaviour
{

    public delegate void ChangeFigureSize(GameObject gameObject);
    public static event ChangeFigureSize onFigureTouched;

    //contains all the same figures that match with the figure in the location point of the line
    private List<GameObject> gleicheFiguren = new List<GameObject>();

    public List<GameObject> GleicheFiguren
    {
        get
        {
            return gleicheFiguren;
        }
    }


    //Represents the serialized List of the MatchedFigure Object
    private List<MatchedFigure> matchedFigures = new List<MatchedFigure>();

    public List<MatchedFigure> MatchedFigures
    {
        get
        {
            return matchedFigures;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gleicheFiguren.Add(collision.collider.gameObject);
        var firstFigure = gleicheFiguren.First();
        var spriteNameFirstFigure =
            firstFigure.transform.Find("figureSprite").GetComponent<SpriteRenderer>().sprite.name;

        var spriteNameCollisionedFigure =
            collision.collider.gameObject.transform.Find("figureSprite")
            .GetComponent<SpriteRenderer>().sprite.name;
        if (spriteNameFirstFigure == spriteNameCollisionedFigure)
        {

            //adds the GamePiece object as MatchedFigure Item to the list before being stored as json object
            var figureAsGamePiece = collision.collider.gameObject;
            Vector3 figuePosition = Camera.main.WorldToScreenPoint(figureAsGamePiece.transform.position);
            MatchedFigure matchedFigure = new MatchedFigure()
            {
                x = figuePosition.x/Screen.width,
                y = figuePosition.y/Screen.height,
                deltaTimeMatch = GameController.Instance.DeltaTime
            };

            matchedFigures.Add(matchedFigure);

            onFigureTouched?.Invoke(collision.collider.gameObject);

        }
    }
}
