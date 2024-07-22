using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPSOnline
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI waitingText;

        private void Awake()
        {
            GameManager.Instance.OnEnoughPlayers += Instance_OnEnoughPlayers;
        }

        private void Instance_OnEnoughPlayers()
        {
            waitingText.gameObject.SetActive(false);
        }
    }
}