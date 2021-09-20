using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class LevelCompletionSaveData
{
    public string LevelName;
    public bool IsComplete;
    public int TurnsTaken;
    [NonSerialized] public UnityAction<int> OnComplete;

    public LevelCompletionSaveData(LevelData levelData)
    {
        LevelName = levelData.Name;
        IsComplete = false;
        TurnsTaken = 0;
    }

    public void SetComplete(int currentTurnCount)
    {
        IsComplete = true;
        if(TurnsTaken > currentTurnCount || TurnsTaken == 0)
            TurnsTaken = currentTurnCount;
        OnComplete?.Invoke(TurnsTaken);
    }

    internal void Update(LevelCompletionSaveData newSaveData)
    {
        IsComplete = newSaveData.IsComplete;
        if(TurnsTaken > newSaveData.TurnsTaken || TurnsTaken == 0)
            TurnsTaken = newSaveData.TurnsTaken;
    }
}
