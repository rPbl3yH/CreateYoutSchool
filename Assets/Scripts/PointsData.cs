using System.Collections.Generic;
using UnityEngine;

public enum TypePoints
{
    Science,
    Physical,
    Musical
}
public class PointsData : MonoBehaviour
{
    [SerializeField] PointsBarController _pointsBarController;

    private Dictionary<TypePoints, int> dicPoints;
    
    private SchoolPoints _schoolPoints;
    private SchoolPoints _currentSchoolPoints;

    private void Start()
    {
        _schoolPoints.Initialize();
        _currentSchoolPoints.Initialize();
        
        EventManager.BuildingCreated += OnBuildingCreated;
        EventManager.BuildingSelected += OnBuildingSelected;
    }
    
    public void OnBuildingCreated(ref SchoolPoints schoolPoints)
    {
        foreach (var pair in schoolPoints.Points)
        {
            _schoolPoints.Points[pair.Key] += pair.Value;
        }

        _pointsBarController.SetPoints(ref _schoolPoints);
        _pointsBarController.SetActiveCurrentTexts(false);
    }
    
    public void OnBuildingSelected(ref SchoolPoints schoolPoints)
    {
        Debug.Log(schoolPoints.Points);
        foreach (var pair in schoolPoints.Points)
        {
            _currentSchoolPoints.Points[pair.Key] = pair.Value;
        }
        
        _pointsBarController.SetActiveCurrentTexts(true);
        _pointsBarController.SetCurrentPoints(ref _currentSchoolPoints);
    }
    
    
}
