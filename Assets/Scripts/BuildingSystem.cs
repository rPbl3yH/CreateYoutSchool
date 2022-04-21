using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    private Vector3[] _directionForBuilding = {Vector3.forward,Vector3.right, Vector3.back, Vector3.left, Vector3.up};
    public static BuildingSystem Current;

    [SerializeField] GameObject _mainPrefab;
    
    [SerializeField] private Tilemap[] _tilemapsFloors;
    [SerializeField] GridLayout Gridlayout;
    Grid _grid;

    public GameObject CurrentPrefab;
    private Building _currentBuilding;
    
    private PlacebleObject _objToPlace;
     
    private Vector3 _falseVector = new Vector3(0,-1,0);
    private List<Placement> _currentPlacements = new List<Placement>();
    private bool _isHaveUpDirection = false;
    [SerializeField] TileBase whiteTile;
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

    private Vector3 GetOffsetFloor(byte idFloor) => new Vector3(0, 0.6f, 0) * idFloor; //Пока здесь просто какой-то вектор. Я изменю его на другой)

    #endregion

    #region Building System

    #region Inizialating

    public void InizialateGameObject(GameObject prefab, Vector3 spawnPos, byte idFloor)
    {
        Vector3 position = GetOffsetToPos(spawnPos, idFloor);
        
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        obj.GetComponent<Building>().IdFloor = idFloor;
        
        var curTilemap = _tilemapsFloors[idFloor];
        Debug.Log("CURTILEMAP ID FLOOR " + idFloor);
        curTilemap.SetTile(curTilemap.WorldToCell(spawnPos), whiteTile);
        
        
        if (obj.TryGetComponent(out SchoolBuilding schoolBuilding))
        {
            SchoolPoints schoolPoints = new SchoolPoints();
            schoolPoints.Initialize(0,10);
            schoolBuilding.Initialize(ref schoolPoints);
            EventManager.OnBuidingCreated(ref schoolPoints);
        }
        
        ClearPlacement();
    }
    
    public void InitializePlacements(Building building)
    {
        ClearPlacement();
        SetCurrentBuilding(building);

        Vector3[] positions = GetPositionsForPlacement(_currentBuilding);
        
        CreatePlacements(positions, _currentBuilding.IdFloor);
    }
    
    private Vector3[] GetPositionsForPlacement(Building building)
    {
        var permission = building.GetPermissionForBuilding();
        byte sizeCanTile = (byte)permission.Length;
        List<Vector3> positions = new List<Vector3>();
        
        for (int i = 0; i < sizeCanTile; i++)
        {
            var position = GetPositionForPlacement(building.transform.position, _directionForBuilding[i]);
            Vector3Int otherCellPos = _tilemapsFloors[building.IdFloor].WorldToCell(position);

            if (i != sizeCanTile - 1) //Последний элемент это вектор вверх, поэтому делается проверка
            {
                if(!IsBuildForPlacement(otherCellPos, building.IdFloor))
                    continue;
            }
            else
            {
                if (IsBuildForUpPlacement(otherCellPos, building.IdFloor))
                {
                    _isHaveUpDirection = true;  
                    Debug.Log("_isHaveUpDirection TRUE");
                }   
            }

            positions.Add(position);
        }
        
        return positions.ToArray();
    }

    private bool IsBuildForUpPlacement(Vector3Int pos, byte idFloor)
    {
        byte nextIdFloor = idFloor + 1 < _tilemapsFloors.Length ? ++idFloor : idFloor ;
        Debug.Log("NextIdFloor " + nextIdFloor);
        return !IsHaveBuilding(pos, nextIdFloor);
    }
    private bool IsBuildForPlacement(Vector3Int position, byte idFloor)
    {
        if (idFloor - 1 >= 0)                                                                                           
        {
            byte idFloorUnderBuilding = (byte) (idFloor - 1);
            if (!IsNineBuildingsUnderPlacement(position, idFloorUnderBuilding))                                         //Проверка на наличие зданий
                                                                                                                        //в 9 клетках под выбранным
            {
                return false;
            }
        }
        
        if (IsHaveBuilding(position, idFloor))                                                                          //Проверка на наличие другого здания
            return false;

        return true; 
    }

    private void InizialatePlacement(Vector3 pos, byte idFloor)
    {
        Vector3 position = GetOffsetToPos(pos,idFloor);
        
        GameObject obj = Instantiate(_placement, position, Quaternion.identity);
        Placement placement = obj.GetComponent<Placement>();
        placement.IdFloor = idFloor;

        _currentPlacements.Add(placement);
    }

    private Vector3 GetOffsetToPos(Vector3 pos, byte currentIdDFloor) => SnapGridPosition(pos) + GetOffsetFloor(currentIdDFloor);

    #endregion

    #region Placement

    private void CreatePlacements(Vector3[] positions, byte idFloor)
    {
        var lastIndex = positions.Length - 1;
        for (var index = 0; index <= lastIndex; index++)
        {
            var pos = positions[index];
            if (index == lastIndex && _isHaveUpDirection)
            {
                idFloor++;
                Debug.Log("idFloor: " + idFloor);
            }
            
            InizialatePlacement(pos, idFloor);
        }
    }

    private Vector3 GetPositionForPlacement(Vector3 pos, Vector3 offsetVector)
    { 
        Vector3 otherTilePos = pos + offsetVector;
        return otherTilePos;
    }
    
    private bool IsNineBuildingsUnderPlacement(Vector3Int otherCellPos, byte idUnderFloor)
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

    private bool IsHaveBuilding(Vector3Int pos, byte idFloor) => _tilemapsFloors[idFloor].GetTile(pos) == whiteTile;

    private void ClearPlacement()
    {
        _isHaveUpDirection = false;
        foreach (var placement in _currentPlacements) Destroy(placement.gameObject);
        _currentPlacements.Clear();
    }

    #endregion
    
    #region Select Building
    public void SetCurrentPrefab(GameObject value) => CurrentPrefab = value;
    public void ClearCurrentBuilding()
    {
        if(_currentBuilding)
            DisactiveCurrentBuilding(_currentBuilding);
        _currentBuilding = null;
    }
    
    private void SetCurrentBuilding(Building building)
    {
        if (_currentBuilding) DisactiveCurrentBuilding(_currentBuilding);
        _currentBuilding = building;
    }

    private void DisactiveCurrentBuilding(Building building) => building.GetComponent<Renderer>().material.color = Color.white;

    #endregion
    #endregion

    

}
