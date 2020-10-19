using System;
using TMPro;
using UnityEngine;

public class Muffin : MonoBehaviour
{
    public int MuffinPerClick = 1;
    public TMP_Text MuffinAmountText;

    public RectTransform[] Spinlights;
    public float SpinlightVelocity;

    private double muffinAmount;

    public void OnClick()
    {
        AddMuffins(MuffinPerClick);
    }

    public void Update()
    {
        AnimateSpinlights();
    }

    public void Start()
    {
        SetMuffins(0);
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
}
