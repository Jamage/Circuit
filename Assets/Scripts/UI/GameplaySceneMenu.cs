using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameplaySceneMenu : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    [SerializeField] private Button levelSelectButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private GameBoard gameBoard;
    [SerializeField] private TextMeshProUGUI levelCompleteText;

    private void Awake()
    {
        if (menuButton == null)
            menuButton = GameObject.Find("Menu Button").GetComponent<Button>();
        if (levelSelectButton == null)
            levelSelectButton = GameObject.Find("Level Select Button").GetComponent<Button>();
        if (resetButton == null)
            levelSelectButton = GameObject.Find("Reset Button").GetComponent<Button>();
        if (mainMenuButton == null)
            levelSelectButton = GameObject.Find("Main Menu Button").GetComponent<Button>();
        if (gameBoard == null)
            gameBoard = GameObject.Find("Game Board").GetComponent<GameBoard>();
        if (levelCompleteText == null)
            levelCompleteText = GameObject.Find("Complete Text").GetComponent<TextMeshProUGUI>();

        menuButton.onClick.AddListener(() => MenuButton_OnClick());
        levelSelectButton.onClick.AddListener(() => LevelSelectButton_OnClick());
        mainMenuButton.onClick.AddListener(() => MainMenuButton_OnClick());
        resetButton.onClick.AddListener(() => ResetLevel());

        menuButton.gameObject.SetActive(true);
        levelCompleteText.gameObject.SetActive(false);
        ToggleMenuButtons(false);
    }

    public void LevelSelectButton_OnClick()
    {
        levelCompleteText.gameObject.SetActive(false);
        gameBoard.gameObject.SetActive(false);
        ToggleMenuButtons(false);
        LevelSelectManager.Instance.ShowAndSetBackButton(gameObject);
    }

    public void MenuButton_OnClick()
    {
        ToggleMenuButtons(levelSelectButton.gameObject.activeInHierarchy == false);
    }

    private void ToggleMenuButtons(bool setActive)
    {
        levelSelectButton.gameObject.SetActive(setActive);
        mainMenuButton.gameObject.SetActive(setActive);
        resetButton.gameObject.SetActive(setActive);
    }

    public void MainMenuButton_OnClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ResetLevel()
    {
        gameBoard.Reset();
        ToggleMenuButtons(false);
        levelCompleteText.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(true);
    }

    public void SetupForCompletedLevel()
    {
        levelCompleteText.gameObject.SetActive(true);
        ToggleMenuButtons(true);
        menuButton.gameObject.SetActive(false);
    }
}
