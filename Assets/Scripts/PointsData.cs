using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsData : MonoBehaviour
{
    [SerializeField] PointsBarController _pointsBarController;
    
    public byte SciencePoints;
    public byte PhysicalPoints;

    private void Start()
    {
        EventManager.BuildingCreated += OnBuildingCreated;
    }

    public void OnBuildingCreated(byte sciencePointValue, byte physicalPointValue)
    {
        SciencePoints += sciencePointValue;
        PhysicalPoints += physicalPointValue;
        
        _pointsBarController.SetPoint(SciencePoints, PhysicalPoints);
    }
    
    
}
