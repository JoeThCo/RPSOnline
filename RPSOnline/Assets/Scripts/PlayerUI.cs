using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace RPSOnline
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI playerNameText;

        public void OnPlayerNumberChanged(byte newPlayerNumber)
        {
            playerNameText.text = string.Format("Player {0:00}", newPlayerNumber);
        }
    }
}