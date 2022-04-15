using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StandartPlacement : Placement
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        BuildSystem.ClearCurrentBuilding();
        BuildSystem.InizialateGameObject(CurrentPrefab, gameObject.transform.position, IdFloor);
        
        Destroy(gameObject);
    }
}
