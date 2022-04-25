using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSaveDataSystem : MonoBehaviour
{
    [SerializeField] private bool _isLoading;
    private void Start()
    {
        LoadData();
    }

    public void SaveData()
    {
        BinarySavingSystem.SaveData();
    }

    public void LoadData()
    {
        if(!_isLoading)
            return;
        DataSystem dataSystem = BinarySavingSystem.LoadData();
        if(dataSystem == null)
            return;

        for (int index = 0; index < dataSystem.PositionX.Length; index++)
        {
            Vector3 spawnPos = new Vector3(
                dataSystem.PositionX[index], 
                dataSystem.PositionY[index],
                dataSystem.PositionZ[index]);
            
            byte idFloor = dataSystem.IdFloor[index];
            GameObject obj = BuildingSystem.Current.CurrentPrefab;

            int[] points = GetPointsFromData(dataSystem, index);
            
            SchoolPointsData schoolPointsData = new SchoolPointsData();
            
            schoolPointsData.Initialize(points);
            
            BuildingSystem.Current.InitializeGameObject(obj, spawnPos, idFloor, points);
        }
    }

    private int[] GetPointsFromData(DataSystem dataSystem, int id)
    {
        List<int> points = new List<int>();
        points.Add(dataSystem.SciencePoints[id]);
        points.Add(dataSystem.PhysicalPoints[id]);
        points.Add(dataSystem.MusicalPoints[id]);

        return points.ToArray();
    }
}
