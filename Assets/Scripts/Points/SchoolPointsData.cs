using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public struct SchoolPointsData
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
    }

    public void Initialize()
    {
        Points = new Dictionary<TypePoints, int>();
        var typePoints = Enum.GetValues(typeof(TypePoints));
        foreach (TypePoints point in typePoints)
        {
            Points.Add(point, 0);
        }
    }
    
    public void Initialize(int[] points)
    {
        Points = new Dictionary<TypePoints, int>();
        var typePoints = Enum.GetValues(typeof(TypePoints));
        
        if (points.Length != typePoints.Length) Debug.Log("Values of length are not equal");
        
        for (int index = 0; index < typePoints.Length; index++)
        {
            Points.Add((TypePoints) typePoints.GetValue(index), points[index]);
        }
    }

    #endregion
    
    #region Info
    public void PrintInfo()
    {
        Debug.Log($"Science: {Points[TypePoints.Science]} " +
                  $"Physical: {Points[TypePoints.Physical]} " +
                  $"Musical: {Points[TypePoints.Musical]}");
    }
    #endregion
    
}
