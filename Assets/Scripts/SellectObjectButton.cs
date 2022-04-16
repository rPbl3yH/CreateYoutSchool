using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SellectObjectButton : MonoBehaviour
{
    [SerializeField] private UnityEvent<GameObject> _onClick;
    [SerializeField] private GameObject _object;

    private Button _button;


    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => _onClick?.Invoke(_object));
    }
}
