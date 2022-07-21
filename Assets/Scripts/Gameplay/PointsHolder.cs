using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PointsHolder
{
    public static Point BasePoint => allPoints.FirstOrDefault(p => p.pointType == EPointType.Base);
    public static List<Point> allPoints = new List<Point>();
    public static Point GetRandomPatrollPoint()
    {
        var patrollPoints = allPoints.Where(p => p.pointType != EPointType.Base).ToList();
        return patrollPoints[Random.Range(0, patrollPoints.Count)];
    }
}

