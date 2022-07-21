using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "State/Idle State")]
public class Idle : State
{
    [SerializeField] private float _scaleTime;
    [SerializeField] private float _scaleMultiplier;

    Tween _idleTween;
    Vector3 _startSize;

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
