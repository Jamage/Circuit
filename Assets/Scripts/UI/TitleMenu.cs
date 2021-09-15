using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TitleMenu : MonoBehaviour
{
    public GameObject titleCanvas;
    public GameObject levelSelectCanvas;

    public void GoToTitle()
    {
        levelSelectCanvas.SetActive(false);
        titleCanvas.SetActive(true);
    }

    public void GoToLevelSelect()
    {
        titleCanvas.SetActive(false);
        levelSelectCanvas.SetActive(true);
    }

    public void Quit()
    {
        LevelSelectManager.Instance.SaveData();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
