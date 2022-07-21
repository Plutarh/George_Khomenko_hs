using UnityEngine;

public abstract class State : ScriptableObject
{
    public Character character;

    public virtual void Initialize() { }

    public abstract void Run();
    public abstract void End();

}
