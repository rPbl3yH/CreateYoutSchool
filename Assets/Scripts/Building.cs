using UnityEngine;
using UnityEngine.EventSystems;

public class Building : PlacebleObject
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        gameObject.GetComponent<Renderer>().material.color = Color.magenta;
        BuildSystem.InitializePlacements(transform.position);
    }
}
