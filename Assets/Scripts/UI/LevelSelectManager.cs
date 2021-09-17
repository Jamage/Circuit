using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine.UI;
public class LevelSelectManager : GenericSingletonClass<LevelSelectManager>
{
    public List<LevelData> levelDataList;
    public GameObject LevelSelectOptions;
    public LevelSelectButton LevelSelectButtonPrefab;
    public static LevelData selectedLevel;
    public List<LevelCompletionSaveData> levelCompletionData;
    public Dictionary<string, LevelCompletionSaveData> levelSaveDataDictionary;
    public static UnityAction OnLevelComplete;
    private string SaveDataFilePath => $"{Application.persistentDataPath}\\levelSaveData.json";
    public Button backButton;

    protected override void Awake()
    {
        base.Awake();
        levelSaveDataDictionary = new Dictionary<string, LevelCompletionSaveData>();
        levelCompletionData = new List<LevelCompletionSaveData>();
        //If no file, create one with all level data
        if (File.Exists(SaveDataFilePath) == false)
            CreateFreshSaveData();
        LoadData();
        GenerateLevelButtons();
        gameObject.SetActive(false);
    }

    private void LoadData()
    {
        //File exists, load content
        String[] jsonData = File.ReadAllLines(SaveDataFilePath);

        foreach (String jsonLine in jsonData)
        {
            LevelCompletionSaveData saveData = JsonUtility.FromJson<LevelCompletionSaveData>(jsonLine);
            if (levelSaveDataDictionary.ContainsKey(saveData.LevelName))
            {
                levelSaveDataDictionary.Remove(saveData.LevelName);
                LevelCompletionSaveData dataToRemove = levelCompletionData.First(data => data.LevelName == saveData.LevelName);
                levelCompletionData.Remove(dataToRemove);
            }
            
            levelSaveDataDictionary.Add(saveData.LevelName, saveData);
            levelCompletionData.Add(saveData);

        }

        if (jsonData.Length < levelDataList.Count) //Append new level data to save file
            AppendNewLevelSaveData(jsonData);
    }

    private void AppendNewLevelSaveData(String[] jsonData)
    {
        StringBuilder jsonStringBuild = new StringBuilder();
        foreach (string json in jsonData)
            jsonStringBuild.AppendLine(json);

        foreach (LevelData levelData in levelDataList)
        {
            if (levelSaveDataDictionary.ContainsKey(levelData.Name))
                continue;

            LevelCompletionSaveData newSaveData = new LevelCompletionSaveData(levelData);
            string levelJson = JsonUtility.ToJson(newSaveData);
            jsonStringBuild.AppendLine(levelJson);
            levelCompletionData.Add(newSaveData);
            levelSaveDataDictionary.Add(newSaveData.LevelName, newSaveData);
        }

        File.WriteAllText(SaveDataFilePath, jsonStringBuild.ToString());
    }

    private void CreateFreshSaveData()
    {
        StringBuilder jsonData = new StringBuilder();
        foreach (LevelData levelData in levelDataList)
        {
            LevelCompletionSaveData saveData = new LevelCompletionSaveData(levelData);
            string json = JsonUtility.ToJson(saveData);
            jsonData.AppendLine(json);
            levelCompletionData.Add(saveData);
            levelSaveDataDictionary.Add(saveData.LevelName, saveData);
        }
        File.WriteAllText(SaveDataFilePath, jsonData.ToString());
    }

    public void SaveData()
    {
        StringBuilder jsonData = new StringBuilder();
        foreach (LevelCompletionSaveData saveData in levelSaveDataDictionary.Values)
        {
            string json = JsonUtility.ToJson(saveData);
            jsonData.AppendLine(json);
        }

        File.WriteAllText(SaveDataFilePath, jsonData.ToString());
    }

    private void GenerateLevelButtons()
    {
        foreach (LevelData levelData in levelDataList)
        {
            LevelSelectButton newButton = Instantiate(LevelSelectButtonPrefab, Vector3.zero, LevelSelectButtonPrefab.transform.rotation, LevelSelectOptions.transform);
            LevelCompletionSaveData saveData = new LevelCompletionSaveData(levelData);
            if (levelSaveDataDictionary.ContainsKey(levelData.Name))
                saveData = levelSaveDataDictionary[levelData.Name];
            newButton.Setup(saveData);
            newButton.levelSelectButton.onClick.AddListener(() => SelectLevel(levelData.Name));
        }
    }

    public void SelectLevel(string levelName)
    {
        Debug.Log($"Name: {levelName}");
        selectedLevel = levelDataList.First(level => level.Name == levelName);
        StartCoroutine(LoadScene(selectedLevel));
    }

    private IEnumerator LoadScene(LevelData selectedLevel)
    {
        var progress = SceneManager.LoadSceneAsync("SampleScene");
        while (progress.isDone == false)
        {
            //do nothing, wait
            yield return null;
        }

        //Scene Loaded, get GameBoard and set up level
        GameBoard gameBoard = GameObject.Find("Game Board").GetComponent<GameBoard>();
        gameBoard.Setup(selectedLevel);
        gameObject.SetActive(false);
    }

    public void SetLevelComplete()
    {
        LevelCompletionSaveData saveData = levelSaveDataDictionary[selectedLevel.Name];
        saveData.SetComplete();
        SaveData();
        LoadData();
    }

    public void ShowAndSetBackButton(GameObject toggleForBackButton_OnClick)
    {
        gameObject.SetActive(true);
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(() => Disable());
        backButton.onClick.AddListener(() => toggleForBackButton_OnClick.SetActive(true));
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
