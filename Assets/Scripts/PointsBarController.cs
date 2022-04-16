using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsBarController : MonoBehaviour
{
    public static PointsBarController Current;
    
    [SerializeField] private Text _textSciencePoints;
    [SerializeField] private Text _textPhysicalPoints;

    public event Action<byte, byte> UpdatePointsBar;

    public byte SciencePoints;
    public byte PhysicalPoints;

    #region UnityMethods

    private void Awake()
    {
        Current = GetComponent<PointsBarController>();
    }

    private void Update()
    {
        _textPhysicalPoints.text = "Physical - " + PhysicalPoints;
        _textSciencePoints.text = "Science - " + SciencePoints;
    }

    #endregion
    
}
