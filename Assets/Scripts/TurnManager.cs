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
        LinePanel.OnPlacement += IncrementTurnCounter;
        LinePanel.OnSwap += DecrementTurnCounter;
    }


    private void OnDisable()
    {
        LinePanel.OnPlacement -= IncrementTurnCounter;
        LinePanel.OnSwap -= DecrementTurnCounter;
        ResetTurnCounter();
    }

    //OnPlacement
    private void IncrementTurnCounter(IBoardObject placedObject)
    {
        turnCounter++;
    }

    //OnSwap
    private void DecrementTurnCounter(IBoardObject placedObject, IBoardObject occupyingObject)
    {
        turnCounter--;
    }

    public static void ResetTurnCounter()
    {
        turnCounter = 0;
    }
}
