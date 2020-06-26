using System.Collections.Generic;

[System.Serializable]
public class TouchDataList
{
    public List<TouchData> matched;
    public List<TouchData> unmatched;
}

[System.Serializable]
public class TouchData
{
    public List<Point> points;
    public List<MatchedFigure> sameFigures;
}