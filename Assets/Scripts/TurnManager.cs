using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static int turnCounter = 0;

    private void OnEnable()
    {
        ResetTurnCounter();
        Panel.OnPlacement += IncrementTurnCounter;
    }


    private void OnDisable()
    {
        Panel.OnPlacement -= IncrementTurnCounter;
        ResetTurnCounter();
    }

    private void IncrementTurnCounter(IBoardObject placedObject)
    {
        turnCounter++;
    }

    public static void ResetTurnCounter()
    {
        turnCounter = 0;
    }
}
