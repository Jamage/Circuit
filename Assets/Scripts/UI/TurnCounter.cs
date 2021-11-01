using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnCounter : MonoBehaviour
{
    TextMeshProUGUI turnCounterText;

    private void OnEnable()
    {
        if (turnCounterText == null)
            turnCounterText = GetComponent<TextMeshProUGUI>();

        TurnManager.OnTurnChange += TurnChange;
    }

    private void OnDisable()
    {
        TurnManager.OnTurnChange -= TurnChange;
    }

    private void TurnChange()
    {
        turnCounterText.text = TurnManager.turnCounter.ToString();
    }
}
