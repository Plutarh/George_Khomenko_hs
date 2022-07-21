using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateButtonPanel : MonoBehaviour
{
    [SerializeField] private List<ScriptableState> _statesToShow = new List<ScriptableState>();

    [SerializeField] private UIStateButton _btnPrefab;
    [SerializeField] private Transform _stateButtonParent;

    private void Awake()
    {
        CreateStateButtons();
    }

    public void CreateStateButtons()
    {
        foreach (var state in _statesToShow)
        {
            var createdBtn = Instantiate(_btnPrefab, _stateButtonParent);

            createdBtn.SetState(state);
        }
    }
}
