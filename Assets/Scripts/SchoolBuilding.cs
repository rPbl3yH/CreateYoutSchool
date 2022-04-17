using UnityEngine;

public class SchoolBuilding : Building
{
    public PointsBarController PointsController;
    public void Initialize(byte sciencePoint, byte physicalPoint)
    {
        _sciencePoints = sciencePoint;
        _physicalPoints = physicalPoint;

    }
    
    private byte _sciencePoints;
    private byte _physicalPoints;

}
