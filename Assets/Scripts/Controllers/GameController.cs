using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController Instance { get; private set; }

    private GameIOController gameIOController;

    public GameIOController GameIOController
    {
        get
        {
            return gameIOController;
        }
        set
        {
            gameIOController = value;
        }
    }

    private string uid1;
    public string Uid1
    {
        get
        {
            return uid1;
        }
        set
        {
            uid1 = value;
        }
    }

    private string uid2;
    public string Uid2
    {
        get
        {
            return uid2;
        }
        set
        {
            uid2 = value;
        }
    }

    private int score;
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
        }
    }

    private int bestScore;
    public int BestScore
    {
        get
        {
            return bestScore;
        }
        set
        {
            bestScore = value;
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

    private int numberOfFigures;

    public int NumberOfFigures
    {
        get
        {
            return numberOfFigures;
        }
        set
        {
            numberOfFigures = value;
        }
    }

    private int numberOfPlayers;

    public int NumberOfPlayers
    {
        get
        {
            return numberOfPlayers;
        }
        set
        {
            numberOfPlayers = value;
        }
    }

    private long anfangZeit;

    public long AnfangZeit
    {
        get
        {
            return anfangZeit;
        }
        set
        {
            anfangZeit = value;
        }
    }

    private bool gUICanvasEnabled;
    private float deltaTime;

    public float DeltaTime
    {
        get
        {
            return deltaTime;
        }
    }

    public bool GUICanvasEnabled
    {
        get
        {
            return gUICanvasEnabled;
        }
        set
        {
            gUICanvasEnabled = value;
        }
    }

    private bool gamePausedCanvasEnabled;

    public bool GamePausedCanvasEnabled
    {
        get
        {
            return gamePausedCanvasEnabled;
        }
        set
        {
            gamePausedCanvasEnabled = value;
        }
    }

    private bool soundIsActive;

    public bool SoundIsActive
    {
        get
        {
            return soundIsActive;
        }
        set
        {
            soundIsActive = value;
        }
    }

    //Unique instantiatio of the GameController class before the actual game starts
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

    void Start()
    {
        soundIsActive = true;
        gameIOController = GetComponent<GameIOController>();
        DrawLine.onFiguresMatched += OnFiguresMatched;
        AudioController.StartBackgroundSound();
    }

    // constant addition of elapsed time between the game frames (delta times)
    void Update()
    {
        if (gamePausedCanvasEnabled == false)
        {
            deltaTime = deltaTime + UnityEngine.Time.deltaTime;
        }
    }
    public void OnGameStarted()
    {
        numberOfFigures = rows * columns;
        Grid.Instance.Columns = columns;
        Grid.Instance.Rows = rows;
        Grid.Instance.NumberOfCopies = numberOfCopies;
        score = 0;
        bestScore = gameIOController.GetBestScore();
        anfangZeit = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        gameIOController.InitializeCurrentGameDataLists();
        InitializeGrid();

    }
    public void RestartVariables()
    {
        var bgObjects = GameObject.Find("Grid").transform;
        foreach (Transform obj in bgObjects)
        {
            GameObjectUtil.Destroy(obj.gameObject);
        }
    }

    //restarts the game
    void InitializeGrid()
    {
        Grid.Instance.InitializeGrid();
    }

    void OnFiguresMatched()
    {
        if (numberOfFigures == 0)
        {
            numberOfFigures = rows * columns;
            Grid.Instance.InitializeFigures();
        }
        score = score + numberOfCopies;
    }

    private void OnLevelWasLoaded(int level)
    {
        switch (level)
        {
            //goes to the game configuration scene
            case 0:
                RestartVariables();
                gUICanvasEnabled = false;
                AudioController.StartBackgroundSound();
                break;
            case 1:
                //goes to the Grid scene
                deltaTime = UnityEngine.Time.deltaTime;
                OnGameStarted();
                gUICanvasEnabled = true;
                AudioController.StopSound();
                break;
        }
    }
}


