using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action<byte, byte> BuildingSelected;
    public static Action<byte, byte> BuildingCreated;

    public static Action<SchoolPoints> BuildingPointsSelected;
    public static Action<SchoolPoints> BuildingPointsCreated;

    public static void OnBuidingCreated(byte SciencePoint, byte PhysicalPoint)
    {
        BuildingCreated?.Invoke(SciencePoint, PhysicalPoint);
        
    }
    
    public static void OnBuidingCreated(ref SchoolPoints schoolPoints)
    {
        BuildingPointsCreated?.Invoke(schoolPoints);
        
    }

    public static void OnBuildingSelected(byte SciencePoint, byte PhysicalPoint)
    {
        BuildingSelected?.Invoke(SciencePoint, PhysicalPoint);
    }
    public static void OnBuildingSelected(ref SchoolPoints schoolPoints)
    {
        BuildingPointsSelected?.Invoke(schoolPoints);
    }
    
    
}
