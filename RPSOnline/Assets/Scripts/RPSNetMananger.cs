using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPSOnline
{
    public class RPSNetMananger : NetworkManager
    {
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            Debug.Log("Player Added!");
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            Debug.Log("Player Left!");
        }
    }
}