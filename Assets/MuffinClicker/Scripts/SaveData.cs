using System;
using System.Collections.Generic;
using MuffinClicker.Enums;
using UnityEngine;

[Serializable]
public class SaveData
{
    [Serializable]
    private class UpgradableSaveData
    {
        public UpgradableType Type;
        public int Level;
    }

    public double MuffinAmount;

    [SerializeField]
    private List<UpgradableSaveData> upgradableSaveData;

    private Dictionary<UpgradableType, UpgradableSaveData> upgradableLevels;

    public int GetUpgradableLevel(UpgradableType upgradableType)
    {
        return upgradableLevels[upgradableType].Level;
    }

    public void SetUpgradableLevel(UpgradableType upgradableType, int level)
    {
        upgradableLevels[upgradableType].Level = level;
    }

    public SaveData()
    {
        upgradableSaveData = new List<UpgradableSaveData>();
        upgradableLevels = new Dictionary<UpgradableType, UpgradableSaveData>();
    }

    public SaveData(bool createDefaults) : this()
    {
        foreach (UpgradableType upgradableType in Enum.GetValues(typeof(UpgradableType)))
        {
            upgradableLevels[upgradableType] = new UpgradableSaveData
            {
                Type = upgradableType,
                Level = upgradableType == UpgradableType.Muffin ? 1 : 0
            };
        }
    }

    public string Serialize()
    {
        upgradableSaveData.Clear();
        foreach (var pair in upgradableLevels)
        {
            upgradableSaveData.Add(pair.Value);
        }

        return JsonUtility.ToJson(this);
    }

    public static SaveData Deserialize(string jsonString)
    {
        SaveData newSaveData = JsonUtility.FromJson<SaveData>(jsonString);

        foreach (var data in newSaveData.upgradableSaveData)
        {
            newSaveData.upgradableLevels.Add(data.Type, data);
        }

        return newSaveData;
    }
}