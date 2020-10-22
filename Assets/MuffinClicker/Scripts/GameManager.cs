using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private readonly string saveKey = "SaveKey";

    public static GameManager Instance { get; private set; }

    public float saveEveryXSeconds = 1;

    public double MuffinAmount
    {
        get => saveData.MuffinAmount;

        private set
        {
            saveData.MuffinAmount = value;
        }
    }

    public float MuffinMultiplier { get; set; }

    private SaveData saveData;
    private bool autoSaveEnabled;

    private void Awake()
    {
        if (Instance != null)
        {
            throw new Exception($"Multiple singleton instances! {Instance} :: {this}");
        }

        Instance = this;

        MuffinMultiplier = 1;
        LoadSaveData();
        StartCoroutine(SaveCoroutine());
    }

    public double AddMuffins(double requestedValue)
    {
        var addedValue = requestedValue * MuffinMultiplier;
        SetMuffins(MuffinAmount + addedValue);

        return addedValue;
    }

    private void SetMuffins(double newValue)
    {
        MuffinAmount = newValue;
    }

    public void OnApplicationPause(bool pauseStatus)
    {
        Save();
    }

    public void OnDestroy()
    {
        Save();
        autoSaveEnabled = false;
    }

    private IEnumerator SaveCoroutine()
    {
        autoSaveEnabled = true;

        do {
            yield return new WaitForSeconds(saveEveryXSeconds);

            Save();
        } while (autoSaveEnabled);
    }

    private void LoadSaveData()
    {
        if (PlayerPrefs.HasKey(saveKey))
        {
            string serializedSaveData = PlayerPrefs.GetString(saveKey);
            saveData = JsonUtility.FromJson<SaveData>(serializedSaveData);

            return;
        }

        saveData = new SaveData();
    }

    private void Save()
    {
        string serializedSaveData = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(saveKey, serializedSaveData);
    }
}
