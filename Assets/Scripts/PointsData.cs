using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PointsData : MonoBehaviour
{
    [SerializeField] PointsBarController _pointsBarController;

    private SchoolPoints _schoolPoints;
    private SchoolPoints _currentSchoolPoints;
    public int CurrentSciencePoints;
    public int CurrentPhysicalPoints;
    
    public byte SciencePoints;
    public byte PhysicalPoints;

    private void Start()
    {
        EventManager.BuildingPointsCreated += OnBuildingCreated;
        EventManager.BuildingPointsSelected += OnBuildingSelected;
    }

    public void OnBuildingCreated(byte sciencePointValue, byte physicalPointValue)
    {
        SciencePoints += sciencePointValue;
        PhysicalPoints += physicalPointValue;
        
        _pointsBarController.SetPoints(SciencePoints, PhysicalPoints);
        _pointsBarController.SetActiveCurrentTexts(false);
    }
    public void OnBuildingCreated(SchoolPoints schoolPoints)
    {
        
        //_schoolPoints = schoolPoints;
        _schoolPoints.Science += schoolPoints.Science;
        _schoolPoints.Physical += schoolPoints.Science;

        _pointsBarController.SetPoints(ref _schoolPoints);
        _pointsBarController.SetActiveCurrentTexts(false);
    }

    public void OnBuildingSelected(byte sciencePointValue, byte physicalPointValue)
    {
        CurrentSciencePoints = sciencePointValue;
        CurrentPhysicalPoints = physicalPointValue;
        
        _pointsBarController.SetActiveCurrentTexts(true);
        _pointsBarController.SetCurrentPoints(ref _schoolPoints);
    }
    public void OnBuildingSelected(SchoolPoints schoolPoints)
    {
        _currentSchoolPoints.Science = schoolPoints.Science;
        _currentSchoolPoints.Physical = schoolPoints.Physical;
        
        _pointsBarController.SetActiveCurrentTexts(true);
        _pointsBarController.SetCurrentPoints(ref _currentSchoolPoints);
    }
    
    
}
