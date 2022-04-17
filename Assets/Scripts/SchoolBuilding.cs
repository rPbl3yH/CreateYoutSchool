using UnityEngine;
using UnityEngine.EventSystems;

public class SchoolBuilding : Building
{
    public void Initialize(byte sciencePoint, byte physicalPoint)
    {
        _sciencePoints = sciencePoint;
        _physicalPoints = physicalPoint;

    }
    
    private byte _sciencePoints;
    private byte _physicalPoints;

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        EventManager.OnBuildingSelected(_sciencePoints, _physicalPoints);
    }
}
