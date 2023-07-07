using Fusion;
using UnityEngine;

namespace Game.Gameplay
{
    public struct NetworkCheckerData : INetworkStruct
    {
        public int Index;
        public Vector2Int Position;
    }
}