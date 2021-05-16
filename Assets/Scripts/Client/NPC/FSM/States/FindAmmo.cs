using UnityEngine;
using Guinea.Core;
namespace Guinea
{
    public class FindAmmo : State
    {
        private ICarAI carAI;
        private IMemory memory;
        private IPlayable playable;
        public FindAmmo(FSM fsm, ICarAI carAI, IMemory memory = null, IPlayable playable = null) : base(fsm)
        {
            this.carAI = carAI;
            this.memory = memory;
            this.playable = playable;
        }

        public override void Enter()
        {
            Commons.Log($"[{playable.Obj.name}] entering FindAmmo State");
            carAI.SetTarget(null);
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