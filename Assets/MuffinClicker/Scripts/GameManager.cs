using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private readonly string saveKey = "SaveKey";

    public float saveEveryXSeconds = 1;

    public double MuffinAmount
    {
        get => saveData.MuffinAmount;

        private set
        {
            saveData.MuffinAmount = value;
        }
    }

    private SaveData saveData;
    private bool autoSaveEnabled;

    private void Awake()
    {
        LoadSaveData();
        StartCoroutine(SaveCoroutine());
    }

    public void AddMuffins(double addedValue)
    {
        SetMuffins(MuffinAmount + addedValue);
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
