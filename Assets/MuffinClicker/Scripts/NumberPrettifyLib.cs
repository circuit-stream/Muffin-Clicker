using System;
using UnityEngine;

public static class NumberPrettifyLib
{
    public static string PrettifyNumber(double value)
    {
        int magnitude = value <= 1 ? 0 : Mathf.FloorToInt((float) Math.Log10(value));
        int magnitudeIndex = Mathf.FloorToInt(magnitude / 3f);

        double divisor = Math.Pow(10, magnitudeIndex * 3);

        double shortNumber = value / divisor;
        string digits = String.Format(magnitude >= 3 ? "{0:0.###}" : "{0:0}", shortNumber);


        return $"{digits} {GetNumberSuffix(magnitudeIndex)}";
    }

    private static string GetNumberSuffix(int magnitudeIndex)
    {
        switch(magnitudeIndex)
        {
            case 0:
                break;
            case 1:
                return "Thousand";
            case 2:
            {
                return "Million";
            }
            case 3:
                return "Billion";
            default:
                Debug.LogError("Unhandled prettify number magnitude!");
                return $"* 10^{magnitudeIndex * 3}";
        }

        return string.Empty;
    }
}