using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TitleMenu : MonoBehaviour
{
    public GameObject titleCanvas;

    public void GoToTitle()
    {
        LevelSelectManager.Instance.gameObject.SetActive(false);
        titleCanvas.SetActive(true);
    }

    public void GoToLevelSelect()
    {
        titleCanvas.SetActive(false);
        LevelSelectManager.Instance.gameObject.SetActive(true);
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
