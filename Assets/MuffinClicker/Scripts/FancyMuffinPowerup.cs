using System.Collections;
using UnityEngine;

public class FancyMuffinPowerup : Powerup
{
    public int MuffinPerActivation = 100;

    public RectTransform IconTransform;

    public float AnimationDuration = 0.3f;
    public float WiggleAmplitude = 5;
    public float WiggleFrequency = 30;
    public float HeightMovedDuringAnimation = 7;
    private float snapBackAnimationDuration = 0.1f;
    private bool animating;

    public override void Update()
    {
        if (animating)
            return;

        base.Update();
    }

    protected override void PerformPowerupEffect()
    {
        GameManager.Instance.AddMuffins(MuffinPerActivation);
        StartCoroutine(AnimatePowerupActivation());
    }

    private IEnumerator AnimatePowerupActivation()
    {
        animating = true;
        float elapsedTime = 0;

        Vector2 startAnchoredPosition = IconTransform.anchoredPosition;
        Vector2 endAnchoredPosition = new Vector2(startAnchoredPosition.x, startAnchoredPosition.y + HeightMovedDuringAnimation);

        while (elapsedTime < AnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            var fraction = elapsedTime / AnimationDuration;

            var zRotation = Mathf.Sin(elapsedTime * WiggleFrequency) * WiggleAmplitude;
            IconTransform.localRotation = Quaternion.Euler(0, 0, zRotation);

            IconTransform.anchoredPosition = Vector2.Lerp(startAnchoredPosition, endAnchoredPosition, fraction);

            yield return null;
        }

        float startRotationZ = IconTransform.localRotation.z;

        for (float snapBackTime = 0; snapBackTime < snapBackAnimationDuration; snapBackTime += Time.deltaTime)
        {
            var fraction = snapBackTime / snapBackAnimationDuration;

            IconTransform.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(startRotationZ, 0, fraction));
            IconTransform.anchoredPosition = Vector2.Lerp(endAnchoredPosition, startAnchoredPosition, fraction);

            yield return new WaitForEndOfFrame();
        }

        animating = false;
    }
}