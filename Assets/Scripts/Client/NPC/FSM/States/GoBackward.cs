using Guinea.Core;
namespace Guinea
{
    public class GoBackward : State
    {
        private readonly bool debug = true;
        private ICarAI carAI;
        private IMemory memory;
        private IPlayable playable;
        public GoBackward(FSM fsm, ICarAI carAI, IMemory memory = null, IPlayable playable = null) : base(fsm)
        {
            this.carAI = carAI;
            this.memory = memory;
            this.playable = playable;
        }

        public override void Enter()
        {
            if (debug) Commons.Log($"[{playable.Obj.name}] entering GoBackward State");
            if (carAI.HasTarget) carAI.SetTargetByVec3(carAI.Target.transform.forward * 6.0f);
        }

        public override void Exit()
        {
            carAI.SetTarget(null);
        }

        public override void FixedUpdate()
        {
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