using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZooming : MonoBehaviour
{

    private Touch _startTouch;
    private Touch _endTouch;

    [SerializeField] private float _sensitive;
    [SerializeField] private Transform _target;
    [SerializeField] private float ZoomMax = 10f;

    void Update()
    {
        if (Input.touchCount == 2)
        {
            _startTouch = Input.GetTouch(0);
            _endTouch = Input.GetTouch(1);

            Vector2 deltaStartPos = _startTouch.position - _startTouch.deltaPosition;
            Vector2 deltaEndPos = _endTouch.position - _endTouch.position;

            float distBetweenDeltaPos = (deltaStartPos - deltaEndPos).magnitude;
            float currentDistBetweenDeltaPos = (_startTouch.position - _endTouch.position).magnitude;

            float distance = currentDistBetweenDeltaPos - distBetweenDeltaPos;
            Zooming(distance);
        }
    }

    void Zooming(float value)
    {
        float height = transform.position.y + (value * _sensitive * Time.deltaTime);
        float delta = Mathf.Abs(height - _target.position.y);

        if (delta <= ZoomMax)
            transform.position = new Vector3(transform.position.x, height, transform.position.z);

    }
}
