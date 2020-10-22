using System;
using MuffinClicker.Enums;
using UnityEngine;
using UnityEngine.UI;

public abstract class Powerup : MonoBehaviour
{
    protected int currentUpgradableLevel;
    protected abstract UpgradableType upgradableType { get; }

    public Button powerupButton;

    public float[] CooldownDurationPerLevel = {0, 10, 9, 8, 7, 5};
    private float CooldownDuration =>
        currentUpgradableLevel >= CooldownDurationPerLevel.Length
            ? CooldownDurationPerLevel[CooldownDurationPerLevel.Length - 1]
            : CooldownDurationPerLevel[currentUpgradableLevel];


    public RectTransform CooldownBar;
    private float cooldownBarHeight;

    private float currentCooldown;

    protected bool IsAvailable => currentCooldown <= 0;

    public virtual void Start()
    {
        cooldownBarHeight = CooldownBar.sizeDelta.y;

        // TODO: Load cooldown from save
        SetCooldownBarHeight();

        powerupButton.onClick.AddListener(OnClick);

        var gameManager = GameManager.Instance;
        currentUpgradableLevel = gameManager.GetUpgradableLevel(upgradableType);
        gameManager.OnUpgradableLevelChanged += OnUpgradableLevelChanged;

        gameObject.SetActive(currentUpgradableLevel > 0);
    }

    public void OnDestroy()
    {
        GameManager.Instance.OnUpgradableLevelChanged -= OnUpgradableLevelChanged;
    }

    public virtual void Update()
    {
        UpdateCooldown();
    }

    protected abstract void PerformPowerupEffect();

    private void OnClick()
    {
        Debug.Assert(IsAvailable, "Trying to activate a unavailable powerup", gameObject);

        powerupButton.interactable = false;
        PerformPowerupEffect();
        ResetCooldown();
    }

    protected virtual void OnUpgradableLevelChanged(UpgradableType changedType, int newLevel)
    {
        if (changedType != upgradableType)
            return;

        gameObject.SetActive(true);
        currentUpgradableLevel = newLevel;
    }

    private void ResetCooldown()
    {
        currentCooldown = CooldownDuration;
    }

    private void UpdateCooldown()
    {
        if (IsAvailable)
        {
            return;
        }

        currentCooldown -= Time.deltaTime;
        currentCooldown = Mathf.Max(0, currentCooldown);

        SetCooldownBarHeight();

        powerupButton.interactable = IsAvailable;
    }

    private void SetCooldownBarHeight()
    {
        var fraction = currentCooldown / CooldownDuration;
        CooldownBar.sizeDelta = new Vector2(CooldownBar.sizeDelta.x, fraction * cooldownBarHeight);
    }
}