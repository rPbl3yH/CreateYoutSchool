using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public abstract class Placement : PlacebleObject
{
    protected GameObject CurrentPrefab;
    protected Building BuildingCreator;

    public override void OnPointerClick(PointerEventData eventData)
    {
        CurrentPrefab = BuildSystem.CurrentPrefab;
    }

    public virtual void SetBuildingCreator(Building creator)
    {
        BuildingCreator = creator;
    }
}
