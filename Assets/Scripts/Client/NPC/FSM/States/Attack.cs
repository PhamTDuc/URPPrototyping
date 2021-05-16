using UnityEngine;
using Guinea.Core;
namespace Guinea
{
    public class Attack : State
    {
        private static readonly bool debug = false;
        private ICarAI carAI;
        private IMemory memory;
        private IPlayable playable;
        public Attack(FSM fsm, ICarAI carAI, IMemory memory = null, IPlayable playable = null) : base(fsm)
        {
            this.carAI = carAI;
            this.memory = memory;
            this.playable = playable;
        }

        public override void Enter()
        {
            if (debug) Commons.Log($"[{playable.Obj.name}] entering Attack State");
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
            if (!carAI.HasTarget)
            {
                playable.Shoot(Random.value > 0.5f);
            }

            if (playable.GetCurrentAmmo() == 0)
            {
                playable.Reload();
            }

            if (playable.isTurnedOver())
            {
                playable.Reset();
            }
        }
    }
}