using UnityEngine;

public class SchoolBuilding : Building
{
    public void Initialize(byte sciencePoint, byte physicalPoint)
    {
        _sciencePoint = sciencePoint;
        _physicalPoint = physicalPoint;
    }
    
    private byte _sciencePoint;
    private byte _physicalPoint;
    
    
}
