[System.Serializable]
public class ScoreData
{
    public int matchedFigures;
    public Time time;
    public string gridSize;
    public int numberOfPlayers;
}

[System.Serializable]
public class Time
{
    public long initialTime;
    public long finalTime;
}