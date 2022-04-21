using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SchoolBuilding : Building
{
    private SchoolPoints _schoolPoints;
    
    public void Initialize(ref SchoolPoints schoolPoints)
    {
        _schoolPoints = schoolPoints;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        EventManager.OnBuildingSelected(ref _schoolPoints);
    }
}
