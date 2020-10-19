using System;
using TMPro;
using UnityEngine;

public class Muffin : MonoBehaviour
{
    public int MuffinPerClick = 1;
    public TMP_Text MuffinAmountText;

    public RectTransform Spinlight;
    public float SpinlightVelocity;

    private double muffinAmount;

    public void OnClick()
    {
        AddMuffins(MuffinPerClick);
    }

    public void Update()
    {
        Spinlight.Rotate(0, 0, SpinlightVelocity * Time.deltaTime);
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
}
