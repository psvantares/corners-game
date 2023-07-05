using System.Collections.Generic;
using Fusion;
using Game.Core;
using TMPro;
using UnityEngine;

namespace Game.Gameplay
{
    public class PlayerView : ViewBase
    {
        [SerializeField]
        private Transform[] nodes;

        [SerializeField]
        private TMP_Text playerEntryPrefab;

        private readonly Dictionary<PlayerRef, TMP_Text> playerEntries = new();
        private readonly Dictionary<PlayerRef, string> playerNickNames = new();

        public void AddEntry(PlayerRef playerRef, bool hasStateAuthority, NetworkPlayerData networkPlayerData)
        {
            if (playerEntries.ContainsKey(playerRef))
            {
                return;
            }

            if (networkPlayerData == null)
            {
                return;
            }

            var parent = hasStateAuthority ? nodes[0] : nodes[1];
            var entry = Instantiate(playerEntryPrefab, parent);

            entry.transform.localScale = Vector3.one;
            entry.alignment = hasStateAuthority ? TextAlignmentOptions.Left : TextAlignmentOptions.Right;

            playerNickNames.Add(playerRef, string.Empty);
            playerEntries.Add(playerRef, entry);

            UpdateEntry(playerRef, entry);
        }

        public void RemoveEntry(PlayerRef playerRef)
        {
            if (playerEntries.TryGetValue(playerRef, out var entry) == false)
            {
                return;
            }

            if (entry != null)
            {
                Destroy(entry.gameObject);
            }

            playerNickNames.Remove(playerRef);
            playerEntries.Remove(playerRef);
        }

        public void UpdateNickName(PlayerRef player, string nickName)
        {
            if (playerEntries.TryGetValue(player, out var entry) == false)
            {
                return;
            }

            playerNickNames[player] = nickName;
            UpdateEntry(player, entry);
        }

        private void UpdateEntry(PlayerRef player, TMP_Text entry)
        {
            entry.text = playerNickNames[player].ToUpper();
        }
    }
}