using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;
using System.ComponentModel.Design;
using TMPro;

namespace RPSOnline
{
    public class GameManager : NetworkBehaviour
    {
        public event Action OnAllLockedIn;
        public event Action OnEnoughPlayers;

        static readonly List<Player> playerList = new List<Player>();

        [SyncVar(hook = nameof(AllLockedIn))]
        public bool IsAllPlayersLockedIn = false;

        [SyncVar(hook = nameof(EnoughPlayersJoined))]
        public bool IsEnoughPlayers = false;

        public static GameManager Instance;
        private const int MAX_PLAYERS = 2;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                OnAllLockedIn += AllLockedInChanged;
            }
            else
                Destroy(this);
        }

        private void OnDisable()
        {
            OnAllLockedIn += null;
        }

        #region Player Setup
        [ServerCallback]
        internal void SetPlayerNumbers()
        {
            byte playerNumber = 1;
            foreach (Player player in playerList)
                player.PlayerNumber = playerNumber++;
        }

        internal void AddPlayer(Player player)
        {
            playerList.Add(player);
            IsEnoughPlayers = playerList.Count == MAX_PLAYERS;
        }

        internal void RemovePlayer(Player player)
        {
            playerList.Remove(player);
            IsEnoughPlayers = playerList.Count == MAX_PLAYERS;
        }
        #endregion

        void EnoughPlayersJoined(bool _, bool newState)
        {
            OnEnoughPlayers?.Invoke();
        }

        void AllLockedIn(bool _, bool newState)
        {
            OnAllLockedIn?.Invoke();
        }

        public void OnLockedInChanged(bool newState)
        {
            IsAllPlayersLockedIn = IsAllLockedIn();
        }

        void AllLockedInChanged()
        {
            Player player = GetWinner(playerList[0], playerList[1]);

            if (player != null)
            {
                player.RoundWon();
            }
        }

        static bool IsAllLockedIn()
        {
            if (playerList.Count != 2) return false;

            foreach (Player current in playerList)
                if (!current.IsLockedIn)
                    return false;
            return true;
        }

        Player GetWinner(Player a, Player b)
        {
            Debug.Log(a.Guess + " VS " + b.Guess);
            if (hasPlayerWon(a, b))
                return a;

            if (hasPlayerWon(b, a))
                return b;

            return null;
        }

        bool hasPlayerWon(Player a, Player b)
        {
            if (a.Guess == b.Guess) return false;
            if (a.Guess == RockPaperScissors.None || b.Guess == RockPaperScissors.None) return false;

            if (a.Guess == RockPaperScissors.Rock && b.Guess == RockPaperScissors.Scissors) return true;
            if (a.Guess == RockPaperScissors.Paper && b.Guess == RockPaperScissors.Rock) return true;
            if (a.Guess == RockPaperScissors.Scissors && b.Guess == RockPaperScissors.Paper) return true;

            return false;
        }
    }
}