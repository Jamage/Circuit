using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static UnityAction OnLevelComplete;
    public GameplaySceneMenu gameplaySceneMenu;

    private void OnEnable()
    {
        OnLevelComplete += RoundOver;
    }

    private void OnDisable()
    {
        OnLevelComplete -= RoundOver;
    }

    public void RoundOver()
    {
        gameplaySceneMenu.SetupForCompletedLevel();
    }
}
