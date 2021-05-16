using Guinea.Core;
namespace Guinea
{
    public class Patrol : State
    {
        private readonly bool debug = false;
        private ICarAI carAI;
        private IMemory memory;
        private IPlayable playable;
        public Patrol(FSM fsm, ICarAI carAI, IMemory memory = null, IPlayable playable = null) : base(fsm)
        {
            this.carAI = carAI;
            this.memory = memory;
            this.playable = playable;
        }

        public override void Enter()
        {
            carAI.SetTarget(null);
            if (debug) Commons.Log($"[{playable.Obj.name}] entering Patrol State");
        }

        public override void Exit()
        {
        }

        public override void FixedUpdate()
        {
            if (!carAI.HasTarget) carAI.SetRandomTarget();
            carAI.MoveAI();

        }

        public override void Update()
        {
            if (playable.isTurnedOver())
            {
                playable.Reset();
            }
        }
    }
}