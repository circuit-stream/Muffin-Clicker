using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Muffin : MonoBehaviour
{
    public int MuffinPerClick = 1;

    public RectTransform[] Spinlights;
    public float SpinlightVelocity;

    public Vector2 BottomLeftFeedbackSpawnPosition;
    public Vector2 TopRightFeedbackSpawnPosition;

    public RectTransform MuffinClickRewardPrefab;
    public float MuffinClickRewardAnimationDuration = 1f;
    private const float muffinClickRewardSpeed = 40;
    private List<MuffinClickRewardAnimationInfo> clickRewardAnimationInfos;

    public LittleMuffin LittleMuffinPrefab;

    public float bounceVelocity = 20;
    public AnimationCurve AmplitudeCurve;
    public RectTransform MuffinButton;
    private Coroutine muffinButtonAnimationCoroutine;
    private float muffinButtonAnimationDuration;

    private RectTransform myRectTransform;

    public void OnClick()
    {
        GameManager.Instance.AddMuffins(MuffinPerClick);
        ClickRewardFeedback();
        CreateLittleMuffin();
        StartMuffinButtonAnimation();
    }

    public void Update()
    {
        AnimateSpinlights();
        AnimateClickRewards();
    }

    public void Start()
    {
        myRectTransform = GetComponent<RectTransform>();
        clickRewardAnimationInfos = new List<MuffinClickRewardAnimationInfo>();
        muffinButtonAnimationDuration = AmplitudeCurve.keys[AmplitudeCurve.length - 1].time;
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

    private void CreateLittleMuffin()
    {
        LittleMuffin newObject = GameObject.Instantiate(LittleMuffinPrefab, myRectTransform);
        newObject.Setup(RandomSpawnPosition());
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

    private void StartMuffinButtonAnimation()
    {
        if (muffinButtonAnimationCoroutine != null)
        {
            StopCoroutine(muffinButtonAnimationCoroutine);
        }

        muffinButtonAnimationCoroutine = StartCoroutine(MuffinButtonAnimation());
    }

    private IEnumerator MuffinButtonAnimation()
    {
        float elapsedTime = 0;

        while (elapsedTime < muffinButtonAnimationDuration)
        {
            // Dividing by 100 so we can use a bigger scale for the curve, and see the setup better in the editor
            var amplitude = AmplitudeCurve.Evaluate(elapsedTime) / 100;
            MuffinButton.localScale = (1 + Mathf.Sin(elapsedTime * bounceVelocity) * amplitude) * Vector3.one;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private Vector2 RandomSpawnPosition()
    {
        float xPosition = Random.Range(BottomLeftFeedbackSpawnPosition.x, TopRightFeedbackSpawnPosition.x);
        float yPosition = Random.Range(BottomLeftFeedbackSpawnPosition.y, TopRightFeedbackSpawnPosition.y);

        return new Vector2(xPosition, yPosition);
    }
}
