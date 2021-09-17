using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject levelCompleteCanvas;
    public Button levelSelectButton;
    public Button mainMenuButton;
    public static UnityAction OnLevelComplete;

    private void OnEnable()
    {
        OnLevelComplete += RoundOver;
        levelSelectButton.onClick.AddListener(() => LevelSelectButton_OnClick());
        mainMenuButton.onClick.AddListener(() => MainMenuButton_OnClick());
    }

    private void OnDisable()
    {
        OnLevelComplete -= RoundOver;
    }

    public void RoundOver()
    {
        levelCompleteCanvas.SetActive(true);
    }

    public void LevelSelectButton_OnClick()
    {
        levelCompleteCanvas.SetActive(false);
        LevelSelectManager.Instance.ShowAndSetBackButton(levelCompleteCanvas);
    }

    public void MainMenuButton_OnClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
