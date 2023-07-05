using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class ServiceLocator
    {
        private readonly Dictionary<string, IGameService> services = new();

        public static ServiceLocator Instance { get; private set; }

        private ServiceLocator()
        {
        }

        public static void Initialize()
        {
            Instance = new ServiceLocator();
        }

        public T GetService<T>() where T : IGameService
        {
            var key = typeof(T).Name;

            if (services.TryGetValue(key, out var service))
            {
                return (T)service;
            }

            Debug.LogError($"{key} not registered with {GetType().Name}");
            throw new InvalidOperationException();
        }

        public void Register<T>(T service) where T : IGameService
        {
            var key = typeof(T).Name;

            if (services.ContainsKey(key))
            {
                Debug.LogError($"Attempted to register service of type {key} which is already registered with the {GetType().Name}.");
                return;
            }

            services.Add(key, service);
        }

        public void Unregister<T>() where T : IGameService
        {
            var key = typeof(T).Name;

            if (!services.ContainsKey(key))
            {
                Debug.LogError($"Attempted to unregister service of type {key} which is not registered with the {GetType().Name}.");
                return;
            }

            services.Remove(key);
        }
    }
}