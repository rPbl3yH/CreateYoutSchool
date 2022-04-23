using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public abstract class PlacebleObject : MonoBehaviour, IPointerClickHandler
{
    protected BuildingSystem BuildSystem { get; private set; }
    public byte IdFloor;
    
    [SerializeField] protected TileBase Tile; //Тайлы будут использоваться для опознавания здания
    
    public virtual void Start()
    {
        BuildSystem = BuildingSystem.Current;
    }

    public abstract void OnPointerClick(PointerEventData eventData);
}
