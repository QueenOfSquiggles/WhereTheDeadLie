namespace state_machine;

public abstract partial class State : Node
{

    public struct StateData
    {
        public CharacterBody3D char_body;
        public NavigationAgent3D nav_agent;
        public AnimationTree anim_tree;
        public StateMachine machine;
        public float move_speed;

    }

    public const int RESULT_KEEP = -1;

    public abstract int StateMachineTick(StateData data, float delta);
    
    public virtual void OnExitState(StateData data) {}
    public virtual void OnEnterState(StateData data) {}

    public abstract string GetStateAnimation();

}