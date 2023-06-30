using System;
using System.Collections.Generic;

namespace Game.Utilities
{
    public abstract class ObjectPool<T> : IDisposable where T : UnityEngine.Component
    {
        private bool isDisposed;
        private Queue<T> q;

        protected int MaxPoolCount => int.MaxValue;

        protected abstract T CreateInstance();

        protected virtual void OnBeforeRent(T instance)
        {
            instance.gameObject.SetActive(true);
        }

        protected virtual void OnBeforeReturn(T instance)
        {
            instance.gameObject.SetActive(false);
        }

        protected virtual void OnClear(T instance)
        {
            if (instance == null)
            {
                return;
            }

            var go = instance.gameObject;

            if (go == null)
            {
                return;
            }

            UnityEngine.Object.Destroy(go);
        }

        public int Count
        {
            get
            {
                if (q == null)
                {
                    return 0;
                }

                return q.Count;
            }
        }

        public T Rent()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException("ObjectPool was already disposed.");
            }

            if (q == null)
            {
                q = new Queue<T>();
            }

            var instance = q.Count > 0
                ? q.Dequeue()
                : CreateInstance();

            OnBeforeRent(instance);

            return instance;
        }

        public void Return(T instance)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException("ObjectPool was already disposed.");
            }

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            q ??= new Queue<T>();

            if (q.Count + 1 == MaxPoolCount)
            {
                throw new InvalidOperationException("Reached Max PoolSize");
            }

            OnBeforeReturn(instance);
            q.Enqueue(instance);
        }

        public void Clear(bool callOnBeforeRent = false)
        {
            if (q == null)
            {
                return;
            }

            while (q.Count != 0)
            {
                var instance = q.Dequeue();

                if (callOnBeforeRent)
                {
                    OnBeforeRent(instance);
                }

                OnClear(instance);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    Clear(false);
                }

                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}