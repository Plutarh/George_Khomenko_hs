using UnityEngine;

[CreateAssetMenu(menuName = "States/Idle")]
public class IdleScriptableState : ScriptableState
{
    public float scaleTime;
    public float scaleMultiplier;
    public override State InitializeState(Character target)
    {
        return new Idle(this, target);
    }
}
