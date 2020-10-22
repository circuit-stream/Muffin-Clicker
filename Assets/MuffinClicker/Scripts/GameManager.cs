using System;
using System.Collections;
using MuffinClicker.Enums;
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

    public delegate void OnUpgradableLevelChangedDelegate(UpgradableType changedUpgradableType, int newLevel);
    public OnUpgradableLevelChangedDelegate OnUpgradableLevelChanged;

    public int GetUpgradableLevel(UpgradableType upgradableType)
    {
        return saveData.GetUpgradableLevel(upgradableType);
    }

    public void IncreaseUpgradableLevel(UpgradableType upgradableType, double muffinCost, int amount = 1)
    {
        if (muffinCost > MuffinAmount)
            throw new Exception("Trying to buy upgrade with insufficient muffins");

        MuffinAmount -= muffinCost;

        var newLevel = GetUpgradableLevel(upgradableType) + amount;
        saveData.SetUpgradableLevel(upgradableType, newLevel);

        if (OnUpgradableLevelChanged != null)
        {
            OnUpgradableLevelChanged(upgradableType, newLevel);
        }
    }

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
            saveData = SaveData.Deserialize(serializedSaveData);

            return;
        }

        saveData = new SaveData(true);
    }

    private void Save()
    {
        string serializedSaveData = saveData.Serialize();
        PlayerPrefs.SetString(saveKey, serializedSaveData);
    }
}
