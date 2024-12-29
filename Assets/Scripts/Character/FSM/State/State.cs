public abstract class State
{
    protected CharacterBase character;
    public State(CharacterBase character)
    {
        this.character = character;
    }
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }

}

