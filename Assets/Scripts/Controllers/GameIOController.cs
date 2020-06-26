using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameIOController : MonoBehaviour
{
    private JsonFileDirectory jsonFileDirectoryList;
    private GameDataList gameDataList;

    public GameDataList GameDataList
    {
        get
        {
            return gameDataList;
        }
    }

    //list of points belonging to a match of figures 
    private List<TouchData> matched;
    public List<TouchData> Matched
    {
        get
        {
            return matched;
        }
    }
    //list of points that make up lines of unpaired figures
    private List<TouchData> unmatched;
    public List<TouchData> Unmatched
    {
        get
        {
            return unmatched;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateDirectories();
        LoadJsonFileDirectory();
        LoadGameDataList();
    }

    ///<summary>
    /// returns an array with the all the gameData json string paths
    /// </summary>
    public void LoadJsonFileDirectory()
    {
        string path = "Resourcen/GameDataDirectory.json";
        if (JsonReadWriteController.FileExists(path))
        {
            jsonFileDirectoryList = JsonUtility.FromJson<JsonFileDirectory>
                    (JsonReadWriteController.LoadJsonAsExternalResource(path));
        }
        else
        {
            jsonFileDirectoryList = new JsonFileDirectory()
            { paths = new List<string>() };
        }
    }

    public void LoadGameDataList()
    {
        gameDataList = new GameDataList() { gameData = new List<GameData>() };
        string path = "Resourcen/GameDataFiles";
        if (jsonFileDirectoryList.paths.Count != 0)
        {
            foreach (var jsonFileName in jsonFileDirectoryList.paths)
            {
                string tempPath = path + "/" + jsonFileName;
                if (JsonReadWriteController.FileExists(tempPath))
                {
                    var gameDataAsString = JsonReadWriteController.LoadJsonAsExternalResource(tempPath);
                    GameData parsedGameData = JsonUtility.FromJson<GameData>(gameDataAsString);
                    gameDataList.gameData.Add(parsedGameData);
                }
            }
        }
    }

    void CreateDirectories()
    {
        //erstellt die nötigen Verzeichnisse (Resourcen/GameData/TouchDataJsonFiles), wenn sie noch nicht vorhanden sind.
        Directory.CreateDirectory(Path.GetDirectoryName(
            Application.persistentDataPath + "/Resourcen/GameDataFiles/"));
    }

    public int GetBestScore()
    {
        LoadGameDataList();

        if (gameDataList.gameData.Count != 0)
        {
            var maxScore = gameDataList.gameData.Max(figuresScore => figuresScore.score.matchedFigures);
            return maxScore;
        }
        return 0;
    }

    public void AddNewTouchData(List<Point> points, List<MatchedFigure> matchedFigures)
    {
        TouchData touchData = new TouchData();
        touchData.points = points;
        touchData.sameFigures = matchedFigures;

        if (matchedFigures.Count == GameController.Instance.NumberOfCopies)
        {
            matched.Add(touchData);
        }
        else
        {
            unmatched.Add(touchData);
        }
    }

    public void InitializeCurrentGameDataLists()
    {
        matched = new List<TouchData>();
        unmatched = new List<TouchData>();
    }


    public void SaveNewGameData()
    {
        if (matched.Count != 0 || unmatched.Count != 0)
        {
            GameData gameData = new GameData();
            gameData.id = jsonFileDirectoryList.paths.Count + 1;
            gameData.score = CreateNewScore();

            UserData userData = new UserData();
            userData.uid1 = GameController.Instance.Uid1;
            userData.uid2 = GameController.Instance.Uid2;
            gameData.userData = userData;
            TouchDataList touchDataList = new TouchDataList();
            touchDataList.matched = matched;
            touchDataList.unmatched = unmatched;
            gameData.touchDataList = touchDataList;
            string newFileName = "Game" + gameData.id + ".json";
            string outputString = JsonUtility.ToJson(gameData);
            JsonReadWriteController.WriteJsonToExternalResource
                ("Resourcen/GameDataFiles/" + newFileName, outputString);
            jsonFileDirectoryList.paths.Add(newFileName);
            string directoryListAsString = JsonUtility.ToJson(jsonFileDirectoryList);
            JsonReadWriteController.WriteJsonToExternalResource("Resourcen/GameDataDirectory.json", directoryListAsString);
        }
    }

    public ScoreData CreateNewScore()
    {
        ScoreData scoreData = new ScoreData();
        scoreData.numberOfPlayers = GameController.Instance.NumberOfPlayers;
        scoreData.gridSize = GameController.Instance.Rows + "x" + GameController.Instance.Columns;
        scoreData.matchedFigures = GameController.Instance.Score;
        scoreData.time = new Time();
        scoreData.time.initialTime = GameController.Instance.AnfangZeit;
        scoreData.time.finalTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        return scoreData;
    }
}
