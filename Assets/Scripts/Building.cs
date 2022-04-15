using UnityEngine;
using UnityEngine.EventSystems;

public class Building : PlacebleObject
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        BuildSystem.InitializePlacements(transform.position,  gameObject.GetComponent<Building>());
        gameObject.GetComponent<Renderer>().material.color = Color.magenta;
    }
}
