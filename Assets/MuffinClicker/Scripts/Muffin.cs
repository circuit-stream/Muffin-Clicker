using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Muffin : MonoBehaviour
{
    public int MuffinPerClick = 1;
    public TMP_Text MuffinAmountText;
    private double muffinAmount;

    public RectTransform[] Spinlights;
    public float SpinlightVelocity;

    public Vector2 BottomLeftFeedbackSpawnPosition;
    public Vector2 TopRightFeedbackSpawnPosition;

    public RectTransform MuffinClickRewardPrefab;
    public float MuffinClickRewardAnimationDuration = 1f;
    private const float muffinClickRewardSpeed = 40;
    private List<MuffinClickRewardAnimationInfo> clickRewardAnimationInfos;

    private RectTransform myRectTransform;

    public void OnClick()
    {
        AddMuffins(MuffinPerClick);
        ClickRewardFeedback();
    }

    public void Update()
    {
        AnimateSpinlights();
        AnimateClickRewards();
    }

    public void Start()
    {
        SetMuffins(0);

        myRectTransform = GetComponent<RectTransform>();
        clickRewardAnimationInfos = new List<MuffinClickRewardAnimationInfo>();
    }

    private void AddMuffins(double addedValue)
    {
        SetMuffins(muffinAmount + addedValue);
    }

    private void SetMuffins(double newValue)
    {
        muffinAmount = newValue;
        MuffinAmountText.text = muffinAmount + " muffins";
    }

    private void AnimateSpinlights()
    {
        for (var index = 0; index < Spinlights.Length; index++)
        {
            var spinlight = Spinlights[index];

            float velocity = SpinlightVelocity;
            if (index % 2 == 0)
            {
                velocity *= 1.5f;
            }

            spinlight.Rotate(0, 0, velocity * Time.deltaTime);
        }
    }

    private void ClickRewardFeedback()
    {
        RectTransform newObject = GameObject.Instantiate(MuffinClickRewardPrefab, myRectTransform);

        newObject.rotation = Quaternion.identity;
        newObject.localPosition = RandomSpawnPosition();

        MuffinClickRewardAnimationInfo animationInfo = new MuffinClickRewardAnimationInfo(newObject, MuffinPerClick);
        clickRewardAnimationInfos.Add(animationInfo);
    }

    private void AnimateClickRewards()
    {
        List<MuffinClickRewardAnimationInfo> expiredAnimations = new List<MuffinClickRewardAnimationInfo>();

        foreach (MuffinClickRewardAnimationInfo clickRewardAnimationInfo in clickRewardAnimationInfos)
        {
            if (!AnimateClickReward(clickRewardAnimationInfo))
            {
                expiredAnimations.Add(clickRewardAnimationInfo);
            }
        }

        foreach (var clickRewardAnimationInfo in expiredAnimations)
        {
            clickRewardAnimationInfos.Remove(clickRewardAnimationInfo);
        }
    }

    private bool AnimateClickReward(MuffinClickRewardAnimationInfo animationInfo)
    {
        animationInfo.ElapsedTime += Time.deltaTime;
        animationInfo.Transform.anchoredPosition += Vector2.up * (Time.deltaTime * muffinClickRewardSpeed);

        Color rewardTextColor = animationInfo.RewardText.color;
        rewardTextColor.a = Mathf.Lerp(1, 0, animationInfo.ElapsedTime / MuffinClickRewardAnimationDuration);
        animationInfo.RewardText.color = rewardTextColor;

        if (!(animationInfo.ElapsedTime >= MuffinClickRewardAnimationDuration))
            return true;

        animationInfo.Destroy();
        return false;
    }

    private Vector2 RandomSpawnPosition()
    {
        float xPosition = Random.Range(BottomLeftFeedbackSpawnPosition.x, TopRightFeedbackSpawnPosition.x);
        float yPosition = Random.Range(BottomLeftFeedbackSpawnPosition.y, TopRightFeedbackSpawnPosition.y);

        return new Vector2(xPosition, yPosition);
    }
}
