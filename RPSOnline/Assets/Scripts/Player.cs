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

        [Header("Player UI")]
        [SerializeField] PlayerUI playerUI = null;

        [Header("SyncVars")]

        [SyncVar(hook = nameof(PlayerNumberChanged))]
        public byte playerNumber = 0;

        [SyncVar(hook = nameof(GuessChanged))]
        public RockPaperScissors guess;

        [SyncVar(hook = nameof(LockedInChanged))]
        public bool isLockedIn = false;

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

        #region Server
        /// <summary>
        /// This is invoked for NetworkBehaviour objects when they become active on the server.
        /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
        /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
        /// </summary>
        public override void OnStartServer()
        {
            base.OnStartServer();
            GameManager.AddPlayer(this);
        }

        /// <summary>
        /// Invoked on the server when the object is unspawned
        /// <para>Useful for saving object data in persistent storage</para>
        /// </summary>
        public override void OnStopServer()
        {
            base.OnStopServer();
            GameManager.RemovePlayer(this);
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
            OnPlayerNumberChanged = playerUI.OnPlayerNumberChanged;
            OnGuessChanged = playerUI.OnGuessChanged;
            OnLockedInChanged = playerUI.OnLockedChanged;

            // Invoke all event handlers with the initial data from spawn payload
            OnGuessChanged.Invoke(RockPaperScissors.None);
            OnPlayerNumberChanged.Invoke(playerNumber);
            OnLockedInChanged.Invoke(false);

            //the local UI is disabled, the locked in text does sync!
            if (!isLocalPlayer)
            {
                playerUI.OnNotLocalPlayer();
            }
            else
            {
                playerUI.OnLocalPlayer();
            }
        }

        /// <summary>
        /// Called when the local player object has been set up.
        /// <para>This happens after OnStartClient(), as it is triggered by an ownership message from the server. This is an appropriate place to activate components or functionality that should only be active for the local player, such as cameras and input.</para>
        /// </summary>
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
        }

        /// <summary>
        /// Called when the local player object is being stopped.
        /// <para>This happens before OnStopClient(), as it may be triggered by an ownership message from the server, or because the player object is being destroyed. This is an appropriate place to deactivate components or functionality that should only be active for the local player, such as cameras and input.</para>
        /// </summary>
        public override void OnStopLocalPlayer()
        {
            base.OnStopLocalPlayer();
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
        }

        [Command]
        public void LockedInButtonPressed()
        {
            isLockedIn = true;
            GameManager.PlayerLockedIn();
        }

        [Command]
        public void GuessRock()
        {
            guess = RockPaperScissors.Rock;
        }

        [Command]
        public void GuessPaper()
        {
            guess = RockPaperScissors.Paper;
        }

        [Command]
        public void GuessScissors()
        {
            guess = RockPaperScissors.Scissors;
        }
        #endregion
    }
}