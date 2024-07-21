using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;
using System.ComponentModel.Design;

namespace RPSOnline
{
    public class GameManager : NetworkBehaviour
    {
        static readonly List<Player> playerList = new List<Player>();

        public static event Action OnAllLockedIn;

        internal static void AddPlayer(Player player)
        {
            playerList.Add(player);
        }

        internal static void RemovePlayer(Player player)
        {
            playerList.Remove(player);
        }

        [ServerCallback]
        public static void PlayerLockedIn()
        {
            Debug.Log("A");
            if (IsAllLockedIn())
            {
                Debug.Log("B");
                OnAllLockedIn?.Invoke();
            }
        }

        [ServerCallback]
        internal static void SetPlayerNumbers()
        {
            byte playerNumber = 1;
            foreach (Player player in playerList)
                player.playerNumber = playerNumber++;
        }

        static bool IsAllLockedIn()
        {
            if (playerList.Count != 2) return false;

            foreach (Player current in playerList)
                if (!current.isLockedIn)
                    return false;
            return true;
        }
    }
}