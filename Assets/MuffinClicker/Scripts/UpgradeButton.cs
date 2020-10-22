using MuffinClicker.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public Button button;
    public Image holderImage;
    public TMP_Text priceText;
    public TMP_Text levelText;
    public Image icon;
    public Image currencyIcon;

    public Color purchaseAvailableTextColor = new Color(80, 220, 65);
    public Color purchaseDisabledTextColor = new Color(230, 75, 90);

    public UpgradableType upgradableType;

    public double[] pricePerLevel = {5, 10, 50, 500, 10000};

    private double CurrentUpgradePrice =>
        currentLevel >= pricePerLevel.Length
            ? pricePerLevel[pricePerLevel.Length - 1]
            : pricePerLevel[currentLevel];

    private bool HasEnoughMuffins => gameManager.MuffinAmount >= CurrentUpgradePrice;

    private int currentLevel;
    private GameManager gameManager;

    public void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnUpgradableLevelChanged += OnUpgradableLevelChanged;

        currentLevel = gameManager.GetUpgradableLevel(upgradableType);
        SetLevelInfo();

        button.onClick.AddListener(OnClick);
    }

    public void OnDestroy()
    {
        gameManager.OnUpgradableLevelChanged -= OnUpgradableLevelChanged;
    }

    private void OnUpgradableLevelChanged(UpgradableType changedType, int newLevel)
    {
        if (changedType != upgradableType)
            return;

        currentLevel = newLevel;
        SetLevelInfo();
    }

    public void Update()
    {
        var hasEnoughMuffins = HasEnoughMuffins;
        var tintColor = hasEnoughMuffins ? Color.white : Color.gray;

        button.interactable = hasEnoughMuffins;
        holderImage.color = tintColor;
        icon.color = tintColor;
        currencyIcon.color = tintColor;
        priceText.color = hasEnoughMuffins ? purchaseAvailableTextColor : purchaseDisabledTextColor;
    }

    private void SetLevelInfo()
    {
        levelText.text = currentLevel.ToString();
        priceText.text = NumberPrettifyLib.PrettifyNumber(CurrentUpgradePrice);
    }

    private void OnClick()
    {
        gameManager.IncreaseUpgradableLevel(upgradableType, CurrentUpgradePrice);
    }
}