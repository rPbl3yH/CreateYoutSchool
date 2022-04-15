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
    
    private PlacebleObject _objToPlace;
    private byte _currentIdFloor;
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

    public void InitializePlacements(Vector3 position)
    {
        ClearPlacement();
        CreatePlacements(GetPositionsForPlacement(position));
    }
    private Vector3 SnapGridPosition(Vector3 worldPosition)
    {
        Vector3Int cellPos = Gridlayout.WorldToCell(worldPosition);
        
        worldPosition = _grid.GetCellCenterWorld(cellPos);
        return worldPosition;
    }

    private Vector3[] GetPositionsForPlacement(Vector3 pos)
    {
        byte sizeCanTile = 5;

        Vector3[] array = new Vector3[sizeCanTile];
        array[0] = GetCheckPositionForPlacement(pos, Vector3.forward);
        array[1] = GetCheckPositionForPlacement(pos, Vector3.right);
        array[2] = GetCheckPositionForPlacement(pos, Vector3.back);
        array[3] = GetCheckPositionForPlacement(pos, Vector3.left);
        array[4] = GetCheckPositionForPlacement(pos, Vector3.zero);
        
        return array;
    }

    private Vector3 GetCheckPositionForPlacement(Vector3 pos, Vector3 offsetVector)
    {
        Vector3 otherTilePos = pos + offsetVector;
        Vector3Int otherCellPos = Gridlayout.WorldToCell(otherTilePos);
        if (offsetVector == Vector3.zero)
            return otherTilePos;
        
        if (_tilemapsFloors[_currentIdFloor].GetTile(otherCellPos) == whiteTile)
            return _falseVector;

        return otherTilePos;
    }
    
    private void CreatePlacements(Vector3[] positions)
    {
        var lastIdVec = positions.Length - 1;
        for (var index = 0; index < lastIdVec; index++)
        {
            var pos = positions[index];
            Debug.Log(pos);
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
        _currentIdFloor = idFloor;
        
        _tilemapsFloors[_currentIdFloor].SetTile(Gridlayout.WorldToCell(spawnPos), whiteTile);
        ClearPlacement();
    }

    private Vector3 GetOffsetFloor(byte idFloor)
    {
        return Vector3.up * idFloor;
    }

    private void InizialatePlacement(Vector3 pos, bool isNextLevel)
    {
        var currentIdDFloor = _currentIdFloor;
        if (isNextLevel)
        {
            if (currentIdDFloor + 1 < _tilemapsFloors.Length)
                currentIdDFloor++;
        }

        Vector3 position = SnapGridPosition(pos) + GetOffsetFloor(currentIdDFloor);
        
        GameObject obj = Instantiate(_placement, position, Quaternion.identity);
        obj.GetComponent<Placement>().IdFloor = currentIdDFloor;
        
        if(obj.GetComponent<Placement>())
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
    #endregion

}
