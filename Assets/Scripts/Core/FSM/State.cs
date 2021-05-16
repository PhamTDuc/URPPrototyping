namespace Guinea.Core
{
    public abstract class State
    {
        protected FSM fsm;
        public State(FSM fsm)
        {
            this.fsm = fsm;
        }
        public abstract void Enter();
        public abstract void Update();
        public abstract void FixedUpdate();
        public abstract void Exit();
    }
}