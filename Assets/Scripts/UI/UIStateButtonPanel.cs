using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateButtonPanel : MonoBehaviour
{
    [SerializeField] private List<ScriptableState> _statesToShow = new List<ScriptableState>();

    [SerializeField] private UIStateButton _btnPrefab;
    [SerializeField] private Transform _stateButtonParent;

    private List<UIStateButton> _stateButtons = new List<UIStateButton>();

    public void CreateStateButtons()
    {
        if (_stateButtons.Count > 0)
        {
            _stateButtons.ForEach(sb => Destroy(sb.gameObject));
            _stateButtons.Clear();
        }

        foreach (var state in _statesToShow)
        {
            var createdBtn = Instantiate(_btnPrefab, _stateButtonParent);

            createdBtn.SetState(state);
            _stateButtons.Add(createdBtn);
        }
    }
}
