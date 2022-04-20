using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static BuildingDelegateSelected BuildingSelected;
    public static BuildingDelegateCreated BuildingCreated;
    public delegate void BuildingDelegateSelected(ref SchoolPoints schoolPoints);
    public delegate void BuildingDelegateCreated(ref SchoolPoints schoolPoints);
    
    public static void OnBuidingCreated(ref SchoolPoints schoolPoints)
    {
        BuildingCreated?.Invoke(ref schoolPoints);
    }

    public static void OnBuildingSelected(ref SchoolPoints schoolPoints)
    {
        BuildingSelected?.Invoke(ref schoolPoints);
    }
    
    
}
