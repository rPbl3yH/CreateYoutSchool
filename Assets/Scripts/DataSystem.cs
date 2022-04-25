using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
public class DataSystem
{
    public float[] PositionX { get; private set; }
    public float[] PositionY { get; private set; }
    public float[] PositionZ { get; private set; }
    
    public byte[] IdFloor { get; private set; }
    
    public byte[] IdPrefabBuilding { get; private set; }
    
    public int[] SciencePoints { get; private set; }
    public int[] PhysicalPoints { get; private set; }
    public int[] MusicalPoints { get; private set; }
    
    private ushort _countBuilding;

    public DataSystem(ushort countBuilding)
    {
        _countBuilding = countBuilding;
        Initialize();

        Building[] buildings = (Building[]) Object.FindObjectsOfType(typeof(Building));
        for (var index = 0; index < buildings.Length; index++)
        {
            var building = buildings[index];
            var position = building.transform.position;
            PositionX[index] = position.x;
            PositionY[index] = position.y;
            PositionZ[index] = position.z;

            IdFloor[index] = building.IdFloor;
            var schoolPoints = building.GetComponent<SchoolBuilding>().SchoolPoints.Points;

            if (schoolPoints.TryGetValue(TypePoints.Science, out var science))
            {
                SciencePoints[index] = science;
            }
            if (schoolPoints.TryGetValue(TypePoints.Physical, out var physical))
            {
                PhysicalPoints[index] = physical;
            }
            if (schoolPoints.TryGetValue(TypePoints.Musical, out var musical))
            {
                MusicalPoints[index] = musical;
            }
        }
    }

    private void Initialize()
    {
        InitializePosition();

        IdFloor = new byte[_countBuilding];
        
        InitializePoints();
    }

    private void InitializePosition()
    {
        PositionX = new float[_countBuilding];
        PositionY = new float[_countBuilding];
        PositionZ = new float[_countBuilding];
    }

    private void InitializePoints()
    {
        SciencePoints = new int[_countBuilding];
        PhysicalPoints = new int[_countBuilding];
        MusicalPoints = new int[_countBuilding];
    }
}
