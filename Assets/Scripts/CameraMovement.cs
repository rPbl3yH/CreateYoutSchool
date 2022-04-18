using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera _cameraMain;

    [Header("Target GameObject")]
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    
    [SerializeField] private float _sensitive = 3;
    [SerializeField] private float _limitY = 60;
    
    [Header("Zoom setting")]
    [SerializeField] private float _zoom = 0.25f;
    [SerializeField] private float _zoomMax = 10;
    [SerializeField] private float _zoomMin = 3;
    
    private float _x, _y;
    
    // Start is called before the first frame update
    void Awake()
    {
        _cameraMain = GetComponent<Camera>();
        
        Initialize();
    }
    
    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetAxis("Mouse ScrollWheel") > 0) _offset.z += _zoom;
        else if(Input.GetAxis("Mouse ScrollWheel") < 0) _offset.z -= _zoom;
        _offset.z = Mathf.Clamp(_offset.z, -Mathf.Abs(_zoomMax), -Mathf.Abs(_zoomMin));

        _x = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * _sensitive;
        _y += Input.GetAxis("Mouse Y") * _sensitive;
        _y = Mathf.Clamp (_y, -_limitY, _limitY);
        transform.localEulerAngles = new Vector3(-_y, _x, 0);
        transform.position = transform.localRotation * _offset + _target.position;
    }

    private void Initialize()
    {
        _limitY = Math.Abs(_limitY);
        if (_limitY > 90) _limitY = 90;

        _offset = new Vector3(_offset.x, _offset.y, -Mathf.Abs(_zoomMax)/ 2);
        transform.position = _target.position + _offset;
    }
}
