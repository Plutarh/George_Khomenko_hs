using UnityEngine;

public class BackToBase : State
{
    private BackToBaseScriptableState _backToBaseScriptableState;

    private Point _basePoint;
    private float _breakDistance = 1.2f;

    public BackToBase(ScriptableState state, Character character) : base(state, character)
    {
        _backToBaseScriptableState = (BackToBaseScriptableState)scriptableState;

        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();

        FindBasePoint();

        character.navMeshAgent.isStopped = false;
    }
    public override void Run()
    {
        if (end) return;

        if (GetDistanceToPoint() <= _breakDistance)
        {
            End();
            return;
        }

        MoveAgent();
    }

    void MoveAgent()
    {
        character.navMeshAgent.SetDestination(_basePoint.transform.position);
    }

    void FindBasePoint()
    {
        _basePoint = PointsHolder.BasePoint;
    }

    float GetDistanceToPoint()
    {
        return Vector3.Distance(character.transform.position, _basePoint.transform.position);
    }

    public override void End()
    {
        if (end) return;
        end = true;

        character.navMeshAgent.isStopped = true;

        character.ChangeState(_backToBaseScriptableState.nextState);
    }
}
