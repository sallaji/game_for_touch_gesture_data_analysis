using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigurePiece : MonoBehaviour
{

    public Sprite[] sprites;
    private SpriteRenderer sprite;

    //only figures that collide with the same line are allowed to be matched (not other lines)
    private string lineName;

    public string LineName
    {
        get
        {
            return lineName;
        }
        set
        {
            lineName = value;
        }
    }

    private void Awake()
    {
        sprite = transform.Find("figureSprite").GetComponent<SpriteRenderer>();
    }

    public void SetSprite(int? pairedNumber)
    {
        var a = pairedNumber;
        if(a == null)
        {
            pairedNumber = 0;
        }
        int pairedTemp = (int)pairedNumber;
        sprite.sprite = sprites[pairedTemp];
    }
}
