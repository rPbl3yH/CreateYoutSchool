using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Current;

    [SerializeField] GameObject _mainPrefab;
    
    
    [SerializeField] private Tilemap _firstFloorTilemap;
    [SerializeField] private Tilemap _secondFloorTlemap;

    [SerializeField] GridLayout Gridlayout;
    Grid _grid;

    public GameObject CurrentPrefab;
    [SerializeField] Vector3 offset;

    private PlacebleObject _objToPlace;
    private Vector3 _falseVector = new Vector3(0,-1,0);
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

    private Vector3[] GetPositionsForBuilding(Vector3 pos)
    {
        byte sizeCanTile = 5;

        Vector3[] array = new Vector3[sizeCanTile];
        array[0] = GetCheckPositionOnBuilding(pos, Vector3.forward);
        array[1] = GetCheckPositionOnBuilding(pos, Vector3.right);
        array[2] = GetCheckPositionOnBuilding(pos, Vector3.back);
        array[3] = GetCheckPositionOnBuilding(pos, Vector3.left);
        array[4] = GetCheckPositionOnBuilding(pos, Vector3.up);
        
        return array;
    }

    private Vector3 GetCheckPositionOnBuilding(Vector3 pos, Vector3 offsetVector)
    {
        Vector3 otherTilePos = pos + offsetVector;
        Vector3Int otherCellPos = Gridlayout.WorldToCell(otherTilePos);
        
        //if(offsetVector == Vector3.zero && _secondFloorTlemap.GetTile(otherCellPos) )

        if (_firstFloorTilemap.GetTile(otherCellPos) == whiteTile)
            return _falseVector;

        return otherTilePos;
    }

    #endregion

    #region Building System

    public void InizializeGameObject(GameObject prefab, Vector3 spawnPos)
    {
        Vector3 position = SnapGridPosition(spawnPos);

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        _firstFloorTilemap.SetTile(Gridlayout.WorldToCell(spawnPos), whiteTile);
        
        DrawCanTileForBuilding(GetPositionsForBuilding(position));
    }

    void InizializePlacement(Vector3 pos)
    {
        Vector3 position = SnapGridPosition(pos);
        GameObject obj = Instantiate(_placement, position, Quaternion.identity);
    }

    public void DrawCanTileForBuilding(Vector3[] positions)
    {
        foreach (var pos in positions)
        {
            if(pos == _falseVector)
                continue;
            
            InizializePlacement(pos);
        }
    }


    private bool CanBePlaced(PlacebleObject placebleObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = Gridlayout.WorldToCell(_objToPlace.GetStartPosition());
        area.size = placebleObject.Size;

        area.size = new Vector3Int(area.size.x + 1, area.size.y + 1, area.size.z);

        TileBase[] baseArray = GetTilesBlock(area, _firstFloorTilemap);

        foreach (var tile in baseArray)
        {
            if (tile == whiteTile)
            {
                return false;
            }
        }

        return true;
    }

    private TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;
        area.size = _objToPlace.Size;
        area.size = new Vector3Int(area.size.x + 1, area.size.y + 1, area.size.z);

        foreach (var vec in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(vec.x, vec.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;

    }


    #endregion

}
