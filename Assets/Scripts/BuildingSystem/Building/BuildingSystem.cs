using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{ 
    public static BuildingSystem Current { get; private set; }

    [Header("Building On Start")]
    [SerializeField] GameObject _startPrefab;
    [Header("Placement")]
    [SerializeField] private GameObject _placementPrefab;
    
    [Header("Maps Setting")]
    [SerializeField] private Tilemap[] _tilemapsFloors;
    [SerializeField] private GridLayout _gridlayout;
    
    [Header("Types Of Tile")]
    [SerializeField] private TileBase _buildingTile;
    
    [Header("Particles")]
    [SerializeField] private ParticleSystem _particleAfterBuilt;

    [Header("Loading and Saving")]
    [SerializeField] private LoadSaveDataSystem _loadSaveDataSystem;
    
    public GameObject CurrentPrefab { get; private set; }
    public GameObject StartPrefab { get; private set; }

    private Grid _grid;
    private Building _currentBuilding;
    private PlacebleObject _objToPlace;
    private List<Placement> _currentPlacements = new List<Placement>();
    private bool _isHaveUpDirection = false;
    private ParticleSystem _currentParticle;
    public ushort CountBuilding { get; private set; }
    private readonly Vector3[] _directionForBuilding = {Vector3.forward,Vector3.right, Vector3.back, Vector3.left, Vector3.up};

    

    #region UnityMethods

    private void Awake()
    {
        Current = this;
        _grid = _gridlayout.GetComponent<Grid>();
        CurrentPrefab = _startPrefab;
    }

    private void Start()
    {
        var cellPos = _gridlayout.WorldToCell(transform.position);
        Instantiate(_placementPrefab, _grid.GetCellCenterWorld(cellPos) , Quaternion.identity);
    }

    #endregion

    #region Utils

    private Vector3 SnapGridPosition(Vector3 worldPosition, byte idFloor)
    {
        Vector3Int cellPos = _gridlayout.WorldToCell(worldPosition);
        worldPosition = _grid.GetCellCenterWorld(cellPos);
        return worldPosition;
    }

    private Vector3 GetOffsetFloor(byte idFloor) => new Vector3(0, 0.4f, 0) * idFloor; //Пока здесь просто какой-то вектор. Я изменю его на другой)

    private Vector3 GetOffsetToPos(Vector3 pos, byte currentIdDFloor)
    {
        return SnapGridPosition(pos, currentIdDFloor) + GetOffsetFloor(currentIdDFloor);
    }
    #endregion

    #region Building System

    #region Initializing

    public void InitializeGameObject(GameObject prefab, Vector3 spawnPos, byte idFloor)
    {
        Vector3 position = GetOffsetToPos(spawnPos, idFloor);
        GameObject obj = CreateAndGetBuilding(prefab,position);
        CountBuilding++;
        
        InitializeIdFloorOfBuilding(obj, idFloor);
        
        InitializeTileOfBuilding(spawnPos, idFloor);

        InitializeSchoolPoints(obj);

        Vector3 spawnPosParticle = new Vector3(position.x,position.y - obj.transform.localScale.y / 2,position.z);
        CreateParticle(spawnPosParticle);
        
        ClearAllPlacements();
        
        if(CountBuilding % 5 == 0)
            _loadSaveDataSystem.SaveData();
    }
    
    public void InitializeGameObject(GameObject prefab, Vector3 spawnPos, byte idFloor, int[] points)
    {
        Vector3 position = GetOffsetToPos(spawnPos, idFloor);
        GameObject obj = CreateAndGetBuilding(prefab,position);
        CountBuilding++;
        
        InitializeIdFloorOfBuilding(obj, idFloor);
        
        InitializeTileOfBuilding(spawnPos, idFloor);

        InitializeSchoolPoints(obj, points);

        Vector3 spawnPosParticle = new Vector3(position.x,position.y - obj.transform.localScale.y / 2,position.z);
        CreateParticle(spawnPosParticle);
        
        ClearAllPlacements();
        
        if(CountBuilding % 5 == 0)
            _loadSaveDataSystem.SaveData();
    }

    private GameObject CreateAndGetBuilding(GameObject prefab, Vector3 position)
    {
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        return obj;
    }

    private void InitializeIdFloorOfBuilding(GameObject obj,byte idFloor)
    {
        if (obj.TryGetComponent(out Building building))
        {
            building.IdFloor = idFloor;
        }
    }

    private void InitializeTileOfBuilding(Vector3 pos, byte idFloor)
    {
        var curTilemap = _tilemapsFloors[idFloor];
        curTilemap.SetTile(curTilemap.WorldToCell(pos), _buildingTile);
    }

    #region Tile

    private void InitializeNoneTile(Vector3 pos, byte idFloor)
    {
        var curTilemap = _tilemapsFloors[idFloor];
        var currentCellPos = curTilemap.WorldToCell(pos);
        curTilemap.SetTile(currentCellPos, null);
    }

    #endregion

    #region Points

    private void InitializeSchoolPoints(GameObject obj)
    {
        if (!obj.TryGetComponent(out SchoolBuilding schoolBuilding)) return;
        
        SchoolPointsData schoolPoints = new SchoolPointsData();
        schoolPoints.Initialize(0,10);
        schoolBuilding.Initialize(ref schoolPoints);
        EventManager.OnBuildingCreated(ref schoolPoints);
    }
    private void InitializeSchoolPoints(GameObject obj, int[] points)
    {
        if (!obj.TryGetComponent(out SchoolBuilding schoolBuilding)) return;
        
        SchoolPointsData schoolPoints = new SchoolPointsData();
        schoolPoints.Initialize(points);
        schoolBuilding.Initialize(ref schoolPoints);
        EventManager.OnBuildingCreated(ref schoolPoints);
    }

    #endregion

    #region Particle

    private void CreateParticle(Vector3 pos)
    {
        _particleAfterBuilt.transform.position = pos;
        _particleAfterBuilt.Play();
    }

    #endregion

    #region Placements

    public void InitializePlacements(Building building)
    {
        ClearAllPlacements();
        SetCurrentBuilding(building);

        Vector3[] positions = GetPositionsForPlacements(_currentBuilding);
        
        CreatePlacements(positions, _currentBuilding.IdFloor);
    }
    
    private Vector3[] GetPositionsForPlacements(Building building)
    {
        var permission = building.GetPermissionForBuilding();
        byte sizeCanTile = (byte)permission.Length;
        List<Vector3> positions = new List<Vector3>();
        
        for (int i = 0; i < sizeCanTile; i++)
        {
            if(!permission[i])
                continue;
            
            var position = GetPositionForPlacement(building.transform.position, _directionForBuilding[i]);
            Vector3Int otherCellPos = _tilemapsFloors[building.IdFloor].WorldToCell(position);

            if (i != sizeCanTile - 1) //Последний элемент это вектор вверх, поэтому делается проверка
            {
                if(!IsBuildForPlacement(otherCellPos, building.IdFloor))
                    continue;
            }
            else
            {
                if (!IsBuildForUpPlacement(otherCellPos, building.IdFloor))
                    continue;
                _isHaveUpDirection = true;  
            }

            positions.Add(position);
        }
        
        return positions.ToArray();
    }

    private bool IsBuildForUpPlacement(Vector3Int pos, byte idFloor)
    {
        byte nextIdFloor = idFloor + 1 < _tilemapsFloors.Length ? ++idFloor : idFloor ;
        return !IsHaveBuilding(pos, nextIdFloor);
    }
    private bool IsBuildForPlacement(Vector3Int position, byte idFloor)
    {
        if (idFloor - 1 >= 0)
        {
            byte underIdFloor = (byte)(idFloor - 1);
            if (!IsNineBuildingsUnderPlacement(position, underIdFloor))//Проверка на наличие зданий в 9 клетках под выбранным             
                return false;
        }

        if (IsHaveBuilding(position, idFloor)) //Проверка на наличие другого здания
            return false;

        return true; 
    }

    private void InitializePlacement(Vector3 pos, byte idFloor)
    {
        Vector3 position = GetOffsetToPos(pos,idFloor);
        
        GameObject obj = Instantiate(_placementPrefab, position, Quaternion.identity);
        Placement placement = obj.GetComponent<Placement>();
        placement.IdFloor = idFloor;

        _currentPlacements.Add(placement);
    }

    #endregion
    

    #endregion

    #region Delete

    public void DeleteCurrentObj()
    {
        CountBuilding--;
        ClearAllPlacements();
        
        var schoolPoints = _currentBuilding.GetComponent<SchoolBuilding>().SchoolPoints;
        EventManager.OnBuildingDeleted(ref schoolPoints);

        if (CountBuilding == 0)
        {
            InitializePlacement(_tilemapsFloors[0].GetCellCenterWorld(Vector3Int.zero), 0);
        }

        InitializeNoneTile(_currentBuilding.transform.position, _currentBuilding.IdFloor);

        Destroy(_currentBuilding.gameObject);
    }

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
            }
            
            InitializePlacement(pos, idFloor);
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

    private bool IsHaveBuilding(Vector3Int pos, byte idFloor) => _tilemapsFloors[idFloor].GetTile(pos) == _buildingTile;

    private void ClearAllPlacements()
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

    private void DisactiveCurrentBuilding(Building building)
    {
        //Здесь логика выделения объекта должна прописана быть
        //building.GetComponent<Renderer>().material.color = Color.white;
    } 

    #endregion
    #endregion
}
