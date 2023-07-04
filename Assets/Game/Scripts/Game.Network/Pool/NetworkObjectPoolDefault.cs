using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Game.Network
{
    public class NetworkObjectPoolDefault : MonoBehaviour, INetworkObjectPool
    {
        [Tooltip("The objects to be pooled, leave it empty to pool every Network Object spawned")]
        [SerializeField]
        private List<NetworkPrefabRef> poolableObjects;

        private readonly Dictionary<NetworkPrefabId, Stack<NetworkObject>> free = new();

        public NetworkObject AcquireInstance(NetworkRunner runner, NetworkPrefabInfo info)
        {
            if (ShouldPool(runner, info))
            {
                var instance = GetObjectFromPool(runner, info);

                instance.transform.position = Vector3.zero;

                return instance;
            }

            return InstantiateObject(runner, info);
        }

        public void ReleaseInstance(NetworkRunner runner, NetworkObject instance, bool isSceneObject)
        {
            if (isSceneObject)
            {
                Destroy(instance.gameObject);
                return;
            }

            if (runner.Config.PrefabTable.TryGetId(instance.NetworkGuid, out var prefabId))
            {
                if (free.TryGetValue(prefabId, out var stack))
                {
                    instance.gameObject.SetActive(false);
                    stack.Push(instance);
                }
                else
                {
                    Destroy(instance.gameObject);
                }

                return;
            }

            Destroy(instance.gameObject);
        }

        private NetworkObject GetObjectFromPool(NetworkRunner runner, NetworkPrefabInfo info)
        {
            NetworkObject instance = null;

            if (free.TryGetValue(info.Prefab, out var stack))
            {
                while (stack.Count > 0 && instance == null)
                {
                    instance = stack.Pop();
                }
            }

            if (instance == null)
            {
                instance = GetNewInstance(runner, info);
            }

            instance.gameObject.SetActive(true);
            return instance;
        }

        private NetworkObject GetNewInstance(NetworkRunner runner, NetworkPrefabInfo info)
        {
            var instance = InstantiateObject(runner, info);

            if (free.TryGetValue(info.Prefab, out var stack) != false)
            {
                return instance;
            }

            stack = new Stack<NetworkObject>();
            free.Add(info.Prefab, stack);

            return instance;
        }

        private NetworkObject InstantiateObject(NetworkRunner runner, NetworkPrefabInfo info)
        {
            if (runner.Config.PrefabTable.TryGetPrefab(info.Prefab, out var prefab))
            {
                return Instantiate(prefab);
            }

            Debug.LogError("No prefab for " + info.Prefab);
            return null;
        }

        private bool ShouldPool(NetworkRunner runner, NetworkPrefabInfo info)
        {
            if (runner.Config.PrefabTable.TryGetPrefab(info.Prefab, out var networkObject))
            {
                if (poolableObjects.Count == 0)
                {
                    return true;
                }

                if (IsPoolableObject(networkObject))
                {
                    return true;
                }
            }
            else
            {
                Debug.LogError("No prefab found.");
            }

            return false;
        }

        private bool IsPoolableObject(NetworkObject networkObject)
        {
            foreach (var poolableObject in poolableObjects)
            {
                if ((Guid)poolableObject == (Guid)networkObject.NetworkGuid)
                {
                    return true;
                }
            }

            return false;
        }
    }
}