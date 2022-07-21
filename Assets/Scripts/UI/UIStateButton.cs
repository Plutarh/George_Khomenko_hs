using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIStateButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _stateName;
    [SerializeField] private ScriptableState _state;

    public void SetState(ScriptableState newState)
    {
        _state = newState;
        _stateName.text = _state.stateName;
    }

    public void ExecuteState()
    {
        Character.Instance.ChangeState(_state);
    }
}
