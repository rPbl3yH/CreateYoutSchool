using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SchoolBuilding : Building
{
    private SchoolPoints _schoolPoints;

    public override void Start()
    {
        base.Start();
        
        Debug.Log(_schoolPoints.Points);
    }

    public void Initialize(ref SchoolPoints schoolPoints)
    {
        _schoolPoints = schoolPoints;
        _schoolPoints.PrintInfo();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        EventManager.OnBuildingSelected(ref _schoolPoints);
    }
}
