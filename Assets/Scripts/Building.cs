using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Building : PlacebleObject
{
    [Header("Permission For Building")]
    [SerializeField] private bool _forward;
    [SerializeField] private bool _right;
    [SerializeField] private bool _back;
    [SerializeField] private bool _left;
    [SerializeField] private bool _up;
    public override void OnPointerClick(PointerEventData eventData)
    {
        BuildSystem.InitializePlacements(this);
        gameObject.GetComponent<Renderer>().material.color = Color.magenta;
    }

    public bool[] GetPermissionForBuilding()
    {
        // Debug.Log($"Forward {_forward} " +
        //           $"Right {_right } Back {_back} Left {_left} Up {_up}");
        return new[] {_forward, _right, _back, _left, _up};
    }
    
    
    
    
    
}
