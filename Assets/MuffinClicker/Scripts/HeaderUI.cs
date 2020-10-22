using System.Collections;
using MuffinClicker.Enums;
using TMPro;
using UnityEngine;

public class HeaderUI : MonoBehaviour
{
    public TMP_Text MuffinAmountText;
    public TMP_Text MuffinPerSecondText;
    public Muffin Muffin;

    private GameManager gameManager;

    public void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnUpgradableLevelChanged += OnUpgradableLevelChanged;
        SetTexts();
        StartCoroutine(nameof(UpdateMuffinPerSecondText));
    }

    public void OnDestroy()
    {
        gameManager.OnUpgradableLevelChanged -= OnUpgradableLevelChanged;
    }

    public void Update()
    {
        SetTexts();
    }

    private void SetTexts()
    {
        MuffinAmountText.text = $"{NumberPrettifyLib.PrettifyNumber(gameManager.MuffinAmount)} muffins";
    }

    private void OnUpgradableLevelChanged(UpgradableType changedType, int newLevel)
    {
        if (changedType != UpgradableType.Muffin)
            return;

        StartCoroutine(nameof(UpdateMuffinPerSecondText));
    }

    private IEnumerator UpdateMuffinPerSecondText()
    {
        yield return null; // Making sure Muffin updated it's upgradableLevel

        MuffinPerSecondText.text = $"{NumberPrettifyLib.PrettifyNumber(Muffin.MuffinPerSecond, true)} muffins / second";
    }
}