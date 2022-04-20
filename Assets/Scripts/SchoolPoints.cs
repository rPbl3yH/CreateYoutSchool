using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public struct SchoolPoints
{
    public Dictionary<TypePoints, int> Points;

    #region Initializing
    public void Initialize(int minValue, int maxValue)
    {
        Points = new Dictionary<TypePoints, int>();
        var typePoints = Enum.GetValues(typeof(TypePoints));
        foreach (TypePoints point in typePoints)
        {
            Points.Add(point, Random.Range(minValue,maxValue));
        }
        PrintInfo();
    }

    public void Initialize()
    {
        Points = new Dictionary<TypePoints, int>();
        var typePoints = Enum.GetValues(typeof(TypePoints));
        foreach (TypePoints point in typePoints)
        {
            Points.Add(point, 0);
        }
        PrintInfo();
    }

    #endregion
    
    #region Info
    public void PrintInfo()
    {
        Debug.Log($"Science: {Points[TypePoints.Science]} Physical: " +
                  $"{Points[TypePoints.Physical]} " +
                  $"Musical: {Points[TypePoints.Musical]}");
    }
    #endregion
    
}
