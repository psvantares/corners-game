using Fusion;
using UnityEngine;

namespace Game.Network
{
    public struct NetworkInputData : INetworkInput
    {
        public const byte MouseButton1 = 0x01;

        public byte Buttons;
        public Vector2Int Position;
    }
}