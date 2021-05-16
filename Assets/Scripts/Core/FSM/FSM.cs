using System;
using System.Collections.Generic;

namespace Guinea.Core
{
    public class FSM
    {
        private static readonly bool debug = false;
        #region StateMachine       
        protected State currentState;
        protected Dictionary<Type, List<Transition>> transitions = new Dictionary<Type, List<Transition>>();
        protected List<Transition> anyTransitions = new List<Transition>();
        protected List<Transition> currentTransitions;
        protected static List<Transition> EmptyTransitions = new List<Transition>();

        #endregion
        #region Transition
        public void AddTransition(State from,
        State to, Func<bool> predicate)
        {
            if (transitions.TryGetValue(from.GetType(), out var currents) == false)
            {
                currents = new List<Transition>();
                transitions[from.GetType()] = currents;
                currents.Add(new Transition(to, predicate));
            }
        }
        public void AddAnyTransition(State to, Func<bool> predicate)
        {
            anyTransitions.Add(new Transition(to, predicate));
        }
        private Transition GetTransition()
        {
            foreach (var transition in anyTransitions)
            {
                if (transition.Condition()) return transition;
            }

            foreach (var transition in currentTransitions)
            {
                if (transition.Condition()) return transition;
            }
            return null;
        }
        #endregion
        public void ChangeState(State state)
        {
            if (state == currentState) return;
            currentState?.Exit();
            if (debug) Commons.Log($"Change State from {currentState} to {state}");
            currentState = state;
            transitions.TryGetValue(currentState.GetType(), out currentTransitions);
            if (currentTransitions == null) currentTransitions = EmptyTransitions;
            currentState?.Enter();
        }
        public void Update()
        {
            ProcessTransition();
            currentState?.Update();
        }

        private void ProcessTransition()
        {
            var transition = GetTransition();
            if (transition != null) ChangeState(transition.To);
        }

        public void FixedUpdate() { currentState?.FixedUpdate(); }
    }
}