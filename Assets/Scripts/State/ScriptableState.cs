using UnityEngine;

public abstract class ScriptableState : ScriptableObject
{
    public string stateName;
    public abstract State InitializeState(Character target);

    private void OnValidate()
    {
        if (stateName == string.Empty)
            stateName = name;
    }
}
