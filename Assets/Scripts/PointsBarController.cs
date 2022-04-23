using System;
using UnityEngine;
using UnityEngine.UI;

public class PointsBarController : MonoBehaviour
{
    [SerializeField] private Text[] _textPoins;
    [SerializeField] private Text[] _currentTextPoins;
    
    [SerializeField] private string _template;

    [SerializeField] private Button _deleteCurrentButton;
    
    #region UnityMethods
    
    #endregion

    #region SetValues

    public void SetPoints(ref SchoolPoints schoolPoints)
    {
        if (schoolPoints.Points.Count > _textPoins.Length)
            throw new ArgumentException("Типов поинтов больше, чем текстов");
        
        foreach (var pair in schoolPoints.Points)
            _textPoins[(int) pair.Key].text = string.Format(_template, pair.Key, pair.Value);
    }
    
    public void SetCurrentPoints(ref SchoolPoints schoolPoints)
    {
        if (schoolPoints.Points.Count > _textPoins.Length)
            throw new ArgumentException("Типов поинтов больше, чем текущих текстов");
        
        foreach (var pair in schoolPoints.Points)
            _currentTextPoins[(int) pair.Key].text = string.Format(_template, pair.Key, pair.Value);
    }

    #endregion


    public void SetActiveAllCurrentElements(bool isActive)
    {
        _deleteCurrentButton.gameObject.SetActive(isActive);
        SetActiveCurrentTexts(isActive);
    }
    
    

    #region Texts

    public void SetActiveCurrentTexts(bool isActive)
    {
        foreach (var text in _currentTextPoins) text.gameObject.SetActive(isActive);
    }

    #endregion
}
