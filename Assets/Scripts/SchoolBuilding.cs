using UnityEngine;
using UnityEngine.EventSystems;

public class SchoolBuilding : Building
{
    public void Initialize(byte sciencePoint, byte physicalPoint)
    {
        SchoolPoints.Musical = 0;
        
        _sciencePoints = sciencePoint;
        _physicalPoints = physicalPoint;

    }

    public void InitializePoints(ref SchoolPoints schoolPoints)
    {
        SchoolPoints = schoolPoints;
        Debug.Log(SchoolPoints.Musical);
        Debug.Log(SchoolPoints.Physical);
        Debug.Log(SchoolPoints.Science);
    }

    protected SchoolPoints SchoolPoints;
    
    private byte _sciencePoints;
    private byte _physicalPoints;

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        EventManager.OnBuildingSelected(ref SchoolPoints);
    }
}
