using System;
using TMPro;
using UnityEngine;

public class MuffinClickRewardAnimationInfo
{
    public RectTransform Transform;
    public TMP_Text RewardText;

    public float ElapsedTime;

    public void Destroy()
    {
        GameObject.Destroy(Transform.gameObject);
    }

    public MuffinClickRewardAnimationInfo(RectTransform transform, int reward)
    {
        Transform = transform;

        RewardText = transform.GetComponent<TMP_Text>();
        RewardText.text = String.Format("+{0}", reward);

        ElapsedTime = 0;
    }
}