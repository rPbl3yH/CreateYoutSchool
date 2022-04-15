using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public abstract class Placement : PlacebleObject
{
    protected GameObject CurrentPrefab;

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        CurrentPrefab = BuildSystem.CurrentPrefab;
    }

    // public virtual void OnMouseDown()
    // {
    //     CurrentPrefab = BuildSystem.CurrentPrefab;
    //     BuildSystem.InizializeGameObject(CurrentPrefab, gameObject.transform.position, IdFloor);
    //     Destroy(gameObject);
    // }
    
    
}
