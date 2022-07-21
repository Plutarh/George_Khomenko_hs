using DG.Tweening;
using UnityEngine;


public class Idle : State
{
    private float _scaleTime;
    private float _scaleMultiplier;

    private IdleScriptableState _idleScriptableState;

    private Tween _idleTween;
    private Vector3 _startSize;

    public Idle(ScriptableState state, Character character) : base(state, character)
    {
        _idleScriptableState = (IdleScriptableState)scriptableState;

        _scaleTime = _idleScriptableState.scaleTime;
        _scaleMultiplier = _idleScriptableState.scaleMultiplier;

        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();

        _startSize = character.transform.localScale;
        _idleTween = character.transform.DOScale(_startSize * _scaleMultiplier, _scaleTime).SetLoops(-1, LoopType.Yoyo);
    }
    public override void Run()
    {

    }

    public override void End()
    {

        _idleTween.Kill();

        character.transform.DOScale(_startSize, _scaleTime);
    }
}
