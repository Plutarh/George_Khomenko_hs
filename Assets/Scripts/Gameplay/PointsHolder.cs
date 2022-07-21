using System.Collections.Generic;
using System.Linq;

public static class PointsHolder
{
    public static Point BasePoint => allPoints.FirstOrDefault(p => p.pointType == EPointType.Base);
    public static List<Point> allPoints = new List<Point>();

}

