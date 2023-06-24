namespace Utils
{
    using System;
    using System.Collections.Generic;

    class StateMachine<T>
    {
        private T stateActive;
        private Dictionary<T, Action> states;

        public StateMachine() { }

        public void Update()
        {
            if (!states.ContainsKey(stateActive)) return;

            states[stateActive]();
        }

        public void AddState(T state, Action action)
        {
            states.Add(state, action);
        }

        public void SetState(T state)
        {
            if (!states.ContainsKey(stateActive)) return;

            stateActive = state;
        }

        public bool IsActiveState(T state)
        {
            return stateActive.Equals(state);
        }
    }
}