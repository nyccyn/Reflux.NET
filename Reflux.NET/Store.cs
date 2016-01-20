using System;

namespace RefluxNET
{
    public abstract class Store<T>
    {
        private event Action<T> StoreTriggered;

        protected void Trigger(T state)
        {
            if (StoreTriggered != null)
                StoreTriggered(state);
        }

        public void Listen(Action<T> callback)
        {
            StoreTriggered += callback;
        }

        public abstract T GetInitialState();

    }
}
