using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace RPSOnline
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI roundWinsText;
        [Space(10)]
        [SerializeField] private TextMeshProUGUI guessText;
        [SerializeField] private RectTransform guessButtons;
        [Space(10)]
        [SerializeField] private RectTransform lockedInText;
        [SerializeField] private RectTransform lockedInButton;

        public bool isLocalPlayer = false;

        public void DisplayUI()
        {
            if (isLocalPlayer)
            {
                OnLocalPlayer();
            }
            else
            {
                OnNotLocalPlayer();
            }
        }

        void OnNotLocalPlayer()
        {
            guessText.gameObject.SetActive(false);
            guessButtons.gameObject.SetActive(false);
            lockedInButton.gameObject.SetActive(false);

            lockedInText.gameObject.SetActive(false);
        }

        void OnLocalPlayer()
        {
            guessText.gameObject.SetActive(true);
            guessButtons.gameObject.SetActive(true);
            lockedInButton.gameObject.SetActive(true);

            lockedInText.gameObject.SetActive(false);
        }

        public void OnLockedChanged(bool newState)
        {
            guessButtons.gameObject.SetActive(false);
            lockedInButton.gameObject.SetActive(false);

            lockedInText.gameObject.SetActive(newState);
        }

        public void OnPlayerNumberChanged(byte newPlayerNumber)
        {
            playerNameText.SetText(string.Format("Player {0:00}", newPlayerNumber));
        }

        public void OnGuessChanged(RockPaperScissors newGuess)
        {
            guessText.SetText(newGuess.ToString());
        }

        public void OnRoundWon(byte roundWins)
        {
            roundWinsText.SetText($"W: {roundWins}");
        }
    }
}