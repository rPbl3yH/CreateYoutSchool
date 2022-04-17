using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class SellectObjectToggle : MonoBehaviour
{
    [SerializeField] private UnityEvent _onSelect;
    [SerializeField] private UnityEvent _onDeselect;

    private Toggle _toggle;


    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener((bool value) =>
        {
            if (value) _onSelect?.Invoke();
            else _onDeselect?.Invoke();
        });
    }
}
