using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Current;

    [SerializeField] GameObject _mainPrefab;

    [SerializeField] private Tilemap[] _tilemapsFloors;
    [SerializeField] GridLayout Gridlayout;
    Grid _grid;

    public GameObject CurrentPrefab;
    public Building CurrentBuilding;
    
    private PlacebleObject _objToPlace;
     
    private Vector3 _falseVector = new Vector3(0,-1,0);
    private List<Placement> _currentPlacements = new List<Placement>();
    [SerializeField] TileBase whiteTile;
    [SerializeField] TileBase blueTile;
    [SerializeField] GameObject _placement;
    
    #region UnityMethods

    private void Awake()
    {
        Current = this;
        _grid = Gridlayout.GetComponent<Grid>();
        CurrentPrefab = _mainPrefab;
    }

    private void Start()
    {
        Instantiate(_placement, transform.position, Quaternion.identity);
    }

    #endregion

    #region Utils

    
    private Vector3 SnapGridPosition(Vector3 worldPosition)
    {
        Vector3Int cellPos = Gridlayout.WorldToCell(worldPosition);
        
        worldPosition = _grid.GetCellCenterWorld(cellPos);
        return worldPosition;
    }

    private Vector3[] GetPositionsForPlacement(Vector3 pos, byte idFloor)
    {
        byte sizeCanTile = 5;

        Vector3[] array = new Vector3[sizeCanTile];
        array[0] = CheckPositionForPlacement(pos, Vector3.forward, idFloor);
        array[1] = CheckPositionForPlacement(pos, Vector3.right, idFloor);
        array[2] = CheckPositionForPlacement(pos, Vector3.back, idFloor);
        array[3] = CheckPositionForPlacement(pos, Vector3.left, idFloor);
        array[4] = CheckPositionForPlacement(pos, Vector3.zero, idFloor);
        
        return array;
    }

    private Vector3 CheckPositionForPlacement(Vector3 pos, Vector3 offsetVector, byte idFloor)
    { 
        Vector3 otherTilePos = pos + offsetVector;
        Vector3Int otherCellPos = _tilemapsFloors[idFloor].WorldToCell(otherTilePos);
        //Проеврка на наличие здание под выбранным
        
        if (idFloor - 1 >= 0)
        {
            byte idFloorUnderBuilding = (byte) (idFloor - 1);
            if (!IsHaveBuildingUnderPlacement(otherCellPos, idFloorUnderBuilding))
            {
                return _falseVector;
            }
                
        }

        //Проверка на этаже выше
        if (offsetVector == Vector3.zero && idFloor + 1 < _tilemapsFloors.Length)
            idFloor++;
        
        //Проверка на наличие другого здания
        if (IsHaveBuilding(otherCellPos, idFloor))
            return _falseVector;

        return otherTilePos;
    }

    private bool IsHaveBuildingUnderPlacement(Vector3Int otherCellPos, byte idUnderFloor)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                //Z пишу в Y, так как GRID имеет направление XZY
                Vector3Int otherUnderCellPos = otherCellPos + new Vector3Int(x, z, 0);
                if (IsHaveBuilding(otherUnderCellPos,idUnderFloor))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsHaveBuilding(Vector3Int pos, byte idFloor)
    {
        return _tilemapsFloors[idFloor].GetTile(pos) == whiteTile;
    }
    
    private void CreatePlacements(Vector3[] positions)
    {
        var lastIdVec = positions.Length - 1;
        for (var index = 0; index < lastIdVec; index++)
        {
            var pos = positions[index];
            if (pos == _falseVector)
                continue;

            InizialatePlacement(pos, false);
        }
        if(positions[lastIdVec] != _falseVector)
            InizialatePlacement(positions[lastIdVec], true);
        
    }

    #endregion

    #region Building System

    public void InizialateGameObject(GameObject prefab, Vector3 spawnPos, byte idFloor)
    {
        Vector3 position = SnapGridPosition(spawnPos) + GetOffsetFloor(idFloor);
        
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        obj.GetComponent<Building>().IdFloor = idFloor;
        var curTilemap = _tilemapsFloors[idFloor];
        curTilemap.SetTile(curTilemap.WorldToCell(spawnPos), whiteTile);
        ClearPlacement();
    }

    private Vector3 GetOffsetFloor(byte idFloor)
    {
        return Vector3.up * idFloor;
    }

    public void InitializePlacements(Vector3 position, Building building)
    {
        ClearPlacement();
        SetCurrentBuilding(building);
        CreatePlacements(GetPositionsForPlacement(position, building.IdFloor));
    }
    
    private void InizialatePlacement(Vector3 pos, bool isNextLevel)
    {
        var currentIdDFloor = CurrentBuilding.IdFloor;
        if (isNextLevel)
        {
            if (currentIdDFloor + 1 < _tilemapsFloors.Length)
                currentIdDFloor++;
        }

        Vector3 position = SnapGridPosition(pos) + GetOffsetFloor(currentIdDFloor);
        
        GameObject obj = Instantiate(_placement, position, Quaternion.identity);
        obj.GetComponent<Placement>().IdFloor = currentIdDFloor;
        
        _currentPlacements.Add(obj.GetComponent<Placement>());
    }

    private void ClearPlacement()
    {
        foreach (var placement in _currentPlacements)
        {
            Destroy(placement.gameObject);
        }
        _currentPlacements.Clear();
    }

    public void ClearCurrentBuilding()
    {
        if(CurrentBuilding)
            DisactiveCurrentBuilding(CurrentBuilding);
        CurrentBuilding = null;
    }
    
    private void SetCurrentBuilding(Building building)
    {
        if (CurrentBuilding)
        {
            DisactiveCurrentBuilding(CurrentBuilding);
        }
        CurrentBuilding = building;
    }

    private void DisactiveCurrentBuilding(Building building)
    {
        building.GetComponent<Renderer>().material.color = Color.white;
    }
    
    #endregion

}
