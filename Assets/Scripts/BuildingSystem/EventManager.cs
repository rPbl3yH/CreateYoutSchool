using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static BuildingDelegateSelected BuildingSelected;
    public static BuildingDelegateCreated BuildingCreated;
    public static BuildingDelegateDeleted BuildingDeleted;
    public delegate void BuildingDelegateSelected(ref SchoolPointsData schoolPoints);
    public delegate void BuildingDelegateCreated(ref SchoolPointsData schoolPoints);
    public delegate void BuildingDelegateDeleted(ref SchoolPointsData schoolPoints);
    
    public static void OnBuildingCreated(ref SchoolPointsData schoolPoints)
    {
        BuildingCreated?.Invoke(ref schoolPoints);
    }

    public static void OnBuildingSelected(ref SchoolPointsData schoolPoints)
    {
        BuildingSelected?.Invoke(ref schoolPoints);
    }

    public static void OnBuildingDeleted(ref SchoolPointsData schoolPoints)
    {
        BuildingDeleted?.Invoke(ref schoolPoints);
    }
    
    
}
