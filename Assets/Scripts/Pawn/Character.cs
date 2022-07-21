using System.Collections.Generic;
using UnityEngine;

public class Character : Pawn
{
    [SerializeField] private State _startState;
    [SerializeField] private List<State> _allStates = new List<State>();
    [SerializeField] private State _currentState;
    private float _moveSpeed;



    public override void Awake()
    {
        base.Awake();
        _moveSpeed = Config.Instance.characterMoveSpeed;
    }

    private void Update()
    {
        RunCurrentState();
    }

    public void ChangeState(State newState)
    {

    }

    void RunCurrentState()
    {
        if (_currentState == null)
        {
            Debug.LogError("Cannot execute NULL state");
            return;
        }

        _currentState.Run();
    }
}
