using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TitleMenu : MonoBehaviour
{
    public GameObject titleCanvas;
    public GameObject howToPlay;

    public void GoToTitle()
    {
        LevelSelectManager.Instance.gameObject.SetActive(false);
        howToPlay.SetActive(false);
        titleCanvas.SetActive(true);
    }

    public void GoToLevelSelect()
    {
        titleCanvas.SetActive(false);
        LevelSelectManager.Instance.ShowAndSetBackButton(titleCanvas);
    }

    public void GoToHowToPlay()
    {
        titleCanvas.SetActive(false);
        howToPlay.SetActive(true);
    }

    public void Quit()
    {
        LevelSelectManager.Instance.SaveData();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#elif UNITY_WEBGL
        return;
#else
        Application.Quit();
#endif
    }
}
