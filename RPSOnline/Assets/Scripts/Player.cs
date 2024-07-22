using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;
using System;

namespace RPSOnline
{
    public class Player : NetworkBehaviour
    {
        public event Action<RockPaperScissors> OnGuessChanged;
        public event Action<byte> OnPlayerNumberChanged;
        public event Action<bool> OnLockedInChanged;
        public event Action<byte> OnRoundWon;

        [Header("Player UI")]
        [SerializeField] PlayerUI playerUI = null;

        [Header("SyncVars")]

        [SyncVar(hook = nameof(PlayerNumberChanged))]
        public byte PlayerNumber = 0;

        [SyncVar(hook = nameof(GuessChanged))]
        public RockPaperScissors Guess;

        [SyncVar(hook = nameof(LockedInChanged))]
        public bool IsLockedIn = false;

        [SyncVar(hook = nameof(RoundWinsChanged))]
        private byte RoundWins = 0;

        #region Actions
        void PlayerNumberChanged(byte _, byte newPlayerNumber)
        {
            OnPlayerNumberChanged?.Invoke(newPlayerNumber);
        }

        void GuessChanged(RockPaperScissors _, RockPaperScissors newGuess)
        {
            OnGuessChanged?.Invoke(newGuess);
        }

        void LockedInChanged(bool _, bool newState)
        {
            OnLockedInChanged?.Invoke(newState);
        }

        void RoundWinsChanged(byte _, byte newWins)
        {
            OnRoundWon?.Invoke(newWins);
        }
        #endregion

        #region Server
        /// <summary>
        /// This is invoked for NetworkBehaviour objects when they become active on the server.
        /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
        /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
        /// </summary>
        public override void OnStartServer()
        {
            base.OnStartServer();
            GameManager.Instance.AddPlayer(this);
        }

        /// <summary>
        /// Invoked on the server when the object is unspawned
        /// <para>Useful for saving object data in persistent storage</para>
        /// </summary>
        public override void OnStopServer()
        {
            base.OnStopServer();
            GameManager.Instance.RemovePlayer(this);
        }

        #endregion

        #region Client
        /// <summary>
        /// Called on every NetworkBehaviour when it is activated on a client.
        /// <para>Objects on the host have this function called, as there is a local client on the host. The values of SyncVars on object are guaranteed to be initialized correctly with the latest state from the server when this function is called on the client.</para>
        /// </summary>
        public override void OnStartClient()
        {
            // wire up all events to handlers in PlayerUI
            OnPlayerNumberChanged += playerUI.OnPlayerNumberChanged;
            OnGuessChanged += playerUI.OnGuessChanged;
            OnRoundWon += playerUI.OnRoundWon;

            OnLockedInChanged += PlayerOnLockedInChanged;

            // Invoke all event handlers with the initial data from spawn payload
            OnGuessChanged.Invoke(RockPaperScissors.None);
            OnPlayerNumberChanged.Invoke(PlayerNumber);
            OnLockedInChanged.Invoke(false);
            OnRoundWon.Invoke(0);
            playerUI.isLocalPlayer = isLocalPlayer;

            playerUI.DisplayUI();
        }

        void PlayerOnLockedInChanged(bool newState)
        {
            GameManager.Instance.OnLockedInChanged(newState);
            playerUI.OnLockedChanged(newState);
        }

        /// <summary>
        /// This is invoked on clients when the server has caused this object to be destroyed.
        /// <para>This can be used as a hook to invoke effects or do client specific cleanup.</para>
        /// </summary>
        public override void OnStopClient()
        {
            OnPlayerNumberChanged = null;
            OnGuessChanged = null;
            OnLockedInChanged = null;
            OnRoundWon = null;
        }
        #endregion

        [ServerCallback]
        public void RoundWon()
        {
            Debug.Log($"Round Won for {gameObject.name}");
            RoundWins++;
        }

        [Command]
        public void LockedInButtonPressed()
        {
            IsLockedIn = true;
        }

        [Command]
        public void GuessRock()
        {
            Guess = RockPaperScissors.Rock;
        }

        [Command]
        public void GuessPaper()
        {
            Guess = RockPaperScissors.Paper;
        }

        [Command]
        public void GuessScissors()
        {
            Guess = RockPaperScissors.Scissors;
        }
    }
}