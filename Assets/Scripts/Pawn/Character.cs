using System.Collections.Generic;
using System.Linq;
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

    // Буду использвать синглтон, если будет больше юнитов, то уже взаимодействие будет через CharacterPicker
    public static Character Instance;

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
        Instance = this;
    }

    private void Start()
    {
        ChangeState(_startState);
    }

    private void Update()
    {
        if (IsDead) return;

        RunCurrentState();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeState(_allStates.FirstOrDefault(st => st is IdleScriptableState));
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeState(_allStates.FirstOrDefault(st => st is PatrollScriptableState));
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeState(_allStates.FirstOrDefault(st => st is BackToBaseScriptableState));
        }
    }



    public void ChangeState(ScriptableState newState)
    {
        if (IsDead) return;
        if (_currentScriptableState == newState) return;
        if (_currentState != null) _currentState.End();

        _currentScriptableState = newState;
        _currentState = _currentScriptableState.InitializeState(this);
    }

    void RunCurrentState()
    {
        if (_currentState == null) return;
        _currentState.Run();
    }

    public override void Death()
    {
        base.Death();
        if (_currentState != null) _currentState.End();
    }
}
