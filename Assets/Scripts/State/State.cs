public abstract class State
{
    public string stateName;

    public ScriptableState scriptableState;

    public Character character;

    public State(Character target)
    {
        character = target;
    }

    public virtual void Initialize() { }

    public abstract void Run();
    public abstract void End();
}
