using UnityEngine;
using UnityEngine.UI;

public abstract class Powerup : MonoBehaviour
{
    public Button powerupButton;

    public float CooldownDuration = 3;
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