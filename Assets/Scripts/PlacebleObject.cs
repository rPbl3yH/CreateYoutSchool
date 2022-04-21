using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public abstract class PlacebleObject : MonoBehaviour, IPointerClickHandler
{
    
    [NonSerialized] public BuildingSystem BuildSystem;
    public byte IdFloor;
    
    [SerializeField] protected TileBase Tile;
    
    public virtual void Start()
    {
        BuildSystem = BuildingSystem.Current;
    }

    public abstract void OnPointerClick(PointerEventData eventData);
}
