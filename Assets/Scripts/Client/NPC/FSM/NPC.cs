using System;
using UnityEngine;
using Guinea.Core;

namespace Guinea
{
    public class NPC : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField]
        private bool debug;
        #region  FSM and States
        private FSM fsm;
        private Patrol patrol;
        private Chase chase;
        private Attack attack;
        private GoBackward goBackward;
        private FindAmmo findAmmo;
        #endregion
        private ICarAI carAI;
        private IPlayable playable;
        private IMemory memory;
        private MemoryInfo mem;
        void Start()
        {
            carAI = GetComponent<ICarAI>();
            playable = GetComponent<IPlayable>();
            memory = GetComponent<IMemory>();
            #region FSM and States
            fsm = new FSM();
            patrol = new Patrol(fsm, carAI, memory, playable);
            chase = new Chase(fsm, carAI, memory, playable);
            attack = new Attack(fsm, carAI, memory, playable);
            goBackward = new GoBackward(fsm, carAI, memory, playable);
            findAmmo = new FindAmmo(fsm, carAI, memory, playable);
            #endregion

            #region Transitions
            fsm.AddTransition(patrol, chase, patrolToChase);
            fsm.AddTransition(chase, attack, chaseToAttack);
            // fsm.AddTransition(chase, patrol, chaseToPatrol);
            fsm.AddTransition(attack, chase, attackToChase);
            fsm.AddTransition(goBackward, patrol, GobackWardToPatrol);
            // fsm.AddTransition(attack, patrol, attackToPatrol);
            fsm.AddAnyTransition(goBackward, chaseToGoBackward);
            #endregion

            fsm.ChangeState(patrol);

            bool patrolToChase()
            {
                if (memory.InMemory(playable.OpponentLayerName, null, out mem))
                {
                    carAI.SetTarget(mem.obj.transform);
                    if (debug) Commons.Log("Change State transition patrolToChase. Chasing: " + mem.obj);
                    return true;
                }
                return false;
            }
            // bool chaseToPatrol() => !memory.InMemory(playable.OpponentLayerName);

            bool chaseToAttack()
            {
                if (memory.InMemory(playable.OpponentLayerName, null, out mem))
                {
                    carAI.SetTarget(mem.obj.transform);
                    Vector3 direction = mem.obj.transform.position - transform.position;
                    if (direction.magnitude < playable.AttackRange &&
                    (Vector3.Angle(transform.forward, direction.normalized) < playable.AttackAngle / 2.0f))
                    {
                        if (debug) Commons.Log("Change State transition chaseToAttack. Attack: " + mem.obj);
                        return true;
                    }
                }
                else
                {
                    if (debug) Commons.Log("Change State transition chaseToPatrol");
                    fsm.ChangeState(patrol); // Go to Patrol State
                }
                return false;
            }
            bool attackToChase()
            {
                if (memory.InMemory(playable.OpponentLayerName, null, out mem))
                {
                    Vector3 direction = mem.obj.transform.position - transform.position;
                    if (direction.magnitude > playable.AttackRange * 0.8)
                    {
                        if (debug) Commons.Log("Change State transition attackToChase. Chase: " + mem.obj);
                        return true;
                    }
                }
                else
                {
                    if (debug) Commons.Log("Change State transition attackToPatrol");
                    fsm.ChangeState(patrol);
                }
                return false;
            }
            // bool attackToPatrol() => !memory.InMemory(playable.OpponentLayerName, null);
            bool chaseToGoBackward()
            {
                if (mem != null && mem.obj != null)
                {
                    Vector3 direction = mem.obj.transform.position - transform.position;
                    if (direction.magnitude < playable.AttackRange * 0.4f)
                    {
                        if (debug) Commons.Log("Change State transition anyToGoBackward!!");
                        return true;
                    }
                }
                return false;
            }

            bool GobackWardToPatrol()
            {
                if (mem != null && mem.obj != null)
                {
                    Vector3 direction = mem.obj.transform.position - transform.position;
                    if (direction.magnitude > playable.AttackRange * 0.6f)
                    {
                        if (debug) Commons.Log("Change State transition BackwardToPatrol!!");
                        return true;
                    }
                }
                return false;
            }
        }

        void Update() => fsm.Update();
        void FixedUpdate() => fsm.FixedUpdate();
    }
}
