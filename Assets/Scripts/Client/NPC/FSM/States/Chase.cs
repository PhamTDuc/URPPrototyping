using UnityEngine;
using Guinea.Core;
namespace Guinea
{
    public class Chase : State
    {
        private static readonly bool debug = false;
        private ICarAI carAI;
        private IMemory memory;
        private IPlayable playable;
        public Chase(FSM fsm, ICarAI carAI, IMemory memory = null, IPlayable playable = null) : base(fsm)
        {
            this.carAI = carAI;
            this.memory = memory;
            this.playable = playable;
        }

        public override void Enter()
        {
            if (debug) Commons.Log($"[{playable.Obj.name}] entering Chase State");
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