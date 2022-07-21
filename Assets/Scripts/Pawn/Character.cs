using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : Pawn
{
    public NavMeshAgent navMeshAgent;

    [SerializeField] private ScriptableState _startState;
    [SerializeField] private ScriptableState _currentScriptableState;
    [SerializeField] private List<ScriptableState> _allStates = new List<ScriptableState>();

    private State _currentState;
    private float _moveSpeed;

    public enum EState
    {
        Idle,
        Patroll
    }

    public override void Awake()
    {
        _maxHealth = Config.Instance.characterMaxHealth;

        base.Awake();
        _moveSpeed = Config.Instance.characterMoveSpeed;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = _moveSpeed;
    }

    private void Start()
    {
        ChangeState(_startState);
    }

    private void Update()
    {
        RunCurrentState();
    }

    public void ChangeState(ScriptableState newState)
    {
        if (_currentScriptableState == newState) return;
        _currentScriptableState = newState;
        _currentState = _currentScriptableState.InitializeState(this);
    }

    void RunCurrentState()
    {
        if (_currentState == null) return;
        _currentState.Run();
    }
}
