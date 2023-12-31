﻿using Fusion;
using Game.Data;

namespace Game.Gameplay
{
    public class NetworkPlayerData : NetworkBehaviour
    {
        private PlayerView playerView;

        [Networked(OnChanged = nameof(OnNickNameChanged))]
        public NetworkString<_16> NickName { get; private set; }

        public override void Spawned()
        {
            if (Object.HasStateAuthority)
            {
                NickName = FindObjectOfType<PlayerData>().GetNickName();
            }

            playerView = FindObjectOfType<PlayerView>();
            playerView.AddEntry(Object.InputAuthority, Object.HasStateAuthority, this);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            playerView.RemoveEntry(Object.InputAuthority);
        }

        // Events

        public static void OnNickNameChanged(Changed<NetworkPlayerData> playerInfo)
        {
            var inputAuthority = playerInfo.Behaviour.Object.InputAuthority;
            var nickName = playerInfo.Behaviour.NickName.ToString();

            playerInfo.Behaviour.playerView.UpdateNickName(inputAuthority, nickName);
        }
    }
}