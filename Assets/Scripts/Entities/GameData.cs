using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int id;
    public UserData userData;
    public ScoreData score;
    public TouchDataList touchDataList;
}

[System.Serializable]
public class GameDataList
{
    public List<GameData> gameData;
}