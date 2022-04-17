using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsBarController : MonoBehaviour
{
    [SerializeField] private Text _textSciencePoints;
    [SerializeField] private Text _textPhysicalPoints;

    [SerializeField] private string _template;
    
    #region UnityMethods
    

    #endregion

    public void SetPoint(byte sciencePoint, byte physicalPoint)
    {
        _textSciencePoints.text = string.Format(_template, "Science", sciencePoint);
        _textPhysicalPoints.text = string.Format(_template, "Physical", physicalPoint);
    }
    
}
