using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SchoolBuilding : Building
{
    public SchoolPointsData SchoolPoints { get; private set; }

    public void Initialize(ref SchoolPointsData schoolPoints)
    {
        SchoolPoints = schoolPoints;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        var schoolPoints = SchoolPoints;
        EventManager.OnBuildingSelected(ref schoolPoints);
    }
}
