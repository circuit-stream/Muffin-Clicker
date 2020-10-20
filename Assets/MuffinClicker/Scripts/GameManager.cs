using UnityEngine;

public class GameManager : MonoBehaviour
{
    public double MuffinAmount { get; private set; }

    public void AddMuffins(double addedValue)
    {
        SetMuffins(MuffinAmount + addedValue);
    }

    private void SetMuffins(double newValue)
    {
        MuffinAmount = newValue;
    }
}