using System;
using TMPro;
using UnityEngine;

public class Muffin : MonoBehaviour
{
    public int MuffinPerClick = 1;
    public TMP_Text MuffinAmountText;

    private double muffinAmount;

    public void OnClick()
    {
        AddMuffins(MuffinPerClick);
    }

    public void Awake()
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
