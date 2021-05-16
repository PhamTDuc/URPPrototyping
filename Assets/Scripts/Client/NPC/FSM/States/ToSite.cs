using UnityEngine;
using Guinea.Core;
namespace Guinea
{
    public class ToSite : State
    {
        private readonly bool debug = true;
        private ICarAI carAI;
        private IMemory memory;
        private IPlayable playable;
        public ToSite(FSM fsm, ICarAI carAI, IMemory memory = null, IPlayable playable = null) : base(fsm)
        {
            this.carAI = carAI;
            this.memory = memory;
            this.playable = playable;
        }

        public override void Enter()
        {
            if (debug) Commons.Log($"[{playable.Obj.name}] entering ToSite State");
            carAI.SetTargetByVec3(new Vector3(145.0f, 5.0f, 24.0f));
        }

        public override void Exit()
        {
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