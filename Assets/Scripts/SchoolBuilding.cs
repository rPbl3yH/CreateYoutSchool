using UnityEngine;

public class SchoolBuilding : Building
{
    public PointsBarController PointsController;
    public void Initialize(byte sciencePoint, byte physicalPoint)
    {
        _sciencePoints = sciencePoint;
        _physicalPoints = physicalPoint;
        PointsController = PointsBarController.Current;
        
        SendToController();
    }
    
    private byte _sciencePoints;
    private byte _physicalPoints;

    private void SendToController()
    {
        PointsController.PhysicalPoints += _physicalPoints;
        PointsController.SciencePoints += _sciencePoints;
    }
}
