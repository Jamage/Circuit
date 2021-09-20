using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectButton : MonoBehaviour
{
    public Toggle levelCompleteToggle;
    public Button levelSelectButton;
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI turnCountText;

    private void Awake()
    {
        if (levelCompleteToggle == null)
            levelCompleteToggle = GetComponentInChildren<Toggle>();
        if (levelSelectButton == null)
            levelSelectButton = GetComponentInChildren<Button>();
        if (buttonText == null)
            buttonText = levelSelectButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Setup(LevelCompletionSaveData levelSaveData)
    {
        buttonText.text = levelSaveData.LevelName;
        levelCompleteToggle.isOn = levelSaveData.IsComplete;
        levelSaveData.OnComplete = null;
        levelSaveData.OnComplete += OnLevelComplete;
        turnCountText.text = $"Turns: {levelSaveData.TurnsTaken}";
    }

    private void OnLevelComplete(int turnsTaken)
    {
        levelCompleteToggle.isOn = true;
        turnCountText.text = $"Turns: {turnsTaken}";
    }
}
