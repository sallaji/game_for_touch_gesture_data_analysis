using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public static Grid Instance { get; private set; }

    /*
     * replace the key for the the kind of figure you want to display when
     * spawning to the grid 
     */
    public enum PieceType
    {
        NORMAL,
    };

    //displays the types in the inspector and
    // associates the type with the prefab
    [System.Serializable]
    public struct PiecePrefab
    {
        public PieceType type;
        public GameObject prefab;
    };

    private int columns;

    public int Columns
    {
        get
        {
            return columns;
        }
        set
        {
            columns = value;
        }
    }

    private int rows;

    public int Rows
    {
        get
        {
            return rows;
        }
        set
        {
            rows = value;
        }
    }

    private int numberOfCopies;

    public int NumberOfCopies
    {
        get
        {
            return numberOfCopies;
        }

        set
        {
            numberOfCopies = value;
        }
    }

    //List of figure Prefabs to be displayed on the grid
    public PiecePrefab[] piecePrefabs;
    public GameObject backgroundPrefab;
    private int?[,] pairedNumbersForSprites;

    private float perfectHeight;
    private float perfectWidth;
    private float heightFigure;

    private Dictionary<PieceType, GameObject> piecePrefabDict;

    private GamePiece[,] pieces;

    public GamePiece[,] Pieces
    {
        get
        {
            return pieces;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            //it does not destroy the gameobject that the gamemanager script is located on
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Returns a twodimensional array with random number values according to 
    // the quantity of figures to be displayed
    public int?[,] CreatePairedSpriteList(int spriteListLength)
    {
        System.Random r = new System.Random();
        var tempPairList = new int?[columns, rows];
        int randomX = r.Next(0, columns);
        int randomY = r.Next(0, rows);
        int randomSprite = r.Next(0, spriteListLength);
        var alreadyContainedNumbers
        = new List<int>();
        int counter = 0;
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {

                while (alreadyContainedNumbers.Contains(randomSprite))
                {
                    randomSprite = r.Next(0, spriteListLength);
                }

                while (tempPairList[randomX, randomY] != null)
                {
                    randomX = r.Next(0, columns);
                    randomY = r.Next(0, rows);
                }

                if (counter < numberOfCopies - 1)
                {
                    counter++;
                }
                else
                {
                    alreadyContainedNumbers.Add(randomSprite);
                    counter = 0;
                }
                tempPairList[randomX, randomY] = randomSprite;
            }

        }
        return tempPairList;
    }

    public void InitializeGrid()
    {
        perfectHeight = Screen.height / PixelPerfectCamera.pixelsToUnits;
        perfectWidth = Screen.width / PixelPerfectCamera.pixelsToUnits;
        heightFigure = perfectHeight / 1.1f / rows;
        LoadPrefabsIntoDictionary();
        InitializeBackgroundPrefab();
        InitializeFigures();
    }

    //Copies the values from the array pieceprefabs set in the inspector 
    //into this private dictionary
    private void LoadPrefabsIntoDictionary()
    {
        //Copies the values from the array pieceprefabs set in the inspector 
        //into this private dictionary
        piecePrefabDict = new Dictionary<PieceType, GameObject>();

        for (int i = 0; i < piecePrefabs.Length; i++)
        {
            //associates the PiecePrefab key to the prefab value
            if (!piecePrefabDict.ContainsKey(piecePrefabs[i].type))
            {
                piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);

            }
        }
    }

    public void InitializeBackgroundPrefab()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject background = GameObjectUtil.Instantiate(backgroundPrefab,
               GetPiecePosition(i, j));
                background.name = "Background(" + i + "," + j + ")";
                //makes it child of the parent object
                background.transform.parent = transform;

                Vector2 spriteDimensions = background.GetComponent<SpriteRenderer>().bounds.size;

                //calculates the sprite scale to its spriterenderer image vector2 size
                Vector2 spritePixelToScale = new Vector2(1 / spriteDimensions.x,
                                    1 / spriteDimensions.y);

                background.transform.localScale = new Vector3(spritePixelToScale.x * heightFigure,
                spritePixelToScale.y * heightFigure, 0);
            }
        }
    }

    public void InitializeFigures()
    {
        pieces = new GamePiece[columns, rows];
        pairedNumbersForSprites = CreatePairedSpriteList(50);
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                pieces[x, y] = SpawnNewPiece(x, y, PieceType.NORMAL);
                if (pieces[x, y].IsMovable())
                {
                    pieces[x, y].MovableComponent.Move(x, y);
                }
                if (pieces[x, y].HasFigurePieceComponent())
                {
                    pieces[x, y].FigureComponent.SetSprite(pairedNumbersForSprites[x, y]);
                }
            }
        }
    }

    //Returns the position of the piece inside the grid
    public Vector2 GetPiecePosition(int x, int y)
    {
        return new Vector2(x * heightFigure - columns * heightFigure / 2.0f + heightFigure / 2,
        y * heightFigure - rows * heightFigure / 2.0f + heightFigure / 2);
    }

    public GamePiece SpawnNewPiece(int x, int y, PieceType pieceType)
    {

        var newPiece = GameObjectUtil.Instantiate(piecePrefabDict[pieceType],
                    Vector3.zero);
        newPiece.name = "Piece(" + x + "," + y + ")";
        newPiece.transform.parent = transform;

        newPiece.transform.localScale = new Vector3(1, 1, 1);
        Vector3 spriteDimensions = newPiece.transform
        .Find("figureSprite").GetComponent<SpriteRenderer>().bounds.size;

        Vector2 spritePixelToScale = new Vector2((1 / spriteDimensions.x) * 0.6f,
           (1 / spriteDimensions.y) * 0.6f);
        var originalSize = new Vector3(spritePixelToScale.x * heightFigure,
        spritePixelToScale.y * heightFigure, 0);
        newPiece.GetComponent<Figure>().OriginalSize = originalSize;
        newPiece.transform.localScale = originalSize;
        pieces[x, y] = newPiece.GetComponent<GamePiece>();
        pieces[x, y].Init(x, y, this, PieceType.NORMAL);
        return pieces[x, y];
    }
}
