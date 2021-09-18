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
    [NonSerialized] public UnityAction OnComplete;

    public LevelCompletionSaveData(LevelData levelData)
    {
        LevelName = levelData.Name;
        IsComplete = false;
    }

    public void SetComplete()
    {
        IsComplete = true;
        OnComplete?.Invoke();
    }

    internal void Update(LevelCompletionSaveData saveData)
    {
        IsComplete = saveData.IsComplete;
    }
}
