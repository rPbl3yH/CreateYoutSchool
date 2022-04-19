using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Target GameObject")]
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    
    [SerializeField] private float _sensitive = 3;
    [SerializeField] private float _limitY = 60;

    [SerializeField] private float _zoomMax;
    [SerializeField] private float _zoomMin;
    [SerializeField] private float _zoom;
    
    private float _x, _y;
    
    private Touch _touch;
    
    void Awake()
    {
        Initialize();
    }
    
    void Update()
    {
        if (Input.touchCount == 1)
        {
            _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Moved)
            {
                float deltaY = _touch.deltaPosition.y;
                if(deltaY > 0) _offset.z += _zoom;
                else if(deltaY < 0) _offset.z -= _zoom;
                
                _offset.z = Mathf.Clamp(_offset.z, -Mathf.Abs(_zoomMax), -Mathf.Abs(_zoomMin));
                
                _x = transform.localEulerAngles.y - _touch.deltaPosition.x / 100 * _sensitive;
                
                transform.localEulerAngles = new Vector3(_y, _x, 0);
                transform.position = transform.localRotation * _offset + _target.position;
            }
        }
        
    }
    
    

    private void Initialize()
    {
        _limitY = Math.Abs(_limitY);
        if (_limitY > 90) _limitY = 90;
        _y = transform.localEulerAngles.x;
        
        transform.position = _target.position + _offset;
    }
}
