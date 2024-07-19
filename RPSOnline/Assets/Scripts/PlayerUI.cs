using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace RPSOnline
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI guessText;
        [Space(10)]
        [SerializeField] private RectTransform localUI;
        [SerializeField] private RectTransform forGuessing;
        [SerializeField] private RectTransform postGuess;
        [SerializeField] private RectTransform displayAllTheTime;

        public void OnNotLocalPlayer()
        {
            localUI.gameObject.SetActive(false);
            displayAllTheTime.gameObject.SetActive(false);
        }

        public void ResetUI() 
        {
            forGuessing.gameObject.SetActive(true);
            postGuess.gameObject.SetActive(false);
        }

        public void OnLockedIn()
        {
            forGuessing.gameObject.SetActive(false);
            postGuess.gameObject.SetActive(true);
        }

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