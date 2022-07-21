using UnityEngine;

[CreateAssetMenu(menuName = "States/Back To Base")]
public class BackToBaseScriptableState : ScriptableState
{
    public ScriptableState nextState;

    public override State InitializeState(Character target)
    {
        return new BackToBase(this, target);
    }
}