using UnityEngine;


public class Patroll : State
{
    private float _moveSpeed;
    private PatrollScriptableState _patrollScriptableState;
    private float _breakDistance = 1.5f;
    private Point _currentPoint;

    public Patroll(ScriptableState state, Character character) : base(state, character)
    {
        _patrollScriptableState = (PatrollScriptableState)scriptableState;

        // Тут можно сделать проверку, что если это игрок то он берет данные из конфига, для остальных из скриптейбла
        _moveSpeed = Config.Instance.characterMoveSpeed;
        //moveSpeed = _patrollScriptableState.moveSpeed;

        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();
        character.navMeshAgent.isStopped = false;
        FindRandomPoint();
    }

    public override void Run()
    {
        MoveAgent();
    }

    void MoveAgent()
    {
        if (GetDistanceToPoint() <= _breakDistance)
            FindRandomPoint();


        character.navMeshAgent.SetDestination(_currentPoint.transform.position);
    }

    void FindRandomPoint()
    {
        if (_currentPoint != null)
            _currentPoint.transform.localScale = Vector3.one * 0.2f;

        _currentPoint = PointsHolder.GetRandomPatrollPoint();

        _currentPoint.transform.localScale = Vector3.one * 0.4f;
    }

    float GetDistanceToPoint()
    {
        return Vector3.Distance(character.transform.position, _currentPoint.transform.position);
    }

    public override void End()
    {
        if (end) return;
        end = true;

        if (_currentPoint != null)
            _currentPoint.transform.localScale = Vector3.one * 0.2f;

        character.navMeshAgent.isStopped = true;
    }
}