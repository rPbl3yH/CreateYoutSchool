using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsBarController : MonoBehaviour
{
    [SerializeField] private Text _textSciencePoints;
    [SerializeField] private Text _textPhysicalPoints;
    
    [SerializeField] private Text _currentTextSciencePoints;
    [SerializeField] private Text _currentTextPhysicalPoints;

    [SerializeField] private string _template;
    
    #region UnityMethods
    

    #endregion

    public void SetPoints(byte sciencePoint, byte physicalPoint)
    {
        _textSciencePoints.text = string.Format(_template, "Science", sciencePoint);
        _textPhysicalPoints.text = string.Format(_template, "Physical", physicalPoint);
    }

    public void SetCurrentPoints(byte sciencePointValue, byte physicalPointValue)
    {
        _currentTextSciencePoints.text = string.Format(_template, "Science", sciencePointValue);
        _currentTextPhysicalPoints.text = string.Format(_template, "Physical", physicalPointValue);
    }

    public void SetActiveCurrentTexts(bool IsActive)
    {
        _currentTextSciencePoints.gameObject.SetActive(IsActive);
        _currentTextPhysicalPoints.gameObject.SetActive(IsActive);
    }
    
}
