using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace RPSOnline
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI playerNameText;
        [SerializeField] TextMeshProUGUI guessText;

        public void OnPlayerNumberChanged(byte newPlayerNumber)
        {
            playerNameText.SetText(string.Format("Player {0:00}", newPlayerNumber));
        }

        public void OnGuessChanged(RockPaperScissors newGuess)
        {
            guessText.SetText(newGuess.ToString());
        }
    }
}