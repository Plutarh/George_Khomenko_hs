using UnityEngine;

[CreateAssetMenu(menuName = "States/Patroll")]
public class PatrollScriptableState : ScriptableState
{
    public float moveSpeed;

    public override State InitializeState(Character target)
    {
        return new Patroll(this, target);
    }
}
