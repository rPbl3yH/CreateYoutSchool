using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public abstract class Placement : PlacebleObject
{
    protected GameObject CurrentPrefab;

    public override void OnPointerClick(PointerEventData eventData)
    {
        CurrentPrefab = BuildSystem.CurrentPrefab;
    }
}
