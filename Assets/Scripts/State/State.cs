public abstract class State
{
    public string stateName;

    public ScriptableState scriptableState;

    public Character character;
    public bool end;

    public State(ScriptableState scriptable, Character target)
    {
        character = target;
        scriptableState = scriptable;
    }

    public virtual void Initialize() { }

    public abstract void Run();
    public abstract void End();
}
