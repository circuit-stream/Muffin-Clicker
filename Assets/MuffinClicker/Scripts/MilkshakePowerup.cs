using UnityEngine;

public class MilkshakePowerup : Powerup
{
    public float MuffiinExtraMultiplier = 1;
    public float PowerupDuration = 3;

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