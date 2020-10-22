using MuffinClicker.Enums;
using UnityEngine;

public class MilkshakePowerup : Powerup
{
    public float[] MultiplierPerLevel = {0, 1, 1.5f, 2, 3, 5};
    private float MuffiinExtraMultiplier =>
        currentUpgradableLevel >= MultiplierPerLevel.Length
            ? MultiplierPerLevel[MultiplierPerLevel.Length - 1]
            : MultiplierPerLevel[currentUpgradableLevel];

    public float[] PowerupDurationPerLevel = {0, 3, 4, 5, 6, 7};
    private float PowerupDuration =>
        currentUpgradableLevel >= PowerupDurationPerLevel.Length
            ? PowerupDurationPerLevel[PowerupDurationPerLevel.Length - 1]
            : PowerupDurationPerLevel[currentUpgradableLevel];

    protected override UpgradableType upgradableType => UpgradableType.Milkshake;

    private float remainingPowerupDuration;

    public float BounceAmplitude = 0.1f;
    public float BounceFrequency = 10;

    private RectTransform myRectTransform;

    private bool IsPowerupActive => remainingPowerupDuration > 0;

    public override void Update()
    {
        if (IsPowerupActive)
        {
            UpdateActivePowerup();
            return;
        }

        base.Update();
    }

    public override void Start()
    {
        base.Start();

        myRectTransform = GetComponent<RectTransform>();
    }

    protected override void OnUpgradableLevelChanged(UpgradableType changedType, int newLevel)
    {
        if (IsPowerupActive)
            DisablePowerup();

        base.OnUpgradableLevelChanged(changedType, newLevel);

        if (IsPowerupActive)
            GameManager.Instance.MuffinMultiplier += MuffiinExtraMultiplier;
    }

    protected override void PerformPowerupEffect()
    {
        remainingPowerupDuration = PowerupDuration;

        GameManager.Instance.MuffinMultiplier += MuffiinExtraMultiplier;
    }

    private void DisablePowerup()
    {
        GameManager.Instance.MuffinMultiplier -= MuffiinExtraMultiplier;
    }

    private void UpdateActivePowerup()
    {
        if (!IsPowerupActive)
        {
            return;
        }

        remainingPowerupDuration -= Time.deltaTime;
        remainingPowerupDuration = Mathf.Max(0, remainingPowerupDuration);

        AnimatePowerup();

        if (!IsPowerupActive)
        {
            DisablePowerup();
        }
    }

    private void AnimatePowerup()
    {
        var elapsedTime = PowerupDuration - remainingPowerupDuration;
        var multiplier = IsPowerupActive ? 1 + Mathf.Sin(elapsedTime * BounceFrequency) * BounceAmplitude : 1;
        myRectTransform.localScale = Vector3.one * multiplier;
    }
}