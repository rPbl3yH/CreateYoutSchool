using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.Video;

public abstract class PlacebleObject : MonoBehaviour, IPointerClickHandler
{
    
    [NonSerialized] public BuildingSystem BuildSystem;
    [NonSerialized] public byte IdFloor;
    [SerializeField] protected TileBase Tile;
    
    public virtual void Start()
    {
        BuildSystem = BuildingSystem.Current;
    }
    
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Click object");
        //Здесь будет выделение объекта
        gameObject.GetComponent<Renderer>().material.color = Color.magenta;
        
    }
}
