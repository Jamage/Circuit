using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    public static int turnCounter = 0;
    public static UnityAction OnTurnChange;

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
        OnTurnChange?.Invoke();
    }

    //OnSwap
    private void DecrementTurnCounter(IBoardObject placedObject, IBoardObject occupyingObject)
    {
        turnCounter--;
        OnTurnChange?.Invoke();
    }

    public static void ResetTurnCounter()
    {
        turnCounter = 0;
        OnTurnChange?.Invoke();
    }
}
