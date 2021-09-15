using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectButton : MonoBehaviour
{
    public Toggle levelCompleteToggle;
    public Button levelSelectButton;
    public TextMeshProUGUI buttonText;

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
    }
}
