using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsData : MonoBehaviour
{
    [SerializeField] PointsBarController _pointsBarController;

    public byte CurrentSciencePoints;
    public byte CurrentPhysicalPoints;
    
    public byte SciencePoints;
    public byte PhysicalPoints;

    private void Start()
    {
        EventManager.BuildingCreated += OnBuildingCreated;
        EventManager.BuildingSelected += OnBuildingSelected;
    }

    public void OnBuildingCreated(byte sciencePointValue, byte physicalPointValue)
    {
        SciencePoints += sciencePointValue;
        PhysicalPoints += physicalPointValue;
        
        _pointsBarController.SetPoints(SciencePoints, PhysicalPoints);
        _pointsBarController.SetActiveCurrentTexts(false);
    }

    public void OnBuildingSelected(byte sciencePointValue, byte physicalPointValue)
    {
        CurrentSciencePoints = sciencePointValue;
        CurrentPhysicalPoints = physicalPointValue;
        
        _pointsBarController.SetActiveCurrentTexts(true);
        _pointsBarController.SetCurrentPoints(CurrentSciencePoints,CurrentPhysicalPoints);
    }
    
    
}
